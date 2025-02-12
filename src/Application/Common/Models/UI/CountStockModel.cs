using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Common.Models.UI;
public class CountStockModel
{
    public List<CountStockDetail> products { get; set; }
}
