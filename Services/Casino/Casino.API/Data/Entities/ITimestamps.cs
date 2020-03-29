using System;

namespace Casino.API.Data.Entities
{
    interface ITimestamps
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
