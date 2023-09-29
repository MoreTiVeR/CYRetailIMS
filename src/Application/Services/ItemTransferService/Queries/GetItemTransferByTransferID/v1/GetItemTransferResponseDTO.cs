using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByTransferID.v1;

[Serializable]
public class GetItemTransferResponseDTO
{
    public int transferid { get; set; }
    public int transfertypeid { get; set; }
    public string transfertypename { get; set; }
    public int sourceid { get; set; }
    public string sourcename { get; set; }
    public int destinationid { get; set; }
    public string destinationname { get; set; }
    public string description { get; set; }
    public string createdby { get; set; }
    public DateTime creadeddate { get; set; }
    public string? updatedby { get; set; }
    public DateTime? updateddate { get; set; }

    public int itemid { get; set; }
    public string itemname { get; set; }
    public int qty { get; set; }
    public int? receiveqty { get; set; }
    public int? returnqty { get; set; }

    public int transferstatusid { get; set; }
    public string transferstatusname_th { get; set; }
    public string transferstatusname_en { get; set; }

    
}
