using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.ReportService.Queries.ItemTransferShortageReport.v1;
public class ItemTransferShortageReportHandler : BaseService, IRequestHandler<ItemTransferShortageReportQuery, BaseResponse<List<ItemTransferShortageReportResponseDTO>>>
{
    public ItemTransferShortageReportHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<ItemTransferShortageReportResponseDTO>>> Handle(ItemTransferShortageReportQuery request, CancellationToken cancellationToken)
    {
        var resReport = (from a in await _unitOfWork.Repository<TTItemTransferHistory>().QueryAsync()
                         join branch in await _unitOfWork.Repository<TMBranch>().QueryAsync() on a.BranchID equals branch.BranchID
                         join b in await _unitOfWork.Repository<TMItem>().QueryAsync() on a.ItemID equals b.ItemID
                         into jItem 
                         from item in jItem.DefaultIfEmpty()
                         join s in await _unitOfWork.Repository<TMSubItemType>().QueryAsync() on item.SubItemTypeID equals s.SubItemTypeID
                         into jSubitemType 
                         from subitemType in jSubitemType.DefaultIfEmpty()
                         where (a.SuggestRefillQtyBySystem - a.RefillQty) != 0
                         select new ItemTransferShortageReportResponseDTO
                         {
                             transferhistoryid = a.TransferHistoryID,
                             branchid = a.BranchID,
                             branchname = branch.BranchName,
                             itemid = a.ItemID,
                             itemcode = a.ItemCode,
                             itemname = a.ItemName,
                             itemtypeid = item.ItemTypeID,
                             itemtypename = item.ItemType.ItemTypeName,
                             brandid = a.BrandID,
                             brandname = item.Brand.BrandName,
                             subitemtypeid = subitemType != null ? subitemType.SubItemTypeID : null,
                             subitemtypename = subitemType != null ? subitemType.SubTypeNameTH : "ไม่มีประเภทย่อย",
                             suggestrefillqtybysystem = a.SuggestRefillQtyBySystem,
                             refillqty = a.RefillQty,
                             createddate = a.CreatedDate,
                         }).AsQueryable();

        if (request.branchid > 0)
        {
            resReport = resReport.Where(w => w.branchid == request.branchid);
        }

        if (request.subitemtypeid.HasValue && request.subitemtypeid > 0)
        {
            resReport = resReport.Where(w => w.subitemtypeid == request.subitemtypeid);
        }

        if (request.transferstartdate.HasValue)
        {
            resReport = resReport.Where(w => w.createddate.Date >= request.transferstartdate.Value.Date);
        }

        if (request.transferenddate.HasValue)
        {
            resReport = resReport.Where(w => w.createddate.Date <= request.transferenddate.Value.Date);
        }

        if (!resReport.Any())
        {
            throw new Exception("ไม่พบข้อมูล");
        }

        return new BaseResponse<List<ItemTransferShortageReportResponseDTO>>
        {
            result = true,
            data = resReport.OrderBy(s => s.transferhistoryid).ToList(),
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
