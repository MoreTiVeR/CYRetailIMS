using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace CYRetailIMS.Application.Services.MenuService.Queries.GetMenuByRoleID.v1;

[Serializable]
public class SubMenuResponseDTO
{
	public int submenuid { get; set; }
	public int seq { get; set; }
	public string menuname_en { get; set; }
	public string menuname_th { get; set; }
	public string description { get; set; }
	public string cms_controllername { get; set; }
	public string cms_actionname { get; set; }
	public object cms_i_class { get; set; }
	public object cms_span_class { get; set; }
	public object cms_link { get; set; }
	public bool isactive { get; set; }
}
