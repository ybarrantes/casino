using Casino.Services.DB.SQL.Contracts.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Casino.Data.Models.Entities
{
    public class UserAccount : IEntityModelBase, IEntityModelTimestamps, IEntityModelSoftDeletes
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public User UserOwner { get; set; }

        [Required]
        public UserAccountState State { get; set; }

        [Required]
        public UserAccountType Type { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedAt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
