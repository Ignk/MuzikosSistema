using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MuzikosSistema.Models;
using PagedList;

namespace MuzikosSistema.Controllers
{
    [Authorize]
    public class MusicController : Controller
    {

        private MusicDBEntities _entities = new MusicDBEntities();
        // GET: Music
       /* public ActionResult Index()
        {
            return View(_entities.Song.ToList().OrderBy(x => x.SongName));
        }*/
        public ActionResult Index(string option, string search, int? pageNumber)
        {

            //if a user choose the radio button option as Subject  
            if (option == "Artist")
            {
                //Index action method will return a view with a student records based on what a user specify the value in textbox  
                return View(_entities.Song.Where(x => x.SongArtist.Name.Contains(search) || search == null).ToList().OrderBy(x => x.SongName).ToPagedList(pageNumber ?? 1, 15));
            }
            else if (option == "Song")
            {
                return View(_entities.Song.Where(x => x.SongName.Contains(search) || search == null).ToList().OrderBy(x => x.SongName).ToPagedList(pageNumber ?? 1, 15));
            }
            else if (option == "TimePeriod")
            {
                return View(_entities.Song.Where(x => x.TimePeriod.Contains(search) || search == null).ToList().OrderBy(x => x.SongName).ToPagedList(pageNumber ?? 1, 15));
            }
            else if (option == "Language")
            {
                return View(_entities.Song.Where(x => x.Language.Contains(search) || search == null).ToList().OrderBy(x => x.SongName).ToPagedList(pageNumber ?? 1, 15));
            }
            else 
            {
                return View(_entities.Song.Where(x => x.Style1.StyleName.Contains(search) || search == null).ToList().OrderBy(x => x.SongName).ToPagedList(pageNumber ?? 1, 15));
            }
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

            songConsist.artistSongs = _entities.Song.ToList().Where(a => a.SongArtist.Id == songConsist.song.Artist && a.Id != id).ToList();
            songConsist.existsOtherSongs = songConsist.artistSongs.Any();

            songConsist.comments = _entities.Comment.ToList().Where(a => a.Song == id).ToList();
            songConsist.existsComments = songConsist.comments.Any();

            songConsist.userCount = this.CountHistory(User.Identity.GetUserName(), id);
            songConsist.totalCount = _entities.History.Where(h => h.Song == id).Sum(h => h.Count);

            List<SongRecomandation> songRecomendations = _entities.SongRecomandation.Where(sr => sr.Recomandation == songConsist.song.Id).ToList();
            songRecomendations = songRecomendations.OrderBy(a => Guid.NewGuid()).ToList();
            songRecomendations = songRecomendations.OrderByDescending(sr => sr.Rank).Take(10).ToList();

            songConsist.simillarSongs = new List<Song>();
            foreach (var songRec in songRecomendations)
            {
                songConsist.simillarSongs.Add(_entities.Song.Where(s => s.Id == songRec.Song).FirstOrDefault());
            }

            songConsist.simillarSongs = songConsist.simillarSongs.OrderBy(a => Guid.NewGuid()).ToList();

            return View(songConsist);
        }

       

        public int CountHistory(String userName, int songId)
        {
            History history = _entities.History.Where(h => h.Song == songId && h.Username == userName).FirstOrDefault();
            
            if (history  == null)
            {
                history = new History();
                history.Song = songId;
                history.Username = userName;
                history.Count = 1;
                _entities.History.Add(history);
            }
            else
            {
                history.Count++;
                _entities.Entry(history).State = System.Data.Entity.EntityState.Modified;
            }
            _entities.SaveChanges();

            return history.Count;
        }


        // GET: Music/Create/5
        public ActionResult Create(int id = 1)
        {
            var artists = new SelectList(_entities.SongArtist.OrderBy(a => a.Name), "Id", "Name",id);
            ViewData["Artists"] = artists;

            var style = new SelectList(_entities.Style.OrderBy(a => a.StyleName), "Id", "StyleName");
            ViewData["Style"] = style;

            return View();
        }

        // POST: Music/Create
        [HttpPost]
        public ActionResult Create(Song songToCreate)
        {
            try
            {
                songToCreate.Added = DateTime.Now;

                _entities.Song.Add(songToCreate);
                _entities.SaveChanges();
                return RedirectToAction("Details", songToCreate);
            }
            catch
            {
                return View();
            }
        }

        // GET: Music/Edit/5
        public ActionResult Edit(int id)
        {
            var artists = new SelectList(_entities.SongArtist.OrderBy(a => a.Name), "Id", "Name", _entities.Song.Find(id).SongArtist);
            ViewData["ArtistsList"] = artists;

            var style = new SelectList(_entities.Style.OrderBy(a => a.StyleName), "Id", "StyleName",_entities.Song.Find(id).Style1);
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

        // GET: Artist/CreateSongLink/5
        public ActionResult CreateSongLink(int id)
        {
            var songs = new SelectList(_entities.Song.OrderBy(a => a.SongName), "Id", "SongName", id);
            ViewData["SongList"] = songs;

            var types = new SelectList(_entities.SongLinkTypes.OrderBy(a => a.TypeName), "Id", "TypeName");
            ViewData["LinkTypes"] = types;

            return View();
        }

        // POST: Artist/CreateSongLink
        [HttpPost]
        public ActionResult CreateSongLink(int id, SongLink songLink)
        {
            try
            {
                if (_entities.SongLink.ToList().Exists(a => a.Type == songLink.Type && a.Song == id))
                {
                    SongLink songLinkToUpdate = _entities.SongLink.ToList().Find(a => a.Type == songLink.Type && a.Song == id);
                    songLinkToUpdate.Link = songLink.Link;
                    _entities.Entry(songLinkToUpdate).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    _entities.SongLink.Add(songLink);
                }
                _entities.SaveChanges();
                return RedirectToAction("Details", _entities.Song.Find(songLink.Song));
            }
            catch
            {
                return View();
            }
        }

        // GET: Artist/CreateComment/5
        public ActionResult CreateComment(int id)
        {
            Comment comment = new Comment();
            comment.Song1 = _entities.Song.Find(id);
            return View(comment);
        }

        // POST: Artist/CreateComment
        [HttpPost]
        public ActionResult CreateComment(int id, Comment comment)
        {
            try
            {
                string email = User.Identity.GetUserName();
                ListenerProfile listenerProfile = _entities.ListenerProfile.Where(p => p.Email == email).FirstOrDefault();
                if (listenerProfile == null)
                    comment.Author = User.Identity.GetUserName();
                else
                    comment.Author = listenerProfile.Username;

                comment.Song = id;
                _entities.Comment.Add(comment);
                _entities.SaveChanges();

                return RedirectToAction("Details", _entities.Song.Find(id));
            }
            catch
            {
                return View();
            }
        }

    }
}
