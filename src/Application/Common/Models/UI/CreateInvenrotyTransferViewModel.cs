using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CYRetailIMS.Application.Common.Models.UI;

[Serializable]
[JsonObject]
public class CreateInvenrotyTransferViewModel
{
    public List<DetailInvenrotyTransferViewModel> detail { get; set; }
}

public class DetailInvenrotyTransferViewModel
{
    public int branchid { get; set; }
    public int itemid { get; set; }
    public string itemcode { get; set; }
    public string itemname { get; set; }
    public int brandid { get; set; }
    public int qtyinstock { get; set; }
    public int qtyinbranch { get; set; }
    public int notifyminqty { get; set; }

    /// <summary>
    /// จำนวนที่ต้องเติม คำนวนจากระบบ
    /// </summary>
    public int orderqty { get; set; }

    /// <summary>
    /// จำนวนที่เติม กรอกแก้ไขเอง
    /// </summary>
    public int refillqty { get; set; }
    public bool ischeck { get; set; }
}
