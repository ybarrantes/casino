using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;

namespace Casino.API.Data.Extension
{
    /// <summary>
    /// Add feature softdeletes
    /// </summary>
    public interface ISoftDeletesEntityModel
    {
        public static string DeletedAtColumnName => "DeletedAt";

        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// Extends soft deletes feature
        /// </summary>
        /// <param name="changeTracker">ChangeTracker from DataContext BeforeSave</param>
        public static void BeforeSave(ChangeTracker changeTracker)
        {
            var entities = changeTracker.Entries()
                .Where(e => e.State.Equals(EntityState.Deleted) && e.Metadata.GetProperties()
                    .Any(x => x.Name.Equals(DeletedAtColumnName)))
                .ToList();

            foreach (var entity in entities)
            {
                if(entity.CurrentValues[DeletedAtColumnName] == null)
                    entity.CurrentValues[DeletedAtColumnName] = DateTime.Now;

                entity.State = EntityState.Modified;
            }
        }
    }
}
