﻿using Casino.Services.DB.SQL.Contracts.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Casino.Data.Models.Entities
{
    public class Round : IEntityModelBase, IEntityModelTimestamps, IEntityModelSoftDeletes
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public Roulette Roulette { get; set; }

        [Required]
        public RoundState State { get; set; }

        public Number WinNumber { get; set; }

        [Required]
        public User UserOpen { get; set; }
        
        public User UserClose { get; set; }

        public DateTime? ClosedAt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedAt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        // relations
        public List<Bet> Bets { get; set; }
    }
}
