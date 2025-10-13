using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleReport.v1;
using CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByBranchID.v1;

namespace CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByBranchID.v2;

[Serializable]
public class GetTransactionByBranchIDV2ReseponseDTO
{
    public int totalrow { get; set; }

    /// <summary>
    /// เงินรวมทั้งหมด
    /// </summary>
    public decimal totalamount { get; set; }

    /// <summary>
    /// เงินสดทั้งหมด
    /// </summary>
    public decimal totalcash { get; set; }

    /// <summary>
    /// เงินโอนทั้งหมด
    /// </summary>
    public decimal totaltransfer { get; set; }

    /// <summary>
    /// ค่าธรรมเนียมงานเงินทั้งหมด
    /// </summary>
    public decimal totaldepositfee { get; set; }

    public List<GetTransactionByBranchIDResponseDTO> transactiondata { get; set; }
}
