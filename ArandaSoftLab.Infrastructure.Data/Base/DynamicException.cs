using Core.UseCase.Base;
using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Runtime.Serialization;
using System.Text;

namespace ArandaSoftLab.Infrastructure.Data.Base
{
    internal static class DynamicException
    {
        public static string Formatted(DbEntityValidationException innerException)
        {

            if (innerException != null)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var eve in innerException.EntityValidationErrors)
                {
                    sb.AppendLine(string.Format("- Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().FullName, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        sb.AppendLine(string.Format("-- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                            ve.PropertyName,
                            eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                            ve.ErrorMessage));
                    }
                }

                return sb.ToString();
            }
            return "";
        }
        public static string Formatted(DbUpdateException innerException)
        {
            var builder = new StringBuilder("A DbUpdateException was caught while saving changes. ");
            try
            {
                foreach (var result in innerException.Entries)
                {
                    builder.AppendFormat("Type: {0} was part of the problem. ", result.Entity.GetType().Name);
                }
            }
            catch (Exception e)
            {
                builder.Append("Error parsing DbUpdateException: " + e.ToString());
            }
            builder.Append(ObtenerExcepcion(innerException.InnerException));
            return builder.ToString();
        }
        private static string ObtenerExcepcion(Exception InnerException)
        {
            if (InnerException.InnerException != null)
            {
                return ObtenerExcepcion(InnerException.InnerException);
            }
            else
            {
                return InnerException.Message;
            }
        }
    }
    [Serializable]
    public class DynamicFormattedException : Exception, IFormattedValidationException
    {
        protected DynamicFormattedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public DynamicFormattedException() : base() { }
        public DynamicFormattedException(string message) : base(message) { }
        public DynamicFormattedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
