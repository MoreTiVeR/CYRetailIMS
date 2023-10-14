using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Behaviours;
using CYRetailIMS.Application.Common.Mappings.UI;
using CYRetailIMS.Application.Services.AdjustItemTypeService.Queries.GetAdjustItemType.v1;
using CYRetailIMS.Application.Services.AdjustItemTypeService.Queries.GetAdjustItemTypeByID.v1;
using CYRetailIMS.Application.Services.ApproveStatusService.Queries.GetApproveStatus.v1;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchByID.v1;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchList.v1;
using CYRetailIMS.Application.Services.CurrencyService.Queries.GetCurrencyList.v1;
using CYRetailIMS.Application.Services.DepartmentService.Queries.GetDepartmentByID.v1;
using CYRetailIMS.Application.Services.DepartmentService.Queries.GetDepartments.v1;
using CYRetailIMS.Application.Services.EmployeeService.Commands.CreateEmployee.v1;
using CYRetailIMS.Application.Services.ItemBrandService.Queries.GetItemBrandByID.v1;
using CYRetailIMS.Application.Services.ItemBrandService.Queries.GetItemBrandList.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchID.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchList.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchList.v1;
using CYRetailIMS.Application.Services.ItemService.Commands.CreateItem;
using CYRetailIMS.Application.Services.ItemService.Commands.UpdateItem;
using CYRetailIMS.Application.Services.ItemService.Queries.GetItemByID.v1;
using CYRetailIMS.Application.Services.ItemService.Queries.GetItemList.v1;
using CYRetailIMS.Application.Services.ItemTransferStatusService.Queries.GetItemTransferStatus.v1;
using CYRetailIMS.Application.Services.ItemTransferStatusService.Queries.GetItemTransferStatusByID.v1;
using CYRetailIMS.Application.Services.ItemTypeService.Queries.GetItemTypeByID.v1;
using CYRetailIMS.Application.Services.ItemTypeService.Queries.GetItemTypeList.v1;
using CYRetailIMS.Application.Services.MenuService.Queries.GetMenuByRoleID.v1;
using CYRetailIMS.Application.Services.PaymentTypeService.Queries.GetPaymentTypeList.v1;
using CYRetailIMS.Application.Services.PurchaseOrderService.Commands.CreatePurchaseOrder.v1;
using CYRetailIMS.Application.Services.PurchaseTypeService.Queries.GetPurchaseTypeList.v1;
using CYRetailIMS.Application.Services.RoleService.Queries.GetRoleByID.v1;
using CYRetailIMS.Application.Services.RoleService.Queries.GetRoles.v1;
using CYRetailIMS.Application.Services.ShipmentTypeService.Queries.GetShipmentTypeList.v1;
using CYRetailIMS.Application.Services.TransferTypeService.Queries.GetTransferTypeByID.v1;
using CYRetailIMS.Application.Services.TransferTypeService.Queries.GetTransferTypeList.v1;
using CYRetailIMS.Application.Services.UnitOfMeasureService.Queries.GetUnitOfMeasureByID.v1;
using CYRetailIMS.Application.Services.UnitOfMeasureService.Queries.GetUnitOfMeasureList.v1;
using CYRetailIMS.Application.Services.UserInBranchService.Queries.GetUserInBranchByUserID.v1;
using CYRetailIMS.Application.Services.UserService.Commands.CreateUser.v1;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CYRetailIMS.Application;

public static class ConfigureService
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        #region Auto Mapper Configurations
        var mappingConfig = new MapperConfiguration(mc =>
        {
            #region API
            mc.AddProfile<CreateEmployeeMappingProfile>();
            mc.AddProfile<GetMenuByRoleIDMappingProfile>();
            mc.AddProfile<GetUnitOfMeasureByIDMappingProfile>();
            mc.AddProfile<GetUnitOfMeasureListMappingProfile>();

            mc.AddProfile<CreateItemMappingProfile>();
            mc.AddProfile<UpdateItemMappingProfile>();
            mc.AddProfile<GetItemByIDMappingProfile>();
            mc.AddProfile<GetItemListMappingProfile>();

            mc.AddProfile<GetItemTypeByIDMappingProfile>();
            mc.AddProfile<GetItemTypeListMappingProfile>();

            mc.AddProfile<GetItemBrandByIDMappingProfile>();
            mc.AddProfile<GetItemBrandListMappingProfile>();
            mc.AddProfile<GetItemInBranchByBranchListMappingProfile>();

            mc.AddProfile<GetBranchByIDMappingProfile>();
            mc.AddProfile<GetBranchListMappingProfile>();

            mc.AddProfile<GetItemInBranchByBranchIDMappingProfile>();
            mc.AddProfile<GetItemInBranchListMappingProfile>();

            mc.AddProfile<GetUserInBranchByUserIDMappingProfile>();

            mc.AddProfile<GetApproveStatusMappingProfile>();

            mc.AddProfile<GetItemTransferStatusMappingProfiel>();

            mc.AddProfile<CreateUserMappingProfile>();

            mc.AddProfile<GetTransferTypeByIDMappingProfile>();
            mc.AddProfile<GetTransferTypeListMappingProfile>();
            mc.AddProfile<GetItemTransferStatusByIDMappigProfile>();

            mc.AddProfile<GetDepartmentsMappingProfile>();
            mc.AddProfile<GetDepartmentByIDMappingProfile>();
            mc.AddProfile<GetRolesMappingProfile>();
            mc.AddProfile<GetRoleByIDMappingProfile>();


            mc.AddProfile<GetAdjustItemTypeMappingProfile>();
            mc.AddProfile<GetAdjustItemTypeByIDMappingProfile>();


            mc.AddProfile<CreatePurchaseOrderMappingProfile>();

            mc.AddProfile<GetCurrencyListMappingProfile>();

            mc.AddProfile<GetPaymentTypeListMappingProfile>();

            mc.AddProfile<GetPurchaseTypeMappingProfile>();

            mc.AddProfile<GetShipmentTypeMappingProfile>();
            #endregion
        });
        IMapper mapper = mappingConfig.CreateMapper();
        services.AddSingleton(mapper);
        #endregion
        return services;
    }
}
