using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchID.v1;
using CYRetailIMS.Application.Services.ItemService.Queries.GetItemList.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByTransferID.v1;

namespace CYRetailIMS.Application.Common.Mappings.UI.Item;
public class ItemMappingProfile : Profile
{
	public ItemMappingProfile()
	{
		CreateMap<GetItemListResponseDTO, EditItemViewModel>()
			.ForMember(x => x.ItemID, f => f.MapFrom(ff => ff.itemid))
			.ForMember(x => x.ItemTypeID, f => f.MapFrom(ff => ff.itemtypeid))
			.ForMember(x => x.BrandID, f => f.MapFrom(ff => ff.brandid))
			.ForMember(x => x.UnitOfMeasureID, f => f.MapFrom(ff => ff.unitofmeasureid))
			.ForMember(x => x.Description, f => f.MapFrom(ff => ff.description))
			.ForMember(x => x.ItemCode, f => f.MapFrom(ff => ff.itemcode))
			.ForMember(x => x.ItemImageUrl, f => f.MapFrom(ff => ff.itemimageurl))
			.ForMember(x => x.Price, f => f.MapFrom(ff => ff.price))
			.ForMember(x => x.DiscountPercent, f => f.MapFrom(ff => ff.discountpercent))
			.ForMember(x => x.ShortName, f => f.MapFrom(ff => ff.shortname))
			.ForMember(x => x.Qty, f => f.MapFrom(ff => ff.qty))
			.ForMember(x => x.Cost, f => f.MapFrom(ff => ff.cost))
			.ForMember(x => x.IsActive, f => f.MapFrom(ff => ff.isactive));

		CreateMap<GetItemInBranchByBranchIDItemResponseDTO, EditItemViewModel>()
			.ForMember(x => x.ItemID, f => f.MapFrom(ff => ff.itemid))
			.ForMember(x => x.ItemTypeID, f => f.MapFrom(ff => ff.itemtypeid))
			.ForMember(x => x.BrandID, f => f.MapFrom(ff => ff.brandid))
			.ForMember(x => x.UnitOfMeasureID, f => f.MapFrom(ff => ff.unitofmeasureid))
			.ForMember(x => x.Description, f => f.MapFrom(ff => ff.description))
			.ForMember(x => x.ItemCode, f => f.MapFrom(ff => ff.itemcode))
			.ForMember(x => x.ItemImageUrl, f => f.MapFrom(ff => ff.itemimageurl))
			.ForMember(x => x.Price, f => f.MapFrom(ff => ff.price))
			.ForMember(x => x.DiscountPercent, f => f.MapFrom(ff => ff.discountpercent))
			.ForMember(x => x.ShortName, f => f.MapFrom(ff => ff.shortname))
			.ForMember(x => x.Qty, f => f.MapFrom(ff => ff.qty))
			.ForMember(x => x.Cost, f => f.MapFrom(ff => ff.cost))
			.ForMember(x => x.IsActive, f => f.MapFrom(ff => ff.isactive))
			.ForMember(x => x.Name, f => f.MapFrom(ff => ff.itemname));

		CreateMap<GetItemTransferResponseDTO, ReceiveTransferItemViewModel>()
			.ForMember(x => x.TransferID, f => f.MapFrom(ff => ff.transferid))
			.ForMember(x => x.TransferTypeID, f => f.MapFrom(ff => ff.transfertypeid))
			.ForMember(x => x.SourceID, f => f.MapFrom(ff => ff.sourceid))
			.ForMember(x => x.DestinationID, f => f.MapFrom(ff => ff.destinationid))
			.ForMember(x => x.Description, f => f.MapFrom(ff => ff.description))
			.ForMember(x => x.ItemID, f => f.MapFrom(ff => ff.itemid))
            //.ForMember(x => x.ItemName, f => f.MapFrom(ff => ff.itemname))
            .ForMember(x => x.QTY, f => f.MapFrom(ff => ff.qty))
			.ForMember(x => x.TransferStatusID, f => f.MapFrom(ff => ff.transferstatusid));

		CreateMap<GetItemInBranchByBranchIDItemResponseDTO, GetItemListResponseDTO>()
			.ForMember(w => w.itemid, f => f.MapFrom(w => w.itemid))
			.ForMember(w => w.itemcode, f => f.MapFrom(w => w.itemcode))
			.ForMember(w => w.itemtypeid, f => f.MapFrom(w => w.itemtypeid))
			.ForMember(w => w.itemtypename, f => f.MapFrom(w => w.itemtypename))
			.ForMember(w => w.unitofmeasureid, f => f.MapFrom(w => w.unitofmeasureid))
			.ForMember(w => w.unitofmeasurename, f => f.MapFrom(w => w.unitofmeasurename))
			.ForMember(w => w.brandid, f => f.MapFrom(w => w.brandid))
			.ForMember(w => w.brandname, f => f.MapFrom(w => w.brandname))
			.ForMember(w => w.name, f => f.MapFrom(w => w.itemname))
			.ForMember(w => w.shortname, f => f.MapFrom(w => w.shortname))
			.ForMember(w => w.description, f => f.MapFrom(w => w.description))
			.ForMember(w => w.barcode, f => f.MapFrom(w => w.barcode))
			.ForMember(w => w.cost, f => f.MapFrom(w => w.cost))
			.ForMember(w => w.price, f => f.MapFrom(w => w.price))
			.ForMember(w => w.itemimageurl, f => f.MapFrom(w => w.itemimageurl))
			.ForMember(w => w.qty, f => f.MapFrom(w => w.qty))
			.ForMember(w => w.notifyminqty, f => f.MapFrom(w => w.notifyminqty))
			.ForMember(w => w.discountpercent, f => f.MapFrom(w => w.discountpercent))
			.ForMember(w => w.createdby, f => f.MapFrom(w => w.createdby))
			.ForMember(w => w.createddate, f => f.MapFrom(w => w.createddate))
			.ForMember(w => w.updatedby, f => f.MapFrom(w => w.updatedby))
			.ForMember(w => w.updateddate, f => f.MapFrom(w => w.updateddate))
			.ForMember(w => w.isactive, f => f.MapFrom(w => w.isactive));

    }
}
