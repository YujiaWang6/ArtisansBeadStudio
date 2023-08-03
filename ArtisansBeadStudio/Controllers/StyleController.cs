using ArtisansBeadStudio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ArtisansBeadStudio.Controllers
{
    public class StyleController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static StyleController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44383/api/");
        }
        // GET: Style/List
        public ActionResult List()
        {
            string url = "StyleData/ListStyles";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<StyleDto> styles = response.Content.ReadAsAsync<IEnumerable<StyleDto>>().Result;
            return View(styles);
        }
        // GET: Style/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Style/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Style/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Style/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Style/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Style/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Style/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
