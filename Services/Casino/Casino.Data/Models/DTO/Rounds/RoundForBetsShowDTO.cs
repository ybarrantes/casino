using Casino.Data.Models.DTO.Roulettes;
using Casino.Services.DB.SQL.Contracts.Model;
using System;

namespace Casino.Data.Models.DTO.Rounds
{
    public class RoundForBetsShowDTO : IModelDTO
    {
        public long Id { get; set; }

        public RouletteBetsShowDTO Roulette { get; set; }

        public RoundStateDTO State { get; set; }

        public string WinNumber { get; set; }

        public DateTime? ClosedAt { get; set; }

        public DateTime? CreatedAt { get; set; }

    }
}
