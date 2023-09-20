using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[PrimaryKey("ProvinceID", "ProvinceCode", "GeoID")]
[Table("TMProvince")]
public partial class TMProvince : BaseEntity
{
    [Key]
    public int ProvinceID { get; set; }

    [Key]
    [StringLength(2)]
    public string ProvinceCode { get; set; } = null!;

    [StringLength(150)]
    public string ProvinceNameTH { get; set; } = null!;

    [StringLength(150)]
    public string ProvinceNameEN { get; set; } = null!;

    [Key]
    public int GeoID { get; set; }
}
