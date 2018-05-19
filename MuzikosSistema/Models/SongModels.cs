using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MuzikosSistema.Models
{
    public class ArtistConsist
    {
        public SongArtist songArtist { get; set; }
        public List<Artist> artists { get; set; }
        public List<Song> songs { get; set; }

    }

    public class SongConsist
    {
        public Song song { get; set; }
        public List<Artist> artists { get; set; }
        public string youtubeLink { get; set; }
        public string spotifyLink { get; set; }
        public string deezerLink { get; set; }
        public string soundCloudLink { get; set; }
        public List<Song> artistSongs { get; set; }
        public Boolean existsOtherSongs { get; set; }
        public int totalCount { get; set; }
        public int userCount { get; set; }
        public Boolean existsComments { get; set; }
        public List<Comment> comments { get; set; }
    }
    public class ListenerConsist
    {
        public ListenerProfile listener { get; set; }
        public List<ListenerStyles> styles { get; set; }
    }

    public class Recomendation
    {
        public List<Song> mostPopularList { get; set; }
        public List<Song> recomendedList { get; set; }
    }

    public class SongListenCount
    {
        public Song song { get; set; }
        public int totalCount { get; set; }
        
    }
}