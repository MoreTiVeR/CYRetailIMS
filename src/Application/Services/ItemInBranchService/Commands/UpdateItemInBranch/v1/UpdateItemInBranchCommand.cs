using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemInBranchService.Commands.UpdateItemInBranch.v1;
public record UpdateItemInBranchCommand : IRequest<BaseResponse<CommandResponse>>
{
    public int itemid { get; init; }
    public int branchid { get; init; }
    //public string itemname { get; init; }
    //public int itembrandid { get; init; }
    //public int itemtypeid { get; init; }
    //public int unitofmeasureid { get; init; }
    public int qty { get; init; }
    //public string description { get; init; }
    public int notifyminqty { get; set; }
    public int? notifymaxqty { get; set; }
    public decimal price { get; init; }
    //public decimal cost { get; init; }
    //public bool isactive { get; init; }
    public string updatedby { get; init; }
    public DateTime updateddate { get; init; }
}
