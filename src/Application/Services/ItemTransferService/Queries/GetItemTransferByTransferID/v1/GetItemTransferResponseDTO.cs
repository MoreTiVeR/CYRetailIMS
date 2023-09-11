using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByTransferID.v1;

[Serializable]
public class GetItemTransferResponseDTO
{
    public int transferid { get; set; }
    public int transfertypeid { get; set; }
    public string transfertypename { get; set; }
    public int sourceid { get; set; }
    public int sourcename { get; set; }
    public int destinationid { get; set; }
    public int destinationname { get; set; }
    public string description { get; set; }
    public string createdby { get; set; }
    public DateTime creadeddate { get; set; }
}
