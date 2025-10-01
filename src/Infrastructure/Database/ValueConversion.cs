using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CYRetailIMS.Infrastructure.Database;
internal static class ValueConversion
{
    public static class EfCoreConverters
    {
        public static readonly ValueConverter<DateOnly, DateTime> DateOnlyConverter =
            new ValueConverter<DateOnly, DateTime>(
                v => v.ToDateTime(TimeOnly.MinValue),   // เก็บเป็น DateTime (เวลา 00:00:00)
                v => DateOnly.FromDateTime(v));         // แปลงกลับมาเป็น DateOnly

        public static readonly ValueConverter<TimeOnly, TimeSpan> TimeOnlyConverter =
            new ValueConverter<TimeOnly, TimeSpan>(
                v => v.ToTimeSpan(),                    // เก็บเป็น TimeSpan
                v => TimeOnly.FromTimeSpan(v));         // แปลงกลับมาเป็น TimeOnly
    }
}
