using ArandaSoftLab.Core.Domain.Base;
using ArandaSoftLab.Core.Domain.Enumerations;
using ArandaSoftLab.Infrastructure.Audit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;

namespace ArandaSoftLab.Infrastructure.Data.Base
{
    public interface IDbContext
    {
        DbSet<T> Set<T>() where T : class;
        Action<string> Log { get; set; }
        DbEntityEntry Entry(object entity);
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        int SaveChanges(string hostName,string ipAddress,string userName, DateTime now, string modulo, string interactor);
    }

    public class DbContextBase : DbContext, IDbContext
    {
        protected string _UserName { get; set; }

        protected DateTime _Now { get; set; }
        protected string _Modulo { get; set; }
        protected string _Interactor { get; set; }
        protected string _IpAddress { get; set; }
        protected string _HostName { get; set; }
        private List<AuditLog> auditoriaAgregar { get; set; }
        public List<DbEntityEntry> entidadesAgregar { get; set; }

        public DbContextBase(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        public Action<string> Log
        {
            get
            {
                return this.Database.Log;
            }
            set
            {
                this.Database.Log = value;
            }

        }

        //public virtual DbSet<AuditLog> AuditLog { get; set; }

        public int SaveChanges(string hostName,string ipAddress,string userName, DateTime now, string modulo, string interactor)
        {
            _UserName = userName;
            _Now = now;
            _Modulo = modulo;
            _Interactor = interactor;
            _IpAddress = ipAddress;
            _HostName = hostName;
            AplicarAuditoria();
            AuditLogger();
            int NumeroFilas = base.SaveChanges();
            AplicarAuditoriaNuevosRegistros();
            base.SaveChanges();
            return NumeroFilas;
        }
        private void AplicarAuditoriaNuevosRegistros()
        {
            if (entidadesAgregar.Any())
            {
                for (int i = 0; i < entidadesAgregar.Count; i++)
                {
                    DbEntityEntry dbEntry = entidadesAgregar[i];
                    string keyName = dbEntry.Entity.GetType().GetProperties().Single(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Any()).Name;
                    auditoriaAgregar[i].RecordID = dbEntry.CurrentValues.GetValue<object>(keyName).ToString();  // Again, adjust this if you have a multi-column key
                    //this.AuditLog.Add(auditoriaAgregar[i]);
                }
            }
            new AuditoriaService().GuardarAuditorias(auditoriaAgregar);
        }

        private void AplicarAuditoria()
        {
            entidadesAgregar = new List<DbEntityEntry>();
            auditoriaAgregar = new List<AuditLog>();
            var modifiedEntries = ChangeTracker.Entries()
                            .Where(x => x.Entity is IAuditableEntity
                                && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entry in modifiedEntries)
            {
                IAuditableEntity entity = entry.Entity as IAuditableEntity;
                if (entity != null)
                {
                    string identityName = _UserName;
                    DateTime now = _Now;
                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedBy = identityName;
                        entity.CreatedDate = now;
                        entity.State = EstadoEnumeration.Activo.Value;
                    }
                    else
                    {
                        base.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                        base.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                    }

                    entity.UpdatedBy = identityName;
                    entity.UpdatedDate = now;
                    entity.IPAddress = _IpAddress;
                }
            }
        }

        private void AuditLogger()
        {
            foreach (var ent in this.ChangeTracker.Entries().Where(p => p.State == EntityState.Added || p.State == EntityState.Deleted || p.State == EntityState.Modified))
            {
                string identityName;
                identityName = _UserName;

                var auditorias = GetAuditRecordsForChange(ent, identityName);
                auditoriaAgregar.AddRange(auditorias);
            }
        }

