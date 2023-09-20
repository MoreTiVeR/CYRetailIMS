using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMGeography")]
public partial class TMGeography : BaseEntity
{
    [Key]
    public int GeoID { get; set; }

    [StringLength(255)]
    public string GeoName { get; set; } = null!;
}
