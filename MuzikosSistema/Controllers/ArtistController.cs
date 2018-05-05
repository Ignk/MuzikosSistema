using MuzikosSistema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MuzikosSistema.Controllers
{
    public class ArtistController : Controller
    {
        private MusicDBEntities _entities = new MusicDBEntities();
        // GET: Artist
        public ActionResult Index()
        {
            return View(_entities.SongArtist.ToList());
        }

        // GET: Artist/Details/5
        public ActionResult Details(int id)
        {
            ArtistConsist artistConsist = new ArtistConsist();
            artistConsist.songArtist    = _entities.SongArtist.Find(id);
            artistConsist.artists = _entities.Artist.ToList().Where(a => a.SongArtist == id).ToList();
            return View(artistConsist);
        }

        // GET: Artist/Create
        public ActionResult Create()
        {
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
            var style = new SelectList(_entities.Style, "Id", "StyleName");
            ViewData["Style"] = style;

            var songArtist = new SelectList(_entities.SongArtist, "Id", "Name", id);
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
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Artist/EditArtist/5
        public ActionResult EditArtist(int id)
        {
            var style = new SelectList(_entities.Style, "Id", "StyleName");
            ViewData["StyleList"] = style;

            var songArtist = new SelectList(_entities.SongArtist, "Id", "Name", id);
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
                return RedirectToAction("Index");
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
