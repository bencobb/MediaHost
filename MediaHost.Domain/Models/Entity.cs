using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MediaHost.Domain.Models
{
    public class Entity : IActiveRecord
    {
        [Key]
        public long Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Exception: Entity Name is Required")]
        public string Name { get; set; }

        
    }
}
