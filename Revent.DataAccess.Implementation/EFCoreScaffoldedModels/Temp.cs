using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Revent.EFCore.DataModel.Models;

[Keyless]
public partial class Temp
{
    [Column("column1", TypeName = "character varying")]
    public string? Column1 { get; set; }
}
