using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.TransactionTypeService.Queries.GetTrasnactionList.v1;

namespace CYRetailIMS.Application.ExternalService.TransactionTypeAPI;
public interface ITransactionTypeAPI
{
    Task<BaseResponse<List<GetTrasnactionByCriteriaResponseDTO>>> GetTransactionTypeByCriteriaAsync(GetTrasnactionByCriteriaQuery reqObj);
}
