using Microsoft.AspNet.Identity;
using MuzikosSistema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MuzikosSistema.Controllers
{
    [Authorize]
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
            String username = User.Identity.GetUserName();
            List<ListenerRecomendation> listenerRecomandations = _entities.ListenerRecomendation.Where(lr => lr.Username == username).OrderByDescending(lr => lr.Rank).Take(20).ToList();
            listenerRecomandations = listenerRecomandations.OrderBy(a => Guid.NewGuid()).ToList();
            listenerRecomandations = listenerRecomandations.Take(5).ToList();


            recomendation.recomendedList = new List<Song>();
            foreach (var listenerRecomandation in listenerRecomandations)
            {
                recomendation.recomendedList.Add(listenerRecomandation.Song1);
            }

            return View(recomendation);
        }


        public ActionResult CalculateSimillarSongRecomendations()
        {
            List<Song> songs = _entities.Song.ToList();

            foreach (var song in songs)
            {
                List<Song> songsToCalculate = _entities.Song.Where(s => s.Id != song.Id).ToList();
                foreach (var songToCalculate in songsToCalculate)
                {
                    SongRecomandation songRecomandation = _entities.SongRecomandation.Where(sr => sr.Song == song.Id && sr.Recomandation == songToCalculate.Id).FirstOrDefault();
                    if (songRecomandation == null && song.SongArtist != songToCalculate.SongArtist)
                    {
                        songRecomandation = new SongRecomandation();
                        songRecomandation.Song = song.Id;
                        songRecomandation.Recomandation = songToCalculate.Id;
                        songRecomandation.Rank = 0;
                        if (song.Style == songToCalculate.Style)
                            songRecomandation.Rank += 2;
                        if (song.TimePeriod == songToCalculate.TimePeriod)
                            songRecomandation.Rank += 2;
                        if (song.Language == songToCalculate.Language)
                            songRecomandation.Rank++;
                        if (song.Mood == songToCalculate.Mood)
                            songRecomandation.Rank++;
                        if (song.Pace == songToCalculate.Pace)
                            songRecomandation.Rank++;
                        if (song.SongArtist.Type == songToCalculate.SongArtist.Type)
                            songRecomandation.Rank++;
                        if (song.SongArtist.Artist.FirstOrDefault().Nationality == songToCalculate.SongArtist.Artist.FirstOrDefault().Nationality)
                            songRecomandation.Rank++;
                        _entities.SongRecomandation.Add(songRecomandation);
                    }
                }
                _entities.SaveChanges();
            }
            return RedirectToAction("RecomendationPage");
        }

        public ActionResult CalculateSimillarListenersRecomendations()
        {
            _entities.Database.ExecuteSqlCommand("TRUNCATE TABLE ListenerRecomendation");
            List<ListenerProfile> listeners = _entities.ListenerProfile.ToList();
            foreach (var listener in listeners)
            {
                List<ListenerProfile> listenersToCompare = _entities.ListenerProfile.Where(l => l.Id != listener.Id).ToList();
                foreach (var listenerToComapare in listenersToCompare)
                {
                    int similarityRank = 0;

                    if (listener.Nationality == listenerToComapare.Nationality)
                        similarityRank++;

                    TimeSpan dateDiference = listener.BirthDate.Value - listenerToComapare.BirthDate.Value;
                    if (Math.Abs(dateDiference.Days) < 365)
                        similarityRank += 5;
                    else if(Math.Abs(dateDiference.Days) < 1825)
                        similarityRank += 3;
                    else if (Math.Abs(dateDiference.Days) < 3650)
                        similarityRank ++;

                    List <ListenerStyles> listenerStyles = _entities.ListenerStyles.Where(ls => ls.Listener == listener.Id).ToList();
                    foreach (var style in listenerStyles)
                    {
                        ListenerStyles compareStyle = _entities.ListenerStyles.Where(ls => ls.Listener == listenerToComapare.Id && ls.Style == style.Style).FirstOrDefault();
                        if (compareStyle != null)
                            similarityRank++;
                    }
                    List<History> history = _entities.History.Where(h => h.Username == listenerToComapare.Email).OrderByDescending(h => h.Count).Take(10).ToList();
                    foreach(var historySong in history)
                    {
                        ListenerRecomendation recomendation = _entities.ListenerRecomendation.Where(r => r.Username == listener.Email && r.Song == historySong.Song).FirstOrDefault();
                        if (recomendation == null)
                        {
                            recomendation = new ListenerRecomendation();
                            recomendation.Song = historySong.Song;
                            recomendation.Username = listener.Email;
                            recomendation.Rank = similarityRank;
                            _entities.ListenerRecomendation.Add(recomendation);
                        }
                        else
                        {
                            recomendation.Rank += similarityRank;
                            _entities.Entry(recomendation).State = System.Data.Entity.EntityState.Modified;
                        }
                    }

                }
            }
            _entities.SaveChanges();
            return RedirectToAction("RecomendationPage");
        }

    }
}