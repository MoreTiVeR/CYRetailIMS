using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models;

public static class EnumModel
{
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
}

