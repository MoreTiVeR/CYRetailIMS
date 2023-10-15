using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.PurchaseOrderService.Commands.ApprovePurchaseOrder.v1;
using CYRetailIMS.Application.Services.PurchaseOrderService.Commands.CreatePurchaseOrder.v1;
using CYRetailIMS.Application.Services.PurchaseOrderService.Commands.DeletePurchaseOrder.v1;
using CYRetailIMS.Application.Services.PurchaseOrderService.Commands.UpdatePurchaseOrder.v1;
using CYRetailIMS.Application.Services.PurchaseOrderService.Queries.GetPurchaseOrderByID.v1;
using CYRetailIMS.Application.Services.PurchaseOrderService.Queries.GetPurchaseOrderByPONumber.v1;
using CYRetailIMS.Application.Services.PurchaseOrderService.Queries.GetPurchaseOrderList.v1;

namespace CYRetailIMS.Application.ExternalService.PurchaseOrderAPI;
public interface IPurchaseOrderAPI
{
	Task<BaseResponse<CommandResponse>> CreatePurchaseOrderAsync(CreatePurchaseOrderCommand createPurchaseOrderCommand);
	Task<BaseResponse<CommandResponse>> UpdatePurchaseOrderAsync(UpdatePurchaseOrderCommand updatePurchaseOrderCommand);
	Task<BaseResponse<CommandResponse>> DeletePurchaseOrderAsync(DeletePurchaseOrderCommand deletePurchaseOrderCommand);
	Task<BaseResponse<CommandResponse>> ApprovePurchaseOrderAsync(ApprovePurchaseOrderCommand approvePurchaseOrderCommand);
	Task<BaseResponse<List<GetPurchaseOrderResposeDTO>>> GetPurchaseOrderListAsync();
	Task<BaseResponse<GetPurchaseOrderResposeDTO>> GetPurchaseOrderByIDAsync(int purchaseorderID);
	Task<BaseResponse<GetPurchaseOrderResposeDTO>> GetPurchaseOrderByPONumberAsync(string purchaseOrderNo);
}
