using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.MenuService.Queries.GetMenuByRoleID.v1;
public class MenusResponseDTO
{
    public int MenuID { get; set; }

    public int Seq { get; set; }

    public string MenuName_TH { get; set; }

    public string MenuName_EN { get; set; }

    public string Description { get; set; }

    public string CMS_DataIconName { get; set; }

    public string CMS_Link { get; set; }

    public string CMS_Title { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreadedDate { get; set; }

    public string UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool IsActive { get; set; }
}
