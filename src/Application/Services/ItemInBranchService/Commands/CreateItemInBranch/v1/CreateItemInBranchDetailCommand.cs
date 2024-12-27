using System.ComponentModel.DataAnnotations;

namespace CYRetailIMS.Application.Services.ItemInBranchService.Commands.CreateItemInBranch.v1;
public record CreateItemInBranchDetailCommand
{
    public int branchid { get; set; }

    [Required]
    public int itemid { get; set; }

    [Required]
    public string itemcode { get; init; }

    [Required]
    public int itemtypeid { get; init; }

    public int? subitemtypeid { get; set; }

    [Required]
    public int unitofmeasureid { get; init; }

    [Required]
    public int brandid { get; init; }

    [Required]
    public string name { get; init; }

    public string shortname { get; init; }

    public string description { get; init; }

    public string barcode { get; init; }

    [Required]
    public decimal price { get; init; }

    [Required]
    public float discountpercent { get; init; }

    [Required]
    public int qty { get; init; }

    [Required]
    public int notifyminqty { get; set; }

    public int? notifymaxqty { get; set; }

    /// <summary>
    /// Item image path
    /// </summary>
    public string itemimageurl { get; init; }

    [Required]
    public string createdby { get; init; }

    [Required]
    public bool isactive { get; init; }

    public bool isupdate { get; set; }
    public decimal cost { get; set; }
}
