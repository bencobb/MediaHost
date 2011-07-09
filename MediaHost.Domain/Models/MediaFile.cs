using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MediaHost.Domain.Models
{
    public class MediaFile : IActiveRecord
    {
        [Key]
        public long Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Exception: EntityId Required")]
        public int EntityId { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Exception: File Name Required")]
        public string Name { get; set; }

        public string RelativeFilePath { get; set; }

    }
}
