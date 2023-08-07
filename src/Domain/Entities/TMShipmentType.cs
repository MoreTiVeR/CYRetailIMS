using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMShipmentType")]
public partial class TMShipmentType
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

    [InverseProperty("ShipmentType")]
    public virtual ICollection<TTShipment> TTShipments { get; set; } = new List<TTShipment>();
}
