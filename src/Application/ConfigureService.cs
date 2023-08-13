using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Behaviours;
using CYRetailIMS.Application.Services.EmployeeService.Commands.CreateEmployee;
using CYRetailIMS.Application.Services.MenuService.Queries.GetMenuByRoleID.v1;
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
            //mc.AddProfile<SampleBusinessValidatioMappingProfile>();
            //mc.AddProfile<GetEmployeeByCodeMappingProfile>();
            //mc.AddProfile<CreateEmployeeMappingProfile>();
            #endregion
        });
        IMapper mapper = mappingConfig.CreateMapper();
        services.AddSingleton(mapper);
        #endregion
        return services;
    }
}
