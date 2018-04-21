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
            return View(_entities.Song.Find(id));
        }

        // GET: Music/Create
        public ActionResult Create()
        {
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
