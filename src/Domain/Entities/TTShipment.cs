using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TTShipment")]
public partial class TTShipment
{
    [Key]
    public int ShipmentID { get; set; }

    public int? ShipmentTypeID { get; set; }

    public int? WarehouseID { get; set; }

    public int PurchaseOrderID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? ShipmentName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ShipmentDate { get; set; }

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

    [ForeignKey("PurchaseOrderID")]
    [InverseProperty("TTShipments")]
    public virtual TTPurchaseOrder PurchaseOrder { get; set; } = null!;

    [ForeignKey("ShipmentTypeID")]
    [InverseProperty("TTShipments")]
    public virtual TMShipmentType? ShipmentType { get; set; }

    [ForeignKey("WarehouseID")]
    [InverseProperty("TTShipments")]
    public virtual TMWarehouse? Warehouse { get; set; }
}
