using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStockByBranchID.v1;

namespace CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStockByID.v1;
public class InquiryCountStockByIDDetail : InquiryCountStockByBranchIDResponseDTO
{
    public int countstockdetailid { get; set; }
}
