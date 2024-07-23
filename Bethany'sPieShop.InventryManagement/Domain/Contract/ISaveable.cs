using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bethany_sPieShop.InventoryManagement.Domain.Contract
{
    public interface ISaveable
    {
         string ConvertToStringForSaving();
    }
}
