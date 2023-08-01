using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Domain.Infrastructure.Database;

namespace CYRetailIMS.Application.Services;
public class BaseService
{
    protected readonly IMapper _mapper;
    protected readonly IUnitOfWork _unitOfWork;

    public BaseService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
}
