using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MediaHost.Domain.Models
{
    public class Group : IActiveRecord
    {
        [Key]
        public long Id { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "Exception: EntityId Required")]
        public long EntityId { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Exception: Group Name Required")]
        public string Name { get; set; }

    }
}
