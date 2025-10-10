using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Extensions;
public static class VatCalculatorExtension
{
    private const decimal VatRate = 0.07m;
    /// <summary>
    /// คำนวณ VAT จากราคาที่ "ยังไม่รวม VAT"
    /// </summary>
    public static decimal CalculateVatFromExclusive(this decimal subTotal)
    {
        try
        {
            return Math.Round(subTotal * VatRate, 2, MidpointRounding.AwayFromZero);
        }
        catch
        {
            return 0;
        }
    }

    /// <summary>
    /// แปลงราคาที่ "รวม VAT แล้ว" ให้ได้ "ยอดก่อน VAT"
    /// </summary>
    public static decimal ExcludeVat(this decimal totalPrice)
    {
        try
        {
            // ตัวอย่าง: 107 -> 107 / 1.07 = 100
            return Math.Round(totalPrice / (1 + VatRate), 2, MidpointRounding.AwayFromZero);
        }
        catch
        {
            return 0;
        }
    }

    /// <summary>
    /// แปลงราคาที่ยังไม่รวม VAT ให้ได้ "ราคาที่รวม VAT แล้ว"
    /// </summary>
    public static decimal IncludeVat(this decimal subTotal)
    {
        try
        {
            return Math.Round(subTotal * (1 + VatRate), 2, MidpointRounding.AwayFromZero);
        }
        catch
        {
            return 0;
        }
    }
}

public static class VatHelper
{
    private const decimal VatRate = 0.07m;

    /// <summary>
    /// คำนวณผลรวมทั้งหมด พร้อม VAT, Discount, Shipping
    /// รองรับทั้งกรณีรวม VAT และไม่รวม VAT
    /// </summary>
    public static (decimal SubTotalExcludeVat, decimal Vat, decimal TotalBill)CalculateTotal(decimal subTotal, decimal discount, decimal shipping, bool isVatInclusive)
    {
        // ป้องกันค่าติดลบ
        subTotal = Math.Max(0, subTotal);
        discount = Math.Max(0, discount);
        shipping = Math.Max(0, shipping);

        static decimal R2(decimal v) => Math.Round(v, 2, MidpointRounding.AwayFromZero);

        if (!isVatInclusive)
        {
            // ---------- กรณียังไม่รวม VAT ----------
            decimal subTotalAfterDiscount = subTotal - discount;
            decimal vat = R2(subTotalAfterDiscount * VatRate);
            decimal totalBill = R2(subTotalAfterDiscount + vat + shipping);

            return (R2(subTotalAfterDiscount), vat, totalBill);
        }
        else
        {
            // ---------- กรณีรวม VAT แล้ว ----------
            // 1. แยกยอดก่อน VAT
            decimal subTotalExcludeVat = R2(subTotal / (1 + VatRate));

            // 2. หักส่วนลดจากยอดก่อน VAT
            decimal subTotalAfterDiscount = subTotalExcludeVat - discount;
            if (subTotalAfterDiscount < 0) subTotalAfterDiscount = 0;

            // 3. คำนวณ VAT จากยอดหลังส่วนลด
            decimal vat = R2(subTotalAfterDiscount * VatRate);

            // 4. รวมยอดสุทธิ (สินค้าหลังลด + VAT + ค่าส่ง)
            decimal totalBill = R2(subTotalAfterDiscount + vat + shipping);

            return (R2(subTotalAfterDiscount), vat, totalBill);
        }
    }
}
