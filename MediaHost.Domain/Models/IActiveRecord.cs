using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MediaHost.Domain.Models
{
    public interface IActiveRecord
    {
        long Id { get; set; }
    }
}
