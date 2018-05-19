using Microsoft.AspNet.Identity;
using MuzikosSistema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MuzikosSistema.Controllers
{
    public class ListenerController : Controller
    {
        private MusicDBEntities _entities = new MusicDBEntities();

        // GET: Listener/ListenerProfile
        public ActionResult ListenerProfile()
        {
            String email = User.Identity.GetUserName();

            ListenerProfile listenerProfile = _entities.ListenerProfile.Where(p => p.Email == email).FirstOrDefault();
            if(listenerProfile==null)
            {
                listenerProfile = new ListenerProfile();
                listenerProfile.Email = email;
                _entities.ListenerProfile.Add(listenerProfile);
                _entities.SaveChanges();
            }

            ListenerConsist listenerConsist = new ListenerConsist();
            listenerConsist.listener = listenerProfile;

            listenerConsist.styles = _entities.ListenerStyles.ToList().Where(a => a.Listener == listenerProfile.Id).ToList();

            return View(listenerConsist);
        }

        // POST: Listener/ListenerProfile
        [HttpPost]
        public ActionResult ListenerProfile(ListenerConsist listenerConsist)
        {
            try
            {
                ListenerProfile listenerProfile = listenerConsist.listener;
                _entities.Entry(listenerProfile).State = System.Data.Entity.EntityState.Modified;
                _entities.SaveChanges();
                return RedirectToAction("Index", "Music");
            }
            catch
            {
                return View();
            }
        }


        // GET: Listener/AddStyle
        public ActionResult AddStyle()
        {
            String email = User.Identity.GetUserName();
            ListenerProfile listenerProfile = _entities.ListenerProfile.Where(p => p.Email == email).FirstOrDefault();
            List<Style> styles = _entities.Style.ToList();
            List<ListenerStyles> listenerStylesList = listenerProfile.ListenerStyles.ToList();
            ListenerStyles listenerStyles = new ListenerStyles();
            listenerStyles.Listener = listenerProfile.Id;

            foreach (var listnerStyle in listenerStylesList)
            {
                styles = styles.Where(l => l.Id != listnerStyle.Style).ToList();
            }

            var styleList = new SelectList(styles.OrderBy(a => a.StyleName ), "Id", "StyleName");

            ViewData["StyleList"] = styleList;

            return View(listenerStyles);
        }

        // POST: Listener/AddStyle
        [HttpPost]
        public ActionResult AddStyle(ListenerStyles listenerStyles)
        {
            try
            {
                _entities.ListenerStyles.Add(listenerStyles);
                _entities.SaveChanges();
                return RedirectToAction("ListenerProfile");
            }
            catch
            {
                return View();
            }
        }

        // GET: Listener/DeleteStyle/5
        public ActionResult DeleteStyle(int id)
        {
            return View(_entities.ListenerStyles.Find(id));
        }

        // POST: Artist/DeleteStyle/5
        [HttpPost]
        public ActionResult DeleteStyle(int id, ListenerStyles listenerStyles)
        {
            try
            {
                listenerStyles = _entities.ListenerStyles.Find(id);
                _entities.ListenerStyles.Remove(listenerStyles);
                _entities.SaveChanges();
                return RedirectToAction("ListenerProfile");
            }
            catch
            {
                return View();
            }
        }

        // GET: Listener/RecomendationPage
        public ActionResult RecomendationPage()
        {
            Recomendation recomendation = new Recomendation();
            List<SongListenCount> songListenCountList = new List<SongListenCount>();

            List<Song> songs = _entities.Song.ToList();
            foreach (var song in songs)
            {
                SongListenCount songListenCount = new SongListenCount();
                int songId = song.Id;
                History history = _entities.History.Where(h => h.Song == songId).FirstOrDefault();
                if (history != null)
                { 
                    songListenCount.totalCount =_entities.History.Where(h => h.Song == songId).Sum(h => h.Count);
                    songListenCount.song = song;
                    songListenCountList.Add(songListenCount);
                }
            }
            songListenCountList = songListenCountList.OrderByDescending(a => a.totalCount).Take(5).ToList();
            recomendation.mostPopularList = new List<Song>();
            foreach (var song in songListenCountList)
            {
                recomendation.mostPopularList.Add(song.song);
            }


            return View(recomendation);
        }

    }
}