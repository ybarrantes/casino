using Casino.Services.DB.SQL.Contracts.Model;

namespace Casino.Data.Models.DTO.Roulettes
{
    public class RouletteRuleTypeShowDTO : IModelDTO
    {
        public string Name { get; set; }

        public byte Numbers { get; set; }
    }
}
