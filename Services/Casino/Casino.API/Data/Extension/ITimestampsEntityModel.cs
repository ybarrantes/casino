using System;

namespace Casino.API.Data.Extension
{
    public interface ITimestampsEntityModel
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
