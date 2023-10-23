using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.SupplierTypeService.Queries.GetSupplierTypeList.v1;

[Serializable]
public class GetSupplierTypeResponseDTO
{
    public int suppliertypeid { get; set; }

    public string suppliertypename { get; set; }

    public string description { get; set; }

    public string createdby { get; set; }

    public DateTime creadeddate { get; set; }

    public bool isactive { get; set; }
}
