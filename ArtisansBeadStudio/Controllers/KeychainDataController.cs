﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ArtisansBeadStudio.Migrations;
using ArtisansBeadStudio.Models;
using Microsoft.AspNet.Identity;
using System.IO;

namespace ArtisansBeadStudio.Controllers
{
    public class KeychainDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        /// <summary>
        /// Returns all the keychains in the system.
        /// </summary>
        /// <returns>
        /// All the keychains in the system with the name of it
        /// </returns>
        /// <example>
        /// GET: api/KeychainData/ListKeychains
        /// </example>
        [HttpGet]
        [ResponseType(typeof(KeychainDto))]
        [Authorize(Roles ="Admin,Guest")]
        public IHttpActionResult ListKeychains()
        {
            bool isAdmin = User.IsInRole("Admin");

            //set the admin can see all the keychains, and guest users can only see their own
            List<Keychain> Keychains; 
            if(isAdmin) Keychains = db.Keychains.ToList();
            else
            {
                string UserId = User.Identity.GetUserId();
                Keychains = db.Keychains.Where(k => k.UserID== UserId).ToList();
            }
            List<KeychainDto> KeychainDtos = new List<KeychainDto>();

            Keychains.ForEach(k => KeychainDtos.Add(new KeychainDto()
            {
                KeychainId = k.KeychainId,
                KeychainName = k.KeychainName
            }));

            return Ok(KeychainDtos);
        }


        /// <summary>
        /// Returns all the keychains contain a specific bead.
        /// </summary>
        /// <param name="id">Bead primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all keychains in the database that contain a particular bead
        /// </returns>
        /// <example>
        /// GET: api/KeychainData/ListKeychainsForBead/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(KeychainDto))]
        public IHttpActionResult ListKeychainsForBead(int id)
        {
            List<Keychain> Keychains = db.Keychains.Where(
                k => k.Beads.Any(
                    b => b.BeadId == id)
                ).ToList();
            List<KeychainDto> KeychainDtos = new List<KeychainDto>();

            Keychains.ForEach(k => KeychainDtos.Add(new KeychainDto()
            {
                KeychainId = k.KeychainId,
                KeychainName = k.KeychainName
            }));

            return Ok(KeychainDtos);
        }



        /// <summary>
        /// Returns all the keychains NOT contain a specific bead.
        /// </summary>
        /// <param name="id">Bead primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all keychains in the database that NOT contain a particular bead
        /// </returns>
        /// <example>
        /// GET: api/KeychainData/ListKeychainsNotIncludeBead/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(KeychainDto))]
        public IHttpActionResult ListKeychainsNotIncludeBead(int id)
        {
            List<Keychain> Keychains = db.Keychains.Where(
                k => !k.Beads.Any(
                    b => b.BeadId == id)
                ).ToList();
            List<KeychainDto> KeychainDtos = new List<KeychainDto>();

            Keychains.ForEach(k => KeychainDtos.Add(new KeychainDto()
            {
                KeychainId = k.KeychainId,
                KeychainName = k.KeychainName
            }));

            return Ok(KeychainDtos);
        }


        /// <summary>
        /// Associate a particular keychain with a particular style
        /// </summary>
        /// <param name="keychainid">The keychain id (primary key)</param>
        /// <param name="styleid">The style id(primary key)</param>
        /// <returns>
        /// HEADER: 200(OK)
        /// OR
        /// HEADER: 404(NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/KeychainData/AssociateKeychainWithStyle/{keychainid}/{styleid}
        /// curl -d "" -v https://localhost:44383/api/KeychainData/AssociateKeychainWithStyle/1/2
        /// </example>
        /// (collaboration part) 
        [HttpPost]
        [Route("api/KeychainData/AssociateKeychainWithStyle/{keychainid}/{styleid}")]
        public IHttpActionResult AssociateKeychainWithStyle(int keychainid, int styleid)
        {
            Keychain SelectedKeychain = db.Keychains.Include(k=>k.Styles).Where(k=>k.KeychainId== keychainid).FirstOrDefault();
            Style SelectedStyle = db.Styles.Find(styleid);
            if (SelectedKeychain==null || SelectedStyle == null)
            {
                return NotFound();
            };

            SelectedKeychain.Styles.Add(SelectedStyle);
            db.SaveChanges();

            return Ok();
        }



        /// <summary>
        /// Remove an association between a particular keychain and a particular style
        /// </summary>
        /// <param name="keychainid">The keychain id (primary key)</param>
        /// <param name="styleid">The style id(primary key)</param>
        /// <returns>
        /// HEADER: 200(OK)
        /// OR
        /// HEADER: 404(NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/KeychainData/RemoveKeychainWithStyle/{keychainid}/{styleid}
        /// curl -d "" -v https://localhost:44383/api/KeychainData/RemoveKeychainWithStyle/1/2
        /// </example>
        /// (collaboration part) 
        [HttpPost]
        [Route("api/KeychainData/RemoveKeychainWithStyle/{keychainid}/{styleid}")]
        public IHttpActionResult RemoveKeychainWithStyle(int keychainid, int styleid)
        {
            Keychain SelectedKeychain = db.Keychains.Include(k => k.Styles).Where(k => k.KeychainId == keychainid).FirstOrDefault();
            Style SelectedStyle = db.Styles.Find(styleid);
            if (SelectedKeychain == null || SelectedStyle == null)
            {
                return NotFound();
            };

            SelectedKeychain.Styles.Remove(SelectedStyle);
            db.SaveChanges();

            return Ok();
        }


