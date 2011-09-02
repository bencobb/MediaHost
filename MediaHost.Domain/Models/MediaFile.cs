using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

using MediaHost.Domain.Storage;
using MediaHost.Domain.Repository;

namespace MediaHost.Domain.Models
{
    public class MediaFile : IActiveRecord
    {
        [Key]
        public long Id { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "Exception: EntityId Required")]
        public long EntityId { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Exception: File Name Required")]
        [StringLength(100, ErrorMessage="Name cannot be longer than 100 characters")]
        public string Name { get; set; }

        [StringLength(100, ErrorMessage = "RelativeFilePath cannot be longer than 500 characters")]
        public string RelativeFilePath { get; set; }

        public int ContentLength { get; set; }

        [StringLength(100, ErrorMessage = "ContentType cannot be longer than 100 characters")]
        public string ContentType { get; set; }

        [StringLength(100, ErrorMessage = "FileName cannot be longer than 100 characters")]
        public string FileName { get; set; }

        public bool IsStreaming { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot be longer than 1000 characters")]
        public string Description { get; set; }

        [IgnoreActiveRecord]
        public long PlaylistId { get; set; }

        [IgnoreActiveRecord]
        public string TemporaryUrl { get; set; }

        public MediaFile() 
        { 
        
        }
    }
}
