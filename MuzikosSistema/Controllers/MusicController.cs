using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MuzikosSistema.Models;

namespace MuzikosSistema.Controllers
{
    public class MusicController : Controller
    {

        private MusicDBEntities _entities = new MusicDBEntities();
        // GET: Music
        public ActionResult Index()
        {
            return View(_entities.Song.ToList());
        }

        // GET: Music/Details/5
        public ActionResult Details(int id)
        {
            SongConsist songConsist = new SongConsist();
            songConsist.song = _entities.Song.Find(id);
            if (_entities.SongLink.ToList().Exists(a => a.Type == 1 && a.Song == id))
            {
                songConsist.youtubeLink = _entities.SongLink.ToList().Where(a => a.Type == 1 && a.Song == id).First().Link;
            }
            if (_entities.SongLink.ToList().Exists(a => a.Type == 2 && a.Song == id))
            {
                songConsist.spotifyLink = _entities.SongLink.ToList().Where(a => a.Type == 2 && a.Song == id).First().Link;
            }
            if (_entities.SongLink.ToList().Exists(a => a.Type == 3 && a.Song == id))
            {
                songConsist.deezerLink = _entities.SongLink.ToList().Where(a => a.Type == 3 && a.Song == id).First().Link;
            }
            if (_entities.SongLink.ToList().Exists(a => a.Type == 4 && a.Song == id))
            {
                songConsist.soundCloudLink = _entities.SongLink.ToList().Where(a => a.Type == 4 && a.Song == id).First().Link;
            }
            songConsist.artists = _entities.Artist.ToList().Where(a => a.SongArtist == songConsist.song.SongArtist.Id).ToList();

            //   _entities.SongLink.Find(id).Link;

            return View(songConsist);
        }

        // GET: Music/Create
        public ActionResult Create()
        {
            var artists = new SelectList(_entities.SongArtist, "Id", "Name");
            ViewData["Artists"] = artists;

            var style = new SelectList(_entities.Style, "Id", "StyleName");
            ViewData["Style"] = style;

            return View();
        }

        // POST: Music/Create
        [HttpPost]
        public ActionResult Create(Song songToCreate)
        {
            try
            {
                _entities.Song.Add(songToCreate);
                _entities.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Music/Edit/5
        public ActionResult Edit(int id)
        {
            var artists = new SelectList(_entities.SongArtist, "Id", "Name", _entities.Song.Find(id).SongArtist);
            ViewData["ArtistsList"] = artists;

            var style = new SelectList(_entities.Style, "Id", "StyleName",_entities.Song.Find(id).Style1);
            ViewData["StyleList"] = style;

            return View(_entities.Song.Find(id));
        }

        // POST: Music/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Song songToUpdate)
        {
            try
            {
                _entities.Entry(songToUpdate).State = System.Data.Entity.EntityState.Modified;
                _entities.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Music/Delete/5
        public ActionResult Delete(int id)
        {
            return View(_entities.Song.Find(id));
        }

        // POST: Music/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Song songToDelete)
        {
            try
            {
                songToDelete = _entities.Song.Find(id);
                _entities.Song.Remove(songToDelete);
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
