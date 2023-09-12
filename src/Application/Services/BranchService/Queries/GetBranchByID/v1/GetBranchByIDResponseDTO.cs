using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.BranchService.Queries.GetBranchByID.v1;

[Serializable]
public class GetBranchByIDResponseDTO
{
    public int branchid { get; set; }

    public string branchcode { get; set; }

    public string branchname { get; set; }

	public string address1 { get; set; }

	public string address2 { get; set; }

	public string subdistrictcode { get; set; }

	public string districtcode { get; set; }

	public string provincecode { get; set; }

	public string zipcode { get; set; }

}
