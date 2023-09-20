using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[PrimaryKey("SubDistrictID", "SubDistrictCode", "GeoID", "ProvinceID")]
[Table("TMDistrict")]
public partial class TMDistrict : BaseEntity
{
    [Key]
    public int SubDistrictID { get; set; }

    [Key]
    [StringLength(4)]
    public string SubDistrictCode { get; set; } = null!;

    [StringLength(150)]
    public string SubDistrictNameTH { get; set; } = null!;

    [StringLength(150)]
    public string SubDistrictNameEN { get; set; } = null!;

    [Key]
    public int GeoID { get; set; }

    [Key]
    public int ProvinceID { get; set; }
}
