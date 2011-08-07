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
        public string Name { get; set; }

        public string RelativeFilePath { get; set; }

        public int ContentLength { get; set; }

        public string ContentType { get; set; }

        public string FileName { get; set; }

        public bool IsStreaming { get; set; }

        public MediaFile() 
        { 
        
        }
    }
}