        /// <summary>
        /// Gather all the keychains related to a particular style
        /// </summary>
        /// <param name="id">Style ID (primary key)</param>
        /// <returns>
        /// All the keychains in the system that match to a particular style ID
        /// </returns>
        /// <example>
        /// GET: api/keychainData/ListKeychainsForStyle/1
        /// </example>
        /// (collaboration part)
        [HttpGet]
        [ResponseType(typeof(KeychainDto))]
        public IHttpActionResult ListKeychainsForStyle(int id)
        {
            List<Keychain> Keychains = db.Keychains.Where(
                k => k.Styles.Any(
                    s => s.StyleID == id
                )).ToList();
            List<KeychainDto> KeychainDtos = new List<KeychainDto>();

            Keychains.ForEach(k => KeychainDtos.Add(new KeychainDto()
            {
                KeychainId = k.KeychainId,
                KeychainName = k.KeychainName
            }));


            return Ok(KeychainDtos);
        }



        /// <summary>
        /// Return the properties of one specific chosen bead in the system
        /// </summary>
        /// <param name="id">The BeadId of that chosen bead</param>
        /// <returns>
        /// All the properties releated to that chosen bead, including the bead description, bead name, bead picture(name of the picture), bead colour and the colour property
        /// </returns>
        /// <example>
        /// GET: api/KeychainData/FindKeychain/3
        /// </example> 
        [HttpGet]
        [ResponseType(typeof(Keychain))]
        [Authorize(Roles ="Admin,Guest")]
        public IHttpActionResult FindKeychain(int id)
        {
            Keychain Keychain = db.Keychains.Find(id);
            KeychainDto KeychainDto = new KeychainDto()
            {
                KeychainId = Keychain.KeychainId,
                KeychainName = Keychain.KeychainName
            };

            if (Keychain == null)
            {
                return NotFound();
            }

            //not process if the user is not admin and the keychain doesn't belong to that user
            bool isAdmin = User.IsInRole("Admin");
            if(!isAdmin && (Keychain.UserID!=User.Identity.GetUserId())) return StatusCode(HttpStatusCode.Forbidden);

            return Ok(KeychainDto);
        }


        /// <summary>
        /// Update a specific keychain in the system with POST Data input
        /// </summary>
        /// <param name="id">The primary key of the specific keychain</param>
        /// <param name="keychain">JSON FORM DATA of a keychain</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/KeychainData/UpdateKeychain/5
        /// </example>
        /// <example>
        /// CREATE: A keychainupdate.json file, with JSON FORM DATA that want to input into system
        ///        {
        ///             "KeychainId": 3,
        ///             "KeychainName": "dog"
        ///         }
        /// COMMAND WINDOW: curl -d @keychainupdate.json -H "Content-type:application/json" https://localhost:44386/api/KeychainData/UpdateKeychain/3
        /// </example>
        // PUT: api/KeychainData/UpdateKeychain/5
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize(Roles ="Admin,Guest")]
        public IHttpActionResult UpdateKeychain(int id, Keychain keychain)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != keychain.KeychainId)
            {
                return BadRequest();
            }

            //check the admin status, not process if the user is not an admin, and if the keychain is not belonging to that user
            bool isAdmin = User.IsInRole("Admin");
            
            if (!isAdmin && (keychain.UserID != User.Identity.GetUserId()))
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

            db.Entry(keychain).State = EntityState.Modified;
            db.Entry(keychain).Property(k => k.UserID).IsModified = false;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KeychainExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        /// <summary>
        /// Add a keychain in the system
        /// </summary>
        /// <param name="keychain">JSON FORM DATA of a keychain</param>
        /// <returns>
        /// Header: 201(Created)
        /// Content: KeychainId, Keychain Data 
        /// or
        /// Header: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/KeychainData/AddKeychain
        /// </example>
        /// <example>
        /// CREATE: A keychain.json file, with JSON FORM DATA that want to input into system
        ///        {
        ///             "KeychainName": "Cat"
        ///         }
        /// COMMAND WINDOW: curl -d @keychain.json -H "Content-type:application/json" https://localhost:44386/api/KeychainData/AddKeychain      
        /// RETURN:{"KeychainId":3,"KeychainName":"Cat","Beads":null}
        /// </example>
        [ResponseType(typeof(Keychain))]
        [HttpPost]
        [Authorize(Roles ="Admin,Guest")]
        public IHttpActionResult AddKeychain(Keychain keychain)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //attach the id
            keychain.UserID = User.Identity.GetUserId();

            db.Keychains.Add(keychain);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = keychain.KeychainId }, keychain);
        }


        /// <summary>
        /// Delete one specific keychain from the system by it's ID
        /// </summary>
        /// <param name="id">The primary key of the keychain that you want to delete</param>
        /// <returns>
        /// Header: 200 (OK, if it deletes successfully)
        /// or
        /// Header: 404 (NOT FOUND, if it deletes NOT successfully)
        /// </returns>
        /// <example>
        /// Post: api/KeychainData/DeleteKeychain/5
        /// curl -d "" "https://localhost:44386/api/KeychainData/DeleteKeychain/5"
        /// Form data: (empty)
        /// </example>
        [HttpPost]
        [ResponseType(typeof(Keychain))]
        [Authorize(Roles ="Admin,Guest")]
        public IHttpActionResult DeleteKeychain(int id)
        {
            Keychain keychain = db.Keychains.Find(id);
            if (keychain == null)
            {
                return NotFound();
            }

            bool isAdmin = User.IsInRole("Admin");
            if (!isAdmin && (keychain.UserID != User.Identity.GetUserId())) return StatusCode(HttpStatusCode.Forbidden);

            db.Keychains.Remove(keychain);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool KeychainExists(int id)
        {
            return db.Keychains.Count(e => e.KeychainId == id) > 0;
        }
    }
}