using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;


[Index("TransactionID", Name = "IX_TTTransactionDeletionLogs_TransactionID")]
public partial class TTTransactionDeletionLog : BaseEntity
{
    [Key]
    public int DelTransactionLogID { get; set; }

    public int TransactionID { get; set; }

    public int BranchID { get; set; }

    [Required]
    [StringLength(500)]
    [Unicode(false)]
    public string Reason { get; set; }

    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? UpdatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedDate { get; set; }

    [ForeignKey("TransactionID")]
    [InverseProperty("TTTransactionDeletionLogs")]
    public virtual TTTransaction Transaction { get; set; }
}