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

    [StringLength(50)]
    [Unicode(false)]
    public string WarehouseName { get; set; } = null!;

    public int BranchID { get; set; }

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

    [ForeignKey("BranchID")]
    [InverseProperty("TMWarehouses")]
    public virtual TMBranch Branch { get; set; } = null!;

    [InverseProperty("Warehouse")]
    public virtual ICollection<TMShipment> TMShipments { get; set; } = new List<TMShipment>();

    [InverseProperty("Warehouse")]
    public virtual ICollection<TTPurchaseOrder> TTPurchaseOrders { get; set; } = new List<TTPurchaseOrder>();
}
