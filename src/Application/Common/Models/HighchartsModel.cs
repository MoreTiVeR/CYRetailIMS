using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models;

[Serializable]
public class HighchartsModel
{
    public string title_text { get; set; }
    public List<HighchartsDataModel> data { get; set; }
}

[Serializable]
public class HighchartsDataModel
{
    public string xvalue { get; set; }
    public double yvalue { get; set; }
}

[Serializable]
public class HighchartsPieModel
{
    public string title_text { get; set; }
    public List<HighchartsPieDataModel> data { get; set; }
}


[Serializable]
public class HighchartsPieDataModel
{
    public string name { get; set; }
    public double y { get; set; }
    public bool sliced { get; set; }
    public bool selected { get; set; }
}