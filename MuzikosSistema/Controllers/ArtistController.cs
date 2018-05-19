using MuzikosSistema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;


namespace MuzikosSistema.Controllers
{
    [Authorize]
    public class ArtistController : Controller
    {
        private MusicDBEntities _entities = new MusicDBEntities();
        // GET: Artist
        public ActionResult Index(string option, string search, int? pageNumber)
        {
            if (option == "Artist")
            {
                //Index action method will return a view with a student records based on what a user specify the value in textbox  
                return View(_entities.SongArtist.Where(x => x.Name.Contains(search) || search == null).ToList().OrderBy(x => x.Name).ToPagedList(pageNumber ?? 1, 15));
            }
            else
            {
                return View(_entities.SongArtist.Where(x => x.SongArtistType.TypeName.Contains(search) || search == null).ToList().OrderBy(x => x.Name).ToPagedList(pageNumber ?? 1, 15));
            }
        }

        // GET: Artist/Details/5
        public ActionResult Details(int id)
        {
            ArtistConsist artistConsist = new ArtistConsist();
            artistConsist.songArtist    = _entities.SongArtist.Find(id);
            artistConsist.artists = _entities.Artist.ToList().Where(a => a.SongArtist == id).ToList();
            artistConsist.songs = _entities.Song.ToList().Where(a => a.SongArtist.Id == id).ToList();
            return View(artistConsist);
        }

        // GET: Artist/Create
        public ActionResult Create()
        {
            var type = new SelectList(_entities.SongArtistType.OrderBy(a => a.TypeName), "Id", "TypeName");
            ViewData["ArtistType"] = type;
            return View();
        }

        // POST: Artist/Create
        [HttpPost]
        public ActionResult Create(SongArtist songArtistToCreate)
        {
            try
            {
                _entities.SongArtist.Add(songArtistToCreate);
                _entities.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Artist/Edit/5
        public ActionResult Edit(int id)
        {
            var type = new SelectList(_entities.SongArtistType.OrderBy(a => a.TypeName), "Id", "TypeName");
            ViewData["ArtistType"] = type;

            return View(_entities.SongArtist.Find(id));
        }

        // POST: Artist/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, SongArtist songArtistToUpdate)
        {
            try
            {
                _entities.Entry(songArtistToUpdate).State = System.Data.Entity.EntityState.Modified;
                _entities.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Artist/Delete/5
        public ActionResult Delete(int id)
        {
            return View(_entities.SongArtist.Find(id));
        }

        // POST: Artist/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, SongArtist songArtistToDelete)
        {
            try
            {
                songArtistToDelete = _entities.SongArtist.Find(id);
                _entities.SongArtist.Remove(songArtistToDelete);
                _entities.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Artist/CreateArtist/5
        public ActionResult CreateArtist(int id)
        {
            var style = new SelectList(_entities.Style.OrderBy(a => a.StyleName), "Id", "StyleName");
            ViewData["Style"] = style;

            var songArtist = new SelectList(_entities.SongArtist.OrderBy(a => a.Name), "Id", "Name", id);
            ViewData["SongArtistList"] = songArtist;

            return View();
        }

        // POST: Artist/CreateArtist
        [HttpPost]
        public ActionResult CreateArtist(int id, Artist artistToCreate)
        {
            try
            {
                _entities.Artist.Add(artistToCreate);
                _entities.SaveChanges();
                return RedirectToAction("Details", "Artist", _entities.SongArtist.Find(artistToCreate.SongArtist));
            }
            catch
            {
                return View();
            }
        }

        // GET: Artist/EditArtist/5
        public ActionResult EditArtist(int id)
        {
            var style = new SelectList(_entities.Style.OrderBy(a => a.StyleName), "Id", "StyleName");
            ViewData["StyleList"] = style;

            var songArtist = new SelectList(_entities.SongArtist.OrderBy(a => a.Name), "Id", "Name", id);
            ViewData["SongArtistList"] = songArtist;

            return View(_entities.Artist.Find(id));
        }

        // POST: Artist/EditArtist/5
        [HttpPost]
        public ActionResult EditArtist(int id, Artist artistToEdit)
        {
            try
            {
                _entities.Entry(artistToEdit).State = System.Data.Entity.EntityState.Modified;
                _entities.SaveChanges();
               return RedirectToAction("Details", "Artist", _entities.SongArtist.Find(artistToEdit.SongArtist));
                
            }
            catch
            {
                return View();
            }
        }


        // GET: Artist/Delete/5
        public ActionResult DeleteArtist(int id)
        {
            return View(_entities.Artist.Find(id));
        }

        // POST: Artist/Delete/5
        [HttpPost]
        public ActionResult DeleteArtist(int id, Artist artistToDelete)
        {
            try
            {
                artistToDelete = _entities.Artist.Find(id);
                _entities.Artist.Remove(artistToDelete);
                _entities.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
