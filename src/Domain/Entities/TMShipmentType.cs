using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMShipmentType")]
public partial class TMShipmentType : BaseAuditableEntity
{
    [Key]
    public int ShipmentTypeID { get; set; }

    /// <summary>
    /// ประเภทการขนส่ง ขนส่งทางบก ขนส่งทางน้ำ ขนส่งทางอากาศ ขนส่งระบบคอนเทนเนอร์ ขนส่งพัสดุแบบด่วน(Delivery Express)
    /// </summary>
    [StringLength(50)]
    [Unicode(false)]
    public string? ShipmentTypeName { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Description { get; set; }

    [InverseProperty("ShipmentType")]
    public virtual ICollection<TTShipment> TTShipments { get; set; } = new List<TTShipment>();
}
