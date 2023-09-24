using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TMRoleInMenus;
public class TMRoleInMenuDeleteEvent : BaseEvent
{
    public TMRoleInMenu Item { get; set; }
    public TMRoleInMenuDeleteEvent(TMRoleInMenu tmRoleInMenu)
    {
        Item = tmRoleInMenu;
    }
}