using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using CYRetailIMS.Application.Common.Interfaces;
using AutoMapper;
using CYRetailIMS.Application.ExternalService.BranchAPI;
using CYRetailIMS.Application.Services.BranchService.Commands.CreateBranch.v1;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchList.v1;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchByID.v1;
using CYRetailIMS.Application.Services.BranchService.Commands.UpdateBranch.v1;
using CYRetailIMS.Application.Services.BranchService.Commands.DeleteBranch.v1;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin)]
public class BranchController : BaseController
{
    private readonly IBranchAPI _branchAPI;
    public BranchController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
        IBranchAPI branchAPI) : base(httpClientRequest, mapper, log)
    {
        _branchAPI = branchAPI;
    }

    public async Task<IActionResult> Index()
    {
        BaseResponse<List<GetBranchResponseDTO>> branchList = await _branchAPI.GetBranchListAsync();
        ViewBag.BranchList = branchList;
        return View();
    }

    public IActionResult Create()
    {
        return View();
    }

    public async Task<IActionResult> Edit(int branchid)
    {
        BaseResponse<GetBranchResponseDTO> resBranch = await _branchAPI.GetBranchByIDAsync(branchid);
        EditBranchViewModel branchDataModel = MappingEditData(resBranch.data);
        return View(branchDataModel);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBranch([FromBody] CreateBranchViewModel reqData)
    {
        CreateBranchCommand createBrandCommand = PrepareCreateBranchCommand(reqData);
        BaseResponse<CommandResponse> res = await _branchAPI.CreateBranchAsync(createBrandCommand);
        return Json(new { result = res.result, message = res.result ? "บันทึกข้อมูลสำเร็จ." : $"ไม่สามารถทำรายการได้, {res.error.error.message}" });
    }

    [HttpPost]
    public async Task<IActionResult> EditBracnh([FromBody] EditBranchViewModel editBranchObj)
    {
        UpdateBranchCommand updateBranchCommand = PrepareUpdateBranchCommand(editBranchObj);
        BaseResponse<CommandResponse> resUpdateItem = await _branchAPI.UpdateBranchAsync(updateBranchCommand);
        if (resUpdateItem.result)
        {
            return Json(new JsonViewModel { result = resUpdateItem.result, message = resUpdateItem.message });
        }
        return Json(new JsonViewModel { result = resUpdateItem.result, message = resUpdateItem.error.error.message });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteBranch([FromBody] DeleteBranchViewModel delBranchObj)
    {
        DeleteBranchCommand deleteBranchCommand = PrepareDeleteItemCommand(delBranchObj);
        BaseResponse<CommandResponse> resDelItem = await _branchAPI.DeleteBranchAsync(deleteBranchCommand);
        if (resDelItem.result)
        {
            return Json(new JsonViewModel { result = resDelItem.result, message = resDelItem.message });
        }
        return Json(new JsonViewModel { result = resDelItem.result, message = resDelItem.error.error.message });
    }

    private CreateBranchCommand PrepareCreateBranchCommand(CreateBranchViewModel createBranchViewModel)
    {
        return new CreateBranchCommand
        {
            branchname = createBranchViewModel.branchname,
            branchcode = createBranchViewModel.branchcode,
            address = createBranchViewModel.address,
            createdby = base.UserProfile.username,
            creadeddate = DateTime.Now,
            isactive = true
        };
    }

    private UpdateBranchCommand PrepareUpdateBranchCommand(EditBranchViewModel editBranchViewModel)
    {
        return new UpdateBranchCommand
        {
            branhid = editBranchViewModel.branchid,
            branchcode = editBranchViewModel.branchcode,
            branchname = editBranchViewModel.branchname,
            address = editBranchViewModel.address,
            updatedby = base.UserProfile.username
        };
    }

    private DeleteBranchCommand PrepareDeleteItemCommand(DeleteBranchViewModel deleteBranchViewModel)
    {
        return new DeleteBranchCommand
        {
            branhid = deleteBranchViewModel.branchid,
            updatedby = base.UserProfile.username
        };
    }

    private EditBranchViewModel MappingEditData(GetBranchResponseDTO resObj)
    {
        return new EditBranchViewModel
        {
            branchcode = resObj.branchcode,
            branchname = resObj.branchname,
            address = resObj.address1
        };
    }
}
