using Casino.Services.DB.SQL.Contracts.Model;

namespace Casino.Data.Models.DTO.Roulettes
{
    public class RouletteRulesShowDTO : IModelDTO
    {
        public long Id { get; set; }

        public RouletteRuleTypeShowDTO Type { get; set; }

        public float Pay { get; set; }
    }
}
