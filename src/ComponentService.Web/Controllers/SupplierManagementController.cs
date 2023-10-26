using System.Collections.Generic;
using AutoMapper;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.ExternalService.SupplierAPI;
using CYRetailIMS.Application.ExternalService.SupplierContactTypeAPI;
using CYRetailIMS.Application.ExternalService.SupplierTypeAPI;
using CYRetailIMS.Application.Services.PurchaseOrderService.Queries.GetPurchaseOrderList.v1;
using CYRetailIMS.Application.Services.SupplierContactTypeService.Queries.GetSupplierContactTypeList.v1;
using CYRetailIMS.Application.Services.SupplierService.Commands.CreateSupplier.v1;
using CYRetailIMS.Application.Services.SupplierService.Commands.DeleteSupplier.v1;
using CYRetailIMS.Application.Services.SupplierService.Commands.UpdateSupplier.v1;
using CYRetailIMS.Application.Services.SupplierService.Queries.GetSupplierList.v1;
using CYRetailIMS.Application.Services.SupplierTypeService.Queries.GetSupplierTypeList.v1;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using Microsoft.AspNetCore.Mvc;
using static CYRetailIMS.Application.Common.Models.EnumModel;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin)]
public class SupplierManagementController : BaseController
{
    private readonly ISupplierAPI _supplierAPI;
    private readonly ISupplierContactTypeAPI _supplierContactTypeAPI;
    private readonly ISupplierTypeAPI _supplierTypeAPI;
    public SupplierManagementController(IHttpClientRequest httpClientRequest, IMapper mapper,
        ILog4NetLogger log,
        ISupplierAPI supplierAPI,
        ISupplierContactTypeAPI supplierContactTypeAPI,
        ISupplierTypeAPI supplierTypeAPI) : base(httpClientRequest, mapper, log)
    {
        _supplierAPI = supplierAPI;
        _supplierContactTypeAPI = supplierContactTypeAPI;
        _supplierTypeAPI = supplierTypeAPI;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> CreateAsync()
    {
        BaseResponse<List<GetSupplierTypeResponseDTO>> resSupplierType = await _supplierTypeAPI.GetSupplierTypeListAsync();
        BaseResponse<List<GetSupplierContactTypeResposeDTO>> resSupplierContactType = await _supplierContactTypeAPI.GetSupplierContactTypeListAsync();

        ViewBag.SupplierTypeList = resSupplierType;
        ViewBag.SupplierContactTypeList = resSupplierContactType;
        return View();
    }

    public async Task<IActionResult> Edit(int supplierid)
    {
        BaseResponse<GetSupplierResponseDTO> resSupplier = await _supplierAPI.GetSupplierByIDAsync(supplierid);
        EditSupplierViewModel editSupplierViewModel = MappingEditSupplier(resSupplier.data);

        BaseResponse<List<GetSupplierTypeResponseDTO>> resSupplierType = await _supplierTypeAPI.GetSupplierTypeListAsync();
        BaseResponse<List<GetSupplierContactTypeResposeDTO>> resSupplierContactType = await _supplierContactTypeAPI.GetSupplierContactTypeListAsync();

        ViewBag.SupplierTypeList = resSupplierType;
        ViewBag.SupplierContactTypeList = resSupplierContactType;
        return View(editSupplierViewModel);
    }


    [HttpPost]
    public async Task<IActionResult> CreateSupplier([FromBody] CreateSupplierViewModel createSupplierView)
    {
        try
        {
            CreateSupplierCommand createSupplier = MappingCreateSupplierCommand(createSupplierView);
            var res = await _supplierAPI.CreateSupplierAsync(createSupplier);
            return Json(new { result = res.result, message = res.result ? "เพิ่มข้อมูลซัฟพลายเออร์สำเร็จ" : $"ไม่สามารถทำรายการได้, {res.error.error.message}" });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateSupplier([FromBody] EditSupplierViewModel editSupplierView)
    {
        try
        {
            UpdateSupplierCommand updateSupplier = MappingUpdateSupplierCommand(editSupplierView);
            var res = await _supplierAPI.UpdateSupplierAsync(updateSupplier);
            return Json(new { result = res.result, message = res.result ? "ปรับปรุงข้อมูลซัฟพลายเออร์สำเร็จ" : $"ไม่สามารถทำรายการได้, {res.error.error.message}" });

        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> DeleteSupplier([FromBody] DeleteSupplierViewModel  deleteSupplierView)
    {
        try
        {
            DeleteSupplierCommand deleteSupplierCommand = new DeleteSupplierCommand
            {
                supplierid = deleteSupplierView.supplierid,
                deleteddby = base.UserProfile.rolename,
                deleteddate = DateTime.Now
            };
            var res = await _supplierAPI.DeleteSupplierAsync(deleteSupplierCommand);
            return Json(new { result = res.result, message = res.result ? "ลบข้อมูลซัฟพลายเออร์สำเร็จ" : $"ไม่สามารถทำรายการได้, {res.error.error.message}" });

        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetSuppliers()
    {
        List<GetSupplierResponseDTO> supplierList;
        try
        {
            BaseResponse<List<GetSupplierResponseDTO>> resSuppliers = await _supplierAPI.GetSupplierListAsync();
            if (!resSuppliers.result)
            {
                throw new Exception(resSuppliers.error.error.message);
            }
            return Json(new { data = resSuppliers.data });
        }
        catch
        {
            supplierList = new List<GetSupplierResponseDTO>();
            return Json(new { data = supplierList });
        }
    }

    private EditSupplierViewModel MappingEditSupplier(GetSupplierResponseDTO supplierDTO)
    {
        return _mapper.Map<EditSupplierViewModel>(supplierDTO);
    }

    private CreateSupplierCommand MappingCreateSupplierCommand(CreateSupplierViewModel supplierData)
    {
        var createCommand = new CreateSupplierCommand
        {
            suppliernameth = supplierData.suppliername_th,
            suppliernameen = supplierData.suppliername_th,
            suppliertypeid = (int)SupplierTypes.Wholesalers,
            description = supplierData.description, //address
            contact = new List<CreateSupplierContact>
            {
                new CreateSupplierContact
                {
                    suppliercontacttypeid = (int)SupplierContactTypes.Email,
                    contactaccountname = supplierData.contactperson,
                    contactperson = supplierData.contactperson,
                    mobileno = supplierData.mobileno
                }
            },
            createdby = base.UserProfile.username,
            createddate = DateTime.Now
        };
        return createCommand;
    }

    private UpdateSupplierCommand MappingUpdateSupplierCommand(EditSupplierViewModel supplierData)
    {
        var updateCommand = new UpdateSupplierCommand
        {
            supplierid = supplierData.supplierid,
            suppliernameth = supplierData.suppliername_th,
            suppliernameen = supplierData.suppliername_en,
            suppliertypeid = supplierData.suppliertypeid,
            description = supplierData.description,
            contact = new List<UpdateSupplierContact>
            {
                new UpdateSupplierContact
                {
                    suppliercontacttypeid = supplierData.suppliercontacttypeid,
                    contactaccountname = supplierData.contactaccountname,
                    contactperson = supplierData.contactperson,
                    mobileno = supplierData.mobileno,
                    desctiption = supplierData.description
                }
            },
            updatedby = base.UserProfile.username,
            updateddate = DateTime.Now
        };
        return updateCommand;
    }
}
