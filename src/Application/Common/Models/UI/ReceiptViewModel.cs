using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;
public class ReceiptViewModel
{
    public string InvoiceNo => $"CY{new Random().Next(100000, 999999).ToString()}";
    public int ReceiveTempID { get; set; }
    public int BranchID { get; set; }
    public string CompanyName { get; set; }
    public string CompanyAddress { get; set; }
    public string? AdditionalHeaderText { get; set; }
    public string? ShopFooterText { get; set; }
    public string? AdditionalFooterText { get; set; }
    public string TelephoneNo { get; set; }
    public string CompanyTaxNo { get; set; }
    public string PrinterName { get; set; }

    public List<SellingBarcodeItemViewModel> Items { get; set; } = new();
    public decimal SubTotal { get; set; }
    public decimal Discount { get; set; }
    public decimal Shipping { get; set; }
    public decimal Vat { get; set; }
    public decimal TotalBill { get; set; }
    public decimal Due { get; set; }
    public DateTime Date { get; set; }
}
