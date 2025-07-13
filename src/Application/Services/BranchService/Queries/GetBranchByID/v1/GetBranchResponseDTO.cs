using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.BranchService.Queries.GetBranchByID.v1;

[Serializable]
public class GetBranchResponseDTO
{
    public int branchid { get; set; }

    public string branchcode { get; set; }

    public string branchname { get; set; }

	public string address1 { get; set; }

	public string address2 { get; set; }

	public int subdistrictid { get; set; }

	public int districtid { get; set; }

	public int provinceid { get; set; }

	public int zipcode { get; set; }

    public bool isactive { get; set; }

}
