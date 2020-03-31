using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Casino.API.Data.Extension
{
    public interface ITimestampsEntityModel
    {
        public static string CreatedAtColumnName => "CreatedAt";
        public static string UpdatedAtColumnName => "UpdatedAt";

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Extends soft deletes feature
        /// </summary>
        /// <param name="changeTracker">ChangeTracker from DataContext BeforeSave</param>
        public static void BeforeSave(ChangeTracker changeTracker)
        {
            List<string> fields = new List<string> { CreatedAtColumnName, UpdatedAtColumnName };
            List<EntityState> states = new List<EntityState> { EntityState.Added, EntityState.Modified };

            var entities = changeTracker.Entries()
                .Where(e => states.Contains(e.State) && e.Metadata.GetProperties()
                    .Any(x => fields.Contains(x.Name)))
                .ToList();

            foreach (var entity in entities)
            {
                entity.CurrentValues[CreatedAtColumnName] = DateTime.Now;
                entity.CurrentValues[UpdatedAtColumnName] = DateTime.Now;
            }
        }

    }
}
