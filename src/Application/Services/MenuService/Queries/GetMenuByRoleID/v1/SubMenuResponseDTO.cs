using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.MenuService.Queries.GetMenuByRoleID.v1;
public class SubMenuResponseDTO
{
    public int SubMenuID { get; set; }

    public int MenuID { get; set; }

    public int Seq { get; set; }

    public string MenuName_EN { get; set; } = null!;

    public string MenuName_TH { get; set; } = null!;

    public string? Description { get; set; }

    public string? CMS_ControllerName { get; set; }

    public string? CMS_ActionName { get; set; }

    public string? CMS_I_Class { get; set; }

    public string? CMS_Span_Class { get; set; }

    public string? CMS_Link { get; set; }
    public bool IsActive { get; set; }
}
