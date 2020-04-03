

using Casino.Data.Models.Entities;
using Casino.Services.DB.SQL.Contracts.Model;
using System;

namespace Casino.Data.Models.DTO.Rounds
{
    public class RoundShowDTO : IModelDTO
    {
        public long Id { get; set; }

        public RoundState State { get; set; }

        public string WinNumber { get; set; }

        public DateTime? CloseAt { get; set; }

        public DateTime? CreatedAt { get; set; }

    }
}
