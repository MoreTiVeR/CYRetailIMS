using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TTShipment")]
public partial class TTShipment : BaseAuditableEntity
{
	[Key]
	public int ShipmentID { get; set; }

	public int ShipmentTypeID { get; set; }

	public int? WarehouseID { get; set; }

	public int PurchaseOrderID { get; set; }

	[StringLength(50)]
	[Unicode(false)]
	public string? ShipmentName { get; set; }

	[Column(TypeName = "datetime")]
	public DateTime? ShipmentDate { get; set; }

	[StringLength(50)]
	[Unicode(false)]
	public string? TrackingNo { get; set; }

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
