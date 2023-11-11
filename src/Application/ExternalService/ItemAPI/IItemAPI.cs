using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemService.Commands.CreateItem;
using CYRetailIMS.Application.Services.ItemService.Commands.CreateItemList;
using CYRetailIMS.Application.Services.ItemService.Commands.DeleteItem;
using CYRetailIMS.Application.Services.ItemService.Commands.UpdateItem;
using CYRetailIMS.Application.Services.ItemService.Queries.GetItemByID.v1;
using CYRetailIMS.Application.Services.ItemService.Queries.GetItemList.v1;

namespace CYRetailIMS.Application.ExternalService.ItemAPI;
public interface IItemAPI
{
    Task<BaseResponse<CommandResponse>> CreateItemAsync(CreateItemCommand createItemCommand);
	Task<BaseResponse<CommandResponse>> CreateItemListAsync(CreateItemListCommand createItemListCommand);
	Task<BaseResponse<CommandResponse>> UpdateItemAsync(UpdateItemCommand updateItemCommand);
    Task<BaseResponse<CommandResponse>> DeleteItemAsync(DeleteItemCommand deleteItemCommand);
    Task<BaseResponse<List<GetItemListResponseDTO>>> GetItemListAsync();
    Task<BaseResponse<GetItemListResponseDTO>> GetItemByIdAsync(int itemID);
    Task<BaseResponse<GetItemByIDResponseDTO>> GetItemByBarCodeAsync(string itemBarcode);
}
