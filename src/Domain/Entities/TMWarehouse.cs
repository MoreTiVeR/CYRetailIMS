using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMWarehouse")]
public partial class TMWarehouse
{
    [Key]
    public int WarehouseID { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string WarehouseName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Description { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string CreatedBy { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime CreadedDate { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? UpdatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedDate { get; set; }

    [Required]
    public bool? Status { get; set; }

    [InverseProperty("Warehouse")]
    public virtual ICollection<TMStock> TMStocks { get; set; } = new List<TMStock>();

    [InverseProperty("Warehouse")]
    public virtual ICollection<TTShipment> TTShipments { get; set; } = new List<TTShipment>();
}
