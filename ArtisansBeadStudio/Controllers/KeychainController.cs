﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using ArtisansBeadStudio.Models;
using System.Web.Script.Serialization;
using ArtisansBeadStudio.Models.ViewModels;
using System.Diagnostics;

namespace ArtisansBeadStudio.Controllers
{
    public class KeychainController : Controller
    {

        //code factoring
        private static readonly HttpClient client;
        static KeychainController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false
            };

            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44383/api/");
        }
        private JavaScriptSerializer jss = new JavaScriptSerializer();




        /// <summary>
        /// Grabs the authentication cookie sent to this controller.
        /// </summary>
        private void GetApplicationCookie()
        {
            string token = "";
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }


        /// <summary>
        /// Accessing information from keychain api controller to get the list of all the keychains
        /// </summary>
        /// <returns>List of keychains</returns>
        /// url: https://localhost:44386/api/KeychainData/ListKeychains
        /// <example>
        /// GET: Keychain/List
        /// </example>
        [Authorize(Roles ="Admin,Guest")]
        public ActionResult List()
        {
            GetApplicationCookie();
            string url = "KeychainData/ListKeychains";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<KeychainDto> keychains = response.Content.ReadAsAsync<IEnumerable<KeychainDto>>().Result;

            return View(keychains);
        }

        /// <summary>
        /// Accessing information from bead api controller to get the information of a specific keychain
        /// </summary>
        /// <param name="id">Primary key of a specific keychain</param>
        /// <returns>The information of that specific keychain</returns>
        /// url: https://localhost:44386/api/KeychainData/FindKeychain/{id}
        ///<example>
        /// GET: Keychain/Details/{id}
        /// </example>
        [Authorize(Roles ="Admin,Guest")]
        public ActionResult Details(int id)
        {
            GetApplicationCookie();
            DetailsKeychain ViewModels = new DetailsKeychain();

            string url = "KeychainData/FindKeychain/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            KeychainDto specificKeychain = response.Content.ReadAsAsync<KeychainDto>().Result;
            ViewModels.specificKeychain = specificKeychain;


            //also show all the beads under this keychian
            url = "BeadData/ListBeadsForKeychain/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<BeadDto> beadsInKeychain = response.Content.ReadAsAsync<IEnumerable<BeadDto>>().Result;
            ViewModels.beadsInKeychain = beadsInKeychain;

            //also show all the associated styles with this specific keychain(collaboration)
            url = "StyleData/ListStylesForKeychain/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<StyleDto> associatedStyles = response.Content.ReadAsAsync<IEnumerable<StyleDto>>().Result;
            ViewModels.associatedStyles = associatedStyles;

            //also show all the aviliable styles(collaboration)
            url = "StyleData/ListStylesNotIncludeKeychain/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<StyleDto> aviliableStyles = response.Content.ReadAsAsync<IEnumerable<StyleDto>>().Result;
            ViewModels.aviliableStyles = aviliableStyles;

            return View(ViewModels);
        }


        //POST: Keychain/Associate/{id}
        [HttpPost]
        [Authorize(Roles ="Admin,Guest")]
        public ActionResult Associate(int id, int styleid)
        {
            //get token to check the authorization
            GetApplicationCookie();
            string url = "KeychainData/AssociateKeychainWithStyle/" + id + "/" + styleid;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details/" + id);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        //Get: Keychain/UnAssociate/{id}?StyleID={styleid}
        [HttpGet]
        [Authorize(Roles ="Admin,Guest")]
        public ActionResult UnAssociate(int id, int styleid)
        {
            GetApplicationCookie();
            string url = "KeychainData/RemoveKeychainWithStyle/"+id+"/"+styleid;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }



        /// <summary>
        /// The MVC5 view called New.cshtml has a form to collect data for creating a new keychain
        /// </summary>
        /// <returns>Send collected data to Create Method</returns>
        /// GET: Keychain/Create
        [Authorize(Roles = "Admin,Guest")]
        public ActionResult New()
        {
            return View();
        }

        /// <summary>
        /// Add a new keychain to the system using the API
        /// </summary>
        /// <param name="keychain">Keychain class</param>
        /// curl -d @keychain.json -H "Content-type:application/json" https://localhost:44386/api/KeychainData/AddKeychain    
        /// <returns>
        /// Successful: redirect to List page
        /// Fail: redirect to Error page
        /// </returns>
        /// POST: Keychain/Create
        [HttpPost]
        [Authorize(Roles ="Admin,Guest")]
        public ActionResult Create(Keychain keychain)
        {
            GetApplicationCookie();
            string url = "KeychainData/AddKeychain";
            string jsonpayload = jss.Serialize(keychain);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        /// <summary>
        /// The MVC5 view called Error.cshtml will hold error message
        /// </summary>
        /// GET: Keychain/Error
        public ActionResult Error()
        {
            return View();
        }


        /// <summary>
        /// The MVC5 view called Edit.cshtml has a form to collect the updating data for a sepcific keychain.
        /// </summary>
        /// <param name="id">The specific keychain primary key</param>
        /// <returns>Send collected data to Update Method</returns>
        /// GET: Keychain/Edit/{id}
        [Authorize(Roles = "Admin,Guest")]
        public ActionResult Edit(int id)
        {
            string url = "KeychainData/FindKeychain/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            KeychainDto specificKeychain = response.Content.ReadAsAsync<KeychainDto>().Result;
            return View(specificKeychain);
        }

        /// <summary>
        /// Update the information of a specific keychain to the system using the API
        /// </summary>
        /// <param name="id">The specific keychain primary key</param>
        /// <param name="bead">Keychain class</param>
        /// curl -d @keychainupdate.json -H "Content-type:application/json" https://localhost:44386/api/KeychainData/UpdateKeychain/3
        /// <returns>
        /// Successful: redirect to List page
        /// Fail: redirect to Error page
        /// </returns>
        /// POST: Keychain/Update/{id}
        [HttpPost]
        [Authorize(Roles = "Admin,Guest")]
        public ActionResult Update(int id, Keychain keychain)
        {
            GetApplicationCookie();
            string url = "KeychainData/UpdateKeychain/" + id;
            string jsonpayload = jss.Serialize(keychain);
            Debug.WriteLine("json payload:" + jsonpayload);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        /// <summary>
        /// The MVC5 view called DeleteConfirm.cshtml to confirm with user whether they want to delete the selected keychain
        /// </summary>
        /// <param name="id">The selected keychain primary key</param>
        /// <returns>send the confirm answer to Delete method</returns>
        /// GET: Keychain/Delete/{id}
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "KeychainData/FindKeychain/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            KeychainDto selectedKeychain = response.Content.ReadAsAsync<KeychainDto>().Result;
            return View(selectedKeychain);
        }



        /// <summary>
        /// Delete the specific keychain
        /// </summary>
        /// <param name="id">The selected keychain primary key</param>
        /// <returns>        
        /// Successful: redirect to List page
        /// Fail: redirect to Error page
        /// </returns>
        /// POST: Keychain/Delete/{id}
        [HttpPost]
        [Authorize(Roles ="Admin,Guest")]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();
            string url = "KeychainData/DeleteKeychain/" + id;

            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
