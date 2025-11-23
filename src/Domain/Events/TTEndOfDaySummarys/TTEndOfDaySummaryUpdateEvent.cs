using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTEndOfDaySummarys;

public class TTEndOfDaySummaryUpdateEvent : BaseEvent
{
    public TTEndOfDaySummary Item { get; set; }
    public TTEndOfDaySummaryUpdateEvent(TTEndOfDaySummary item)
    {
        Item = item;
    }
}