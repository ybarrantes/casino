

using Casino.Data.Models.Entities;
using Casino.Services.DB.SQL.Contracts.Model;
using System;

namespace Casino.Data.Models.DTO.Rounds
{
    public class RoundShowDTO : IModelDTO
    {
        public long Id { get; set; }

        public RoundStateDTO State { get; set; }

        public string WinNumber { get; set; }

        public DateTime? ClosedAt { get; set; }

        public DateTime? CreatedAt { get; set; }

    }
}
