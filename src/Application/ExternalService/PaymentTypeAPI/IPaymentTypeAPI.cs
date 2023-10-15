using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.PaymentTypeService.Queries.GetPaymentTypeList.v1;
using CYRetailIMS.Application.Services.PaymentTypeService.Queries.PaymentTypeByID.v1;

namespace CYRetailIMS.Application.ExternalService.PaymentTypeAPI;
public interface IPaymentTypeAPI
{
	Task<BaseResponse<List<GetPaymentTypeListResponseDTO>>> GetPaymentTypeListAsync();
	Task<BaseResponse<PaymentTypeByIDResponseDTO>> PaymentTypeByIDAsync(int paymentTypeID);
}
