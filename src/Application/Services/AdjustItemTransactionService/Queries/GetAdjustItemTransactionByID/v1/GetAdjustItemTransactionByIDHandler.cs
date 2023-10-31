using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.AdjustItemTransactionService.Queries.GetAdjustItemTransactions.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.AdjustItemTransactionService.Queries.GetAdjustItemTransactionByID.v1;
public class GetAdjustItemTransactionByIDHandler : BaseService, IRequestHandler<GetAdjustItemTransactionByIDQuery, BaseResponse<GetAdjustItemTransactionByIDResponseDTO>>
{
    public GetAdjustItemTransactionByIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GetAdjustItemTransactionByIDResponseDTO>> Handle(GetAdjustItemTransactionByIDQuery request, CancellationToken cancellationToken)
    {
        GetAdjustItemTransactionByIDResponseDTO resData = (from a in await _unitOfWork.Repository<TTAdjustItemTransaction>().QueryAsync()
                                                           join b in await _unitOfWork.Repository<TMAdjustItemType>().QueryAsync() on a.AdjustTypeID equals b.AdjustTypeID
                                                           join c in await _unitOfWork.Repository<TMItem>().FindWithInclude(w => w.IsActive,
                                                           i => i.Include(x => x.ItemType),
                                                           ii => ii.Include(ww => ww.Brand)) on a.ItemID equals c.ItemID
                                                           join emp in await _unitOfWork.Repository<TMEmployee>().FindWithInclude(w => w.IsActive, i => i.Include(ic => ic.User)) 
                                                           on a.CreatedBy equals emp.User.UserName into tUser
                                                           from jUser in tUser.DefaultIfEmpty()
                                                           where a.IsActive && a.AdjustID == request.adjusttransactionid
                                                           select new GetAdjustItemTransactionByIDResponseDTO
                                                           {
                                                               adjustid = a.AdjustTypeID,
                                                               adjusttypeid = a.AdjustTypeID,
                                                               adjusttypename = b.AdjustTypeName,
                                                               branchid = a.BranchID,
                                                               itemid = c.ItemID,
                                                               itemcode = c.ItemCode,
                                                               itemname = c.Name,
                                                               itemtypeid = c.ItemTypeID,
                                                               itemtypename = c.ItemType.ItemTypeName,
                                                               itembrandid = c.BrandID,
                                                               itembrandname = c.Brand.BrandName,
                                                               qty = a.Qty,
                                                               remark = a.Remark,
                                                               createdby = jUser != null ? jUser.FirstName : "N/A",
                                                               createddate = a.CreadedDate,
                                                               isactive = a.IsActive
                                                           }).FirstOrDefault();
        if (resData == null)
        {
            throw new Exception("ไม่พบข้อมูลการปรับสต๊อก");
        }
        return new BaseResponse<GetAdjustItemTransactionByIDResponseDTO>
        {
            result = true,
            data = resData,
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
