using System;

namespace Casino.API.Data.Extension
{
    /// <summary>
    /// Agrega la capacidad de 'borrado suave' de una entidad
    /// </summary>
    interface ISoftDeletesEntityModel
    {
        public DateTime? DeletedAt { get; set; }
    }
}
