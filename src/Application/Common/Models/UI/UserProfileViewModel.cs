using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Services.AccountService.Queries.Login.v1;

namespace CYRetailIMS.Application.Common.Models.UI;
public class UserProfileViewModel : UserProfileResponseDTO
{
    public string homepage_url { get; set; }
}
