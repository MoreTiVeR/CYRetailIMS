using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CYRetailIMS.Application.Common.Models.UI;

[JsonObject]
[Serializable]
public class SearchItemViewModel
{
    public int branchid { get; set; }

    /// <summary>
    /// true = คืนข้อมูลระดับรายสินค้า (รหัสสินค้า/ชื่อสินค้า) สำหรับหน้านับสต๊อกแบบใหม่
    /// false (ค่าเริ่มต้น) = คืนข้อมูลแบบรวมตามประเภทย่อย (พฤติกรรมเดิม)
    /// </summary>
    public bool itemlevel { get; set; } = false;
}