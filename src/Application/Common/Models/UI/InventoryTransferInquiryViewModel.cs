using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CYRetailIMS.Application.Common.Models.UI;

[Serializable]
[JsonObject]
public class InventoryTransferInquiryViewModel
{
    public int draftid { get; set; }
}
