using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TMRoleInMenus;
public class TMRoleInMenuUpdateEvent : BaseEvent
{
    public TMRoleInMenu Item { get; set; }
    public TMRoleInMenuUpdateEvent(TMRoleInMenu tmRoleInMenu)
    {
        Item = tmRoleInMenu;
    }
}