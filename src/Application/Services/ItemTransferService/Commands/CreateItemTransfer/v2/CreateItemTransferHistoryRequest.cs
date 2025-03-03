using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransfer.v2;

public record CreateItemTransferHistoryRequest
{
    public int branchid { get; set; }

    public int itemid { get; set; }
    public string itemcode { get; set; }
    public string itemname { get; set; }

    public int brandid { get; set; }

    /// <summary>
    /// จำนวนในคลังใหญ่ ณ วันทำรายการ
    /// </summary>
    public int qtyinstock { get; set; }

    /// <summary>
    /// จำนวนในสาขาที่เหลือ ณ วันทำรายการ
    /// </summary>
    public int qtyinbranch { get; set; }

    /// <summary>
    /// จำนวนขั้นต่ำ
    /// </summary>
    public int notifyminqty { get; set; }

    /// <summary>
    /// จำนวนที่ต้องเติม คำนวนจากระบบ
    /// </summary>
    public int orderqty { get; set; }

    /// <summary>
    /// จำนวนที่เติม กรอกแก้ไขเอง
    /// </summary>
    public int refillqty { get; set; }

}
