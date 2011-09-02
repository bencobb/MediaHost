using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaHost.Domain.Models
{
    public class Root
    {
        public IEnumerable<Entity> Entities { get; set; }
        public IEnumerable<Group> Groups { get; set; }
        public IEnumerable<MediaFile> MediaFiles { get; set; }
        public IEnumerable<Playlist> Playlists { get; set; }
        public IEnumerable<Playlist_MediaFile> Playlists_MediaFiles { get; set; }

    }
}
