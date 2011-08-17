using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

using MediaHost.Domain.Repository;

namespace MediaHost.Domain.Models
{
    public class Playlist_MediaFile : IActiveRecord
    {
        [Key]
        public long Id { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "Exception: PlaylistId Required")]
        public long PlaylistId{ get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "Exception: MediaFileId Required")]
        public long MediaFileId { get; set; }
    }
}
