using Headspring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArandaSoftLab.Core.Domain.Enumerations
{

    public class ModulesEnumeration : Enumeration<ModulesEnumeration, string>
    {
        public static readonly ModulesEnumeration ArandaSoftLab_Products = new ModulesEnumeration("ArandaSoftLab_Products", "ArandaSoftLab_Products");

        private ModulesEnumeration(string value, string displayName) : base(value, displayName)
        {
          
        }

        public static bool IsValid(string Value)
        {
            return GetAll().Any(rr => rr.Value == Value);
        }
    }
}
