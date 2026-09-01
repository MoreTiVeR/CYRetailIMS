using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStockByBranchID.v1;
public record InquiryCountStockByBranchIDQuery : IRequest<BaseResponse<List<InquiryCountStockByBranchIDResponseDTO>>>
{
    public int branchid { get; init; }

    /// <summary>
    /// true = คืนข้อมูลระดับรายสินค้า (ไม่รวมกลุ่มตามประเภทย่อย) เพื่อแสดง รหัสสินค้า/ชื่อสินค้า
    /// false (ค่าเริ่มต้น) = คงพฤติกรรมเดิม (รวมกลุ่มตามประเภทย่อย)
    /// </summary>
    public bool itemlevel { get; init; } = false;
}
