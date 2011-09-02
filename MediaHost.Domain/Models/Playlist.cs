using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

using MediaHost.Domain.Repository;

namespace MediaHost.Domain.Models
{
    public class Playlist : IActiveRecord
    {
        [Key]
        public long Id { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "Exception: EntityId Required")]
        public long EntityId { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Exception: Playlist Name Required")]
        public string Name { get; set; }

        [IgnoreActiveRecord]
        public IEnumerable<MediaFile> Files { get; set; }
    }
}
