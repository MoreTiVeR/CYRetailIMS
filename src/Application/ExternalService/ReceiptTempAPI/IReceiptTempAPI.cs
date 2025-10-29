
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ReceiveTempService.Commands.CreateReceiveTemplate.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Commands.DeleteReceiveTemplate.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Commands.UpdateReceiveTemplate.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Queries.GetReceiveTempByBranchID.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Queries.GetReceiveTempByCriteria.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Queries.GetReceiveTempByID.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Queries.GetReceiveTempList.v1;

namespace CYRetailIMS.Application.ExternalService.ReceiptTempAPI;
public interface IReceiptTempAPI
{
    Task<BaseResponse<CommandResponse>> CreateBranchAsync(CreateReceiveTemplateCommand createReceiveTemplateCommand);
    Task<BaseResponse<CommandResponse>> UpdateBranchAsync(UpdateReceiveTemplateCommand updateReceiveTemplateCommand);
    Task<BaseResponse<CommandResponse>> DeleteBranchAsync(DeleteReceiveTemplateCommand deleteReceiveTemplateCommand);
    Task<BaseResponse<List<GetReceiveTempResponseDTO>>> GetReceiptTemplateListAsync();
    Task<BaseResponse<GetReceiveTempResponseDTO>> GetReceiptTemplateByIDAsync(GetReceiveTempByIDQuery objReq);
    Task<BaseResponse<GetReceiveTempResponseDTO>> GetReceiptTemplateByBranchIDAsync(GetReceiveTempByBranchIDQuery objReq);
    Task<BaseResponse<GetReceiveTempByCriteriaResponseDTO>> GetReceiptTemplateByCriteriaAsync(GetReceiveTempByCriteriaQuery objReq);
}
