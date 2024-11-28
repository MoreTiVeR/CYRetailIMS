using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Common.Models.UI;

[Serializable]
public class MoneyTransferFileUploadModel
{
    public string filename { get; set; }
    public string filepath { get; set; }
    public IFormFile filedata { get; set; }
}
