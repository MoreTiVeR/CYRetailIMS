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
        Staff = 2,
        Manager = 3,
        AccountingOfficer = 4,
    }

    public enum AdjustItemType
    {
        Add = 1,
        Delete = 2
    }
}

