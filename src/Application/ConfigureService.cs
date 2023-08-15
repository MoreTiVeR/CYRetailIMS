using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Behaviours;
using CYRetailIMS.Application.Services.EmployeeService.Commands.CreateEmployee;
using CYRetailIMS.Application.Services.ItemBrandService.Queries.GetItemBrandByID.v1;
using CYRetailIMS.Application.Services.ItemBrandService.Queries.GetItemBrandList.v1;
using CYRetailIMS.Application.Services.ItemService.Commands.CreateItem;
using CYRetailIMS.Application.Services.ItemService.Queries.GetItemByID.v1;
using CYRetailIMS.Application.Services.ItemService.Queries.GetItemList.v1;
using CYRetailIMS.Application.Services.ItemTypeService.Queries.GetItemTypeByID.v1;
using CYRetailIMS.Application.Services.ItemTypeService.Queries.GetItemTypeList.v1;
using CYRetailIMS.Application.Services.MenuService.Queries.GetMenuByRoleID.v1;
using CYRetailIMS.Application.Services.UnitOfMeasureService.Queries.GetUnitOfMeasureByID.v1;
using CYRetailIMS.Application.Services.UnitOfMeasureService.Queries.GetUnitOfMeasureList.v1;
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
            mc.AddProfile<GetItemByIDMappingProfile>();
            mc.AddProfile<GetItemListMappingProfile>();

            mc.AddProfile<GetItemTypeByIDMappingProfile>();
            mc.AddProfile<GetItemTypeListMappingProfile>();

            mc.AddProfile<GetItemBrandByIDMappingProfile>();
            mc.AddProfile<GetItemBrandListMappingProfile>();
            
            #endregion
        });
        IMapper mapper = mappingConfig.CreateMapper();
        services.AddSingleton(mapper);
        #endregion
        return services;
    }
}
