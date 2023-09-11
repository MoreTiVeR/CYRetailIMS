namespace CYRetailIMS.ComponentService.Web.Models;

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
}
