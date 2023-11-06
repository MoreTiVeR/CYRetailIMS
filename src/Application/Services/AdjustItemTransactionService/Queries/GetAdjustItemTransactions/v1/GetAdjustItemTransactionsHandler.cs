using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.AdjustItemTypeService.Queries.GetAdjustItemType.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.AdjustItemTransactionService.Queries.GetAdjustItemTransactions.v1;
public class GetAdjustItemTransactionsHandler : BaseService, IRequestHandler<GetAdjustItemTransactionsQuery, BaseResponse<List<GetAdjustItemTransactionsResponseDTO>>>
{
    public GetAdjustItemTransactionsHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<GetAdjustItemTransactionsResponseDTO>>> Handle(GetAdjustItemTransactionsQuery request, CancellationToken cancellationToken)
    {
        List<GetAdjustItemTransactionsResponseDTO> resData = (from a in await _unitOfWork.Repository<TTAdjustItemTransaction>().QueryAsync()
                                                              join b in await _unitOfWork.Repository<TMAdjustItemType>().QueryAsync() on a.AdjustTypeID equals b.AdjustTypeID
                                                              join c in await _unitOfWork.Repository<TMItem>().FindWithInclude(w => w.IsActive, 
                                                              i => i.Include(x => x.ItemType), 
                                                              ii => ii.Include(ww => ww.Brand)) on a.ItemID equals c.ItemID
                                                              join emp in await _unitOfWork.Repository<TMEmployee>().FindWithInclude(w => w.IsActive, i => i.Include(ic => ic.User)) on a.CreatedBy equals emp.User.UserName 
                                                              into tUser
                                                              from jUser in tUser.DefaultIfEmpty()
                                                              where a.IsActive
                                                              select new GetAdjustItemTransactionsResponseDTO
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
                                                                  createdby = a.CreatedBy,
                                                                  createddate = a.CreatedDate,
                                                                  isactive = a.IsActive
                                                              }).ToList();
        if(resData.Count == 0)
        {
            throw new Exception("ไม่พบข้อมูลการปรับสต๊อก");
        }

        #region Update updatedby data from emp name
        List<string> userNameList = resData.Select(s => s.createdby).Distinct().ToList();
        IEnumerable<TMUsers> userList = await _unitOfWork.Repository<TMUsers>().FindWithInclude(w => userNameList.Contains(w.UserName), i => i.Include(w => w.TMEmployees));
        var empDataList = userList.Select(s => new { s.UserName, s.TMEmployees.FirstOrDefault().FirstName }).ToList();
        resData = resData.Select(s =>
        {
            if (!string.IsNullOrEmpty(s.createdby))
            {
                s.createdby = empDataList.FirstOrDefault(w => w.UserName == s.createdby) != null
                ? empDataList.FirstOrDefault(w => w.UserName == s.createdby).FirstName : s.createdby;
            }
            return s;
        }).ToList();
        #endregion

        return new BaseResponse<List<GetAdjustItemTransactionsResponseDTO>>
        {
            result = true,
            data = resData,
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
