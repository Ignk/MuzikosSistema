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
    }

    public class SongConsist
    {
        public Song song { get; set; }
        public List<Artist> artists { get; set; }
        public string youtubeLink { get; set; }
        public string spotifyLink { get; set; }
        public string deezerLink { get; set; }
        public string soundCloudLink { get; set; }

    }
}