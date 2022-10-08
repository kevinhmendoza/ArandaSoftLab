using Headspring;
using System.Linq;

namespace ArandaSoftLab.Core.Domain.Enumerations
{
    public class EstadoEnumeration : Enumeration<EstadoEnumeration, string>
    {
        public static readonly EstadoEnumeration Activo = new EstadoEnumeration("AC", "Activo");
        public static readonly EstadoEnumeration Inactivo = new EstadoEnumeration("IN", "Inactivo");

        private EstadoEnumeration(string value, string displayName) : base(value, displayName)
        {

        }

        public static bool IsValid(string Value)
        {
            return GetAll().Any(rr => rr.Value == Value);
        }
    }
}
