using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Extensions;
public static class PurchaseOrderNoGenerator
{
	public static string GeneratePO()
	{
		try
		{
			return  $"PR-{DateTime.Now:ddMMyyyy}-{DateTime.Now:HHmmss}";
		}
		catch(Exception ex)
		{
			throw new Exception($"ไม่สามารถสร้างเลขออเดอร์ได้, กรุณาลองใหม่อีกครั้ง : {ex.Message}");
		}
	}
}
