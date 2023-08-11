using ArtisansBeadStudio.Models;
using ArtisansBeadStudio.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            DetailsStyle ViewModel = new DetailsStyle();

            //objective: communicate with our style data api to retrieve one style
            //curl https://localhost:44316/api/StyleData/FindStyle/{id}

            string url = "StyleData/FindStyle/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            StyleDto SelectedStyle = response.Content.ReadAsAsync<StyleDto>().Result;

            ViewModel.SelectedStyle = SelectedStyle;

            //showing all the keychains under this style
            //collaboration part
            url = "keychainData/ListKyechainsForStyle/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<KeychainDto> keychainsInStyle = response.Content.ReadAsAsync<IEnumerable<KeychainDto>>().Result;
            ViewModel.keychainsInStyle = keychainsInStyle;

            return View(ViewModel);
        }

        // GET: Style/New
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // GET: Style/New
        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Style style)
        {
            string url = "StyleData/AddStyle";

            string jsonpayload = jss.Serialize(style);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List", new { id = style.StyleID });
            }
            else
            {
                return RedirectToAction("Error");
            }


        }


        //// POST: Style/Create
        //[HttpPost]
        //[Authorize]
        //public ActionResult Create(Style style)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: Style/Edit/5
        public ActionResult Edit(int id)
        {
            DetailsStyle ViewModel = new DetailsStyle();

            //the existing style information
            string url = "StyleData/FindStyle/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            StyleDto SelectedStyle = response.Content.ReadAsAsync<StyleDto>().Result;
            ViewModel.SelectedStyle = SelectedStyle;

            return View(ViewModel);
        }

        // POST: Style/Update/5
        [HttpPost]
        public ActionResult Update(int id, Style style)
        {

            string url = "StyleData/UpdateStyle/" + id;
            string jsonpayload = jss.Serialize(style);
            HttpContent content = new StringContent(jsonpayload);
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


        // GET: Style/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "StyleData/FindStyle/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            StyleDto SelectedStyle = response.Content.ReadAsAsync<StyleDto>().Result;
            return View(SelectedStyle);
        }

        /*
        // POST: Style/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            
            string url = "StyleData/DeleteStyle/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List", new { id = Request.QueryString["StyleID"] });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        */
        [HttpPost]
        public ActionResult Delete(int id)
        {

            string url = "StyleData/DeleteStyle/" + id;
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
