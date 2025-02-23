using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStockByID.v1;
public record InquiryCountStockByIDQuery : IRequest<BaseResponse<InquiryCountStockByIDResponseDTO>>
{
    public int countstockid { get; set; }
}