        private List<AuditLog> GetAuditRecordsForChange(DbEntityEntry dbEntry, string userId)
        {
            List<AuditLog> result = new List<AuditLog>();
            DateTime changeTimeUtc = DateTime.UtcNow;
            DateTime changeTime = _Now;

            // Get table name (if it has a Table attribute, use that, otherwise get the pluralized name)
            TableAttribute tableAttr = dbEntry.Entity.GetType().GetTypeInfo().GetCustomAttributes(typeof(TableAttribute), false).SingleOrDefault() as TableAttribute;

            string tableName = tableAttr != null ? tableAttr.Name : ObjectContext.GetObjectType(dbEntry.Entity.GetType()).Name;

            // Get primary key value (If you have more than one key column, this will need to be adjusted)



            string keyName = dbEntry.Entity.GetType().GetProperties().Single(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Any()).Name;

            if (dbEntry.State == EntityState.Added)
            {
                LogAdd(dbEntry, userId, changeTimeUtc, changeTime, tableName, keyName);
            }
            else if (dbEntry.State == EntityState.Deleted)
            {
                LogDelete(dbEntry, userId, result, changeTimeUtc, changeTime, tableName, keyName);
            }
            else if (dbEntry.State == EntityState.Modified)
            {
                LogModified(dbEntry, userId, result, changeTimeUtc, changeTime, tableName, keyName);
            }

            return result;
        }

        private void LogModified(DbEntityEntry dbEntry, string userId, List<AuditLog> result, DateTime changeTimeUtc, DateTime changeTime, string tableName, string keyName)
        {
            foreach (var propertyName in dbEntry.OriginalValues.PropertyNames)
            {
                // For updates, we only want to capture the columns that actually changed
                if (!object.Equals(dbEntry.OriginalValues.GetValue<object>(propertyName), dbEntry.CurrentValues.GetValue<object>(propertyName)))
                {
                    result.Add(
                            new AuditLog()
                            {
                                AuditLogID = Guid.NewGuid(),
                                UserID = userId,
                                EventDateUTC = changeTimeUtc,
                                EventDateLocalTime = changeTime,
                                EventType = AccionEnumeration.Actualizar.Value,    // Modified
                                TableName = tableName,
                                RecordID = dbEntry.OriginalValues.GetValue<object>(keyName).ToString(),
                                ColumnName = propertyName,
                                OriginalValue = dbEntry.OriginalValues.GetValue<object>(propertyName) == null ? null : dbEntry.OriginalValues.GetValue<object>(propertyName).ToString(),
                                NewValue = dbEntry.CurrentValues.GetValue<object>(propertyName) == null ? null : dbEntry.CurrentValues.GetValue<object>(propertyName).ToString(),
                                Module = _Modulo,
                                Interactor = _Interactor,
                                IpAddress = _IpAddress,
                                HostName= _HostName
                            }
                        );
                }
            }
        }

        private void LogDelete(DbEntityEntry dbEntry, string userId, List<AuditLog> result, DateTime changeTimeUtc, DateTime changeTime, string tableName, string keyName)
        {
            // Same with deletes, do the whole record, and use either the description from Describe() or ToString()
            result.Add(new AuditLog()
            {
                AuditLogID = Guid.NewGuid(),
                UserID = userId,
                EventDateUTC = changeTimeUtc,
                EventDateLocalTime = changeTime,
                EventType = AccionEnumeration.Eliminar.Value, // Deleted
                TableName = tableName,
                RecordID = dbEntry.OriginalValues.GetValue<object>(keyName).ToString(),
                ColumnName = "*ALL",
                NewValue = dbEntry.OriginalValues.ToObject().ToString(),
                Module = _Modulo,
                Interactor=_Interactor,
                IpAddress = _IpAddress,
                HostName = _HostName
            }
        );
        }

        private void LogAdd(DbEntityEntry dbEntry, string userId, DateTime changeTimeUtc, DateTime changeTime, string tableName, string keyName)
        {
            // For Inserts, just add the whole record
            // If the entity implements IDescribableEntity, use the description from Describe(), otherwise use ToString()
            //Sobre Escribir
            auditoriaAgregar.Add(new AuditLog()
            {
                AuditLogID = Guid.NewGuid(),
                UserID = userId,
                EventDateUTC = changeTimeUtc,
                EventDateLocalTime = changeTime,
                EventType = AccionEnumeration.Registrar.Value, // Added
                TableName = tableName,
                RecordID = dbEntry.CurrentValues.GetValue<object>(keyName).ToString(),  // Again, adjust this if you have a multi-column key
                ColumnName = "*ALL",    // Or make it nullable, whatever you want
                NewValue = JsonConvert.SerializeObject(dbEntry.CurrentValues.ToObject()),// Convertir a Json
                Module = _Modulo,
                Interactor=_Interactor,
                IpAddress=_IpAddress,
                HostName = _HostName
            });
            entidadesAgregar.Add(dbEntry);
        }

    }
}
