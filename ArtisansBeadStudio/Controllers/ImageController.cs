using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ArtisansBeadStudio.Models;
using ArtisansBeadStudio.Models.ViewModels;
using Microsoft.Ajax.Utilities;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Web.Script.Serialization;

namespace ArtisansBeadStudio.Controllers
{
    public class ImageController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ImageController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44383/api/");
        }
        // GET: Image/List
        public ActionResult List(int id)
        {
            string url = "ImageData/ListImages/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<ImageDto> images = response.Content.ReadAsAsync<IEnumerable<ImageDto>>().Result;
            return View(images);
        }

        // GET: Image/Details/2
        public ActionResult Details(int id)
        {
            DetailsImages ViewModel = new DetailsImages();

            //objective: communicate with our image data api to retrieve one image
            //curl https://localhost:44316/api/ImageData/FindImage/{id}

            string url = "ImageData/FindImage/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            ImageDto SelectedImage = response.Content.ReadAsAsync<ImageDto>().Result;

            ViewModel.SelectedImage = SelectedImage;

            return View(ViewModel);
        }
        public ActionResult Error()
        {

            return View();
        }

        // GET: Image/New
        public ActionResult New()
        {
            string url = "StyleData/ListStyles";
            HttpResponseMessage response = client.GetAsync(url).Result;
            NewImageDto NewImageDto = new NewImageDto();
            response.Content.ReadAsAsync<IEnumerable<Style>>().Result.ForEach(style => NewImageDto.StyleList.Add(style.StyleID, style.StyleName));
            return View(NewImageDto);
        }

        // POST: Image/Create
        [HttpPost]
        public ActionResult Create([Bind(Exclude = "ImageData")] Image image)
        {
            Debug.WriteLine("the json payload is :");
            //objective: add a new image into our system using the API
            //curl -H "Content-Type:application/json" -d @image.json https://localhost:44316/api/imagedata/addimage 
            string url = "imagedata/addimage";

            byte[] imageData = null;
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase objFiles = Request.Files["ImageData"];

                using (var binaryReader = new BinaryReader(objFiles.InputStream))
                {
                    image.ImageData = binaryReader.ReadBytes(objFiles.ContentLength);
                }
            }

            string jsonpayload = jss.Serialize(image);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List", new { id = image.AlbumID });
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: Image/Edit/5
        public ActionResult Edit(int id)
        {
            EditImageDto ViewModel = new EditImageDto();

            //the existing image information
            string url = "ImageData/FindImage/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ImageDto SelectedImage = response.Content.ReadAsAsync<ImageDto>().Result;
            ViewModel.SelectedImage = SelectedImage;


            // all styles to choose from when updating this image
            url = "StyleData/ListStyles/";
            response = client.GetAsync(url).Result;
            response.Content.ReadAsAsync<IEnumerable<Style>>().Result.ForEach(style => ViewModel.StyleList.Add(style.StyleID, style.StyleName));

            return View(ViewModel);
        }

        // POST: Image/Update/5
        [HttpPost]
        public ActionResult Update(int id, Image image)
        {

            string url = "ImageData/UpdateImage/" + id;
            string jsonpayload = jss.Serialize(image);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", new { id = image.ImageID });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Image/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "ImageData/FindImage/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ImageDto SelectedImage = response.Content.ReadAsAsync<ImageDto>().Result;
            return View(SelectedImage);
        }

        // POST: Image/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "ImageData/DeleteImage/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List", new { id = Request.QueryString["AlbumID"] });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
