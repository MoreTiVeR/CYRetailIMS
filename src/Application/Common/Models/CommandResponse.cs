using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models;

[Serializable]
public class CommandResponse
{
    public bool result { get; set; }

    public int transactionid { get; set; }

    public ErrorData error { get; set; }

}

[Serializable]
public class ResponseTransactionData
{
    public int transactionid { get; set; }

}