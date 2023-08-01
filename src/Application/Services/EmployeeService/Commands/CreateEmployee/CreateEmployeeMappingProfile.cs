using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.EmployeeService.Commands.CreateEmployee;
public class CreateEmployeeMappingProfile : Profile
{
    public CreateEmployeeMappingProfile()
    {
        CreateMap<CreateEmployeeCommand, TMEmployee>();
    }
}
