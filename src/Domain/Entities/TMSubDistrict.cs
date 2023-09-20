using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[PrimaryKey("DistrictID", "DistrictCode", "SubDistrictID", "ProvinceID", "GeoID")]
[Table("TMSubDistrict")]
public partial class TMSubDistrict : BaseEntity
{
    [Key]
    public int DistrictID { get; set; }

    [Key]
    [StringLength(6)]
    public string DistrictCode { get; set; } = null!;

    public int? ZipCode { get; set; }

    [StringLength(150)]
    public string? DistrictNameTH { get; set; }

    [StringLength(150)]
    public string DistrictNameEN { get; set; } = null!;

    [Key]
    public int SubDistrictID { get; set; }

    [Key]
    public int ProvinceID { get; set; }

    [Key]
    public int GeoID { get; set; }
}
