using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Services.ItemService.Queries.GetItemByID.v1;

namespace CYRetailIMS.Application.Services.ReportService.Queries.ItemStockReport.v1;
public class ItemStockReportResponseDTO
{
    public int totalrow { get; set; }
    public List<ItemStockReportDetailDTO> data { get; set; }
}
