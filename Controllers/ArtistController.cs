using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JMTI_Test.Models;
using System.IO;
using JMTI_Test.DTO;
using PagedList;

namespace JMTI_Test.Controllers
{
    public class ArtistController : Controller
    {
        ArtistRepository repository = new ArtistRepository();

        public ActionResult Index(int? page, string searchString)
        {
            int pageNumber = page ?? 1;

            var artists = repository.GetAll();

            if(!String.IsNullOrEmpty(searchString))
            {
                artists = repository.GetAll().Where(a => a.ArtistName.ToLower().Contains(searchString.ToLower())).ToList();
            }

            return View(artists.ToPagedList(pageNumber, 3));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ArtistDTO artist)
        {
            Artist artistToSave = new Artist();
            artistToSave.ArtistName = artist.ArtistName;
            artistToSave.PackageName = artist.PackageName;
            artistToSave.ReleaseDate = artist.ReleaseDate;
            artistToSave.Price = artist.Price;

            if (artist.Image==null)
            {
                ModelState.AddModelError("Image", "Image cannot be empty");
                return View(artist);
            }

            string imageExtension = Path.GetExtension(artist.Image.FileName).ToLower();
            if (!imageExtension.Equals(".jpg") && !imageExtension.Equals(".jpeg") && !imageExtension.Equals(".png"))
            {
                ModelState.AddModelError("Image", "Image format only jpg,jpeg and png");
                return View(artist);
            }


            if (artist.Sample == null)
            {
                ModelState.AddModelError("Sample", "Sample cannot be empty");
                return View(artist);
            }

            string sampleExtension = Path.GetExtension(artist.Sample.FileName).ToLower();
            if (!sampleExtension.Equals(".mp3"))
            {
                ModelState.AddModelError("Sample", "Music Sample format only mp3");
                return View(artist);
            }

            artistToSave.ImageUrl = Path.GetFileName(artist.Image.FileName);
            artistToSave.SampleUrl = Path.GetFileName(artist.Sample.FileName);

            var fileImageName = Path.GetFileName(artist.Image.FileName);
            var pathImage = Path.Combine(Server.MapPath("~/Content/Upload/Image"),fileImageName);
            artist.Image.SaveAs(pathImage);

            var fileSampleName = Path.GetFileName(artist.Sample.FileName);
            var pathSample = Path.Combine(Server.MapPath("~/Content/Upload/Sample"),fileSampleName);
            artist.Sample.SaveAs(pathSample);

            repository.Save(artistToSave);

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id=0)
        {
            if (id == 0)
            {
                TempData["MessageInfo"] = "Null Id Is Not Allowed";
                return RedirectToAction("Index");
            }

            var artist = repository.GetById(id);

            if(artist==null)
            {
                TempData["MessageInfo"] = "No Data Found";
                return RedirectToAction("Index");
            }

            ArtistDTO artistDTO = new ArtistDTO();
            artistDTO.ArtistId = artist.ArtistId;
            artistDTO.ArtistName = artist.ArtistName;
            artistDTO.PackageName = artist.PackageName;
            artistDTO.Price = artist.Price;
            artistDTO.ReleaseDate = artist.ReleaseDate;

            return View(artistDTO);
        }

        [HttpPost]
        public ActionResult Edit(ArtistDTO artist)
        {
            var theArtist = repository.GetById(artist.ArtistId);

            theArtist.ArtistName = artist.ArtistName;
            theArtist.PackageName = artist.PackageName;
            theArtist.ReleaseDate = artist.ReleaseDate;
            theArtist.Price = artist.Price;
            theArtist.ImageUrl = theArtist.ImageUrl;
            theArtist.SampleUrl = theArtist.SampleUrl;

            if (artist.Image != null)
            {
                string imageExtension = Path.GetExtension(artist.Image.FileName).ToLower();
                if (!imageExtension.Equals(".jpg") && !imageExtension.Equals(".jpeg") && !imageExtension.Equals(".png"))
                {
                    ModelState.AddModelError("Image", "Image format only jpg,jpeg and png");
                    return View(artist);
                }

                theArtist.ImageUrl = Path.GetFileName(artist.Image.FileName);

                var fileImageName = Path.GetFileName(artist.Image.FileName);
                var pathImage = Path.Combine(Server.MapPath("~/Content/Upload/Image"), fileImageName);
                artist.Image.SaveAs(pathImage);
            }


            if (artist.Sample != null)
            {
                string sampleExtension = Path.GetExtension(artist.Sample.FileName).ToLower();
                if (!sampleExtension.Equals(".mp3"))
                {
                    ModelState.AddModelError("Sample", "Music Sample format only mp3");
                    return View(artist);
                }

                theArtist.SampleUrl = Path.GetFileName(artist.Sample.FileName);

                var fileSampleName = Path.GetFileName(artist.Sample.FileName);
                var pathSample = Path.Combine(Server.MapPath("~/Content/Upload/Sample"), fileSampleName);
                artist.Sample.SaveAs(pathSample);
            }


            repository.Update(theArtist);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id=0)
        {
            if (id == 0)
            {
                TempData["MessageInfo"] = "Null Id Is Not Allowed";
                return RedirectToAction("Index");
            }

            var artist = repository.GetById(id);

            if (artist == null)
            {
                TempData["MessageInfo"] = "No Data Found";
                return RedirectToAction("Index");
            }

            repository.Delete(artist);

            return RedirectToAction("Index");
        }

    }
}
