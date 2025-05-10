using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models;

public static class EnumModel
{
    public enum TransferSource
    {
        WAREHOUSE = 99
    }


    public enum SellTransactionType
    {
        /// <summary>
        /// Retail ขายปลีก
        /// </summary>
        RT = 1,

        /// <summary>
        /// Wholesale ขายส่ง
        /// </summary>
        ws = 2,

        /// <summary>
        /// ขายส่งด้วยเครื่องสแกนบาร์โค้ด Retail via BarCode Scannder
        /// </summary>
        RT01 = 3,

        /// <summary>
        /// Retail via Mobile Scannder ขายส่งด้วยสแกนผ่านกล้องมือถือ
        /// </summary>
        RT02 = 4
    }

    public enum SupplierTypes
    {
        /// <summary>
        /// ผู้ผลิต
        /// </summary>
        Manufacturers = 1,

        /// <summary>
        /// ผู้จัดจำหน่าย
        /// </summary>
        Distributors = 2,

        /// <summary>
        /// ผู้ค้าส่ง
        /// </summary>
        Wholesalers = 3
    }

    public enum SupplierContactTypes
    {
        /// <summary>
        /// ช่องทางการติดต่อผ่านทางอีเมล
        /// </summary>
        Email = 1,

        /// <summary>
        /// ช่องทางการติดต่อผ่านทางFacebook
        /// </summary>
        Facebook = 2,

        /// <summary>
        /// ช่องทางการติดต่อผ่านทางLine
        /// </summary>
        Line = 3
    }

    public enum ApproveStatus
    {
        WaitingApprove = 0,
        Approve = 1,
        NotApprove = 2,
        Cancel = 99
    }

    public enum TransferStatus
    {
        Pending = 0,
        Received = 1,
        Reject = 2,
        Draft = 5,
        Cancel = 99
    }

    public enum TransferType
    {
        /// <summary>
        /// (WTB) โอนจากคลังไปยังสาขา
        /// </summary>
        WTB = 1,

        /// <summary>
        /// (BTB) โอนจากสาขาไปยังสาขา
        /// </summary>
        BTB = 2,

        /// <summary>
        /// (WTW) โอนจากคลังไปยังคลัง
        /// </summary>
        WTW = 3,
    }

    public enum UserRole
    {
        Admin = 1,
		Sale = 2,
		Stock = 3,
		AccountingOfficer = 4,
		SaleArea = 5,
        Audit = 6
    }

    public enum AdjustItemType
    {
        Add = 1,
        Delete = 2
    }

    public enum ItemInBranchStatus
    {
        InActive = 0,
        Active = 1
    }

    public enum InventoryReportSearchType
    {
        ByDate = 1,
        ByMonthOfYear = 2
    }

    public enum PaymentType
    {
        /// <summary>
        /// เงินสด
        /// </summary>
        CA = 1,

        /// <summary>
        /// ผ่อน
        /// </summary>
        CL = 2,

        /// <summary>
        /// เครดิตการ์ด
        /// </summary>
        CC = 3,

        /// <summary>
        /// เช็คธนาคาร
        /// </summary>
        CHQ = 4,

        /// <summary>
        /// เงินโอน
        /// </summary>
        TR = 5
    }
}

