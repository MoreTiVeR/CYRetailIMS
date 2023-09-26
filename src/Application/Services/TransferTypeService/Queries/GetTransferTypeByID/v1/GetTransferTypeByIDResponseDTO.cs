using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.TransferTypeService.Queries.GetTransferTypeByID.v1;
[Serializable]
public class GetTransferTypeByIDResponseDTO
{
    public int transfertypeid { get; set; }

    public string transfertypename { get; set; }

    public string description { get; set; }

    public string createdby { get; set; }

    public DateTime creadeddate { get; set; }

    public bool isactive { get; set; }
}
