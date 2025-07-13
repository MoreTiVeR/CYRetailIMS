using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;
public class SearchItemStockReportViewModel : BasePagination
{
    public int branchid { get; set; }
    public string startdate { get; set; }
    public string enddate { get; set; }
    public List<TableOrder> order { get; set; }
    public List<TableColumn> columns { get; set; }
    public bool isexportalldata { get; set; }
}

public class TableOrder
{
    public int column { get; set; }
    public string dir { get; set; }
}

public class TableColumn
{
    public JsonElement data { get; set; }
    public string name { get; set; }
    public bool searchable { get; set; }
    public bool orderable { get; set; }
    public TableColumnSearch search { get; set; }

    public string GetColumnName()
    {
        if (data.ValueKind == JsonValueKind.String)
            return data.GetString();
        else if (data.ValueKind == JsonValueKind.Object)
        {
            // Expect only 1 property like { "qty": "qty" }
            foreach (var prop in data.EnumerateObject())
                return prop.Value.GetString(); // or prop.Name
        }
        else if (data.ValueKind == JsonValueKind.Number)
        {
            return data.GetRawText();
        }

        return null;
    }
}

public class TableColumnSearch
{
    public string value { get; set; }
    public bool regex { get; set; }
}

