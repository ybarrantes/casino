
namespace Casino.Services.DB.SQL.Contracts.Model
{
    public interface IModelDTOFillable<T>
    {
        T FillEntityFromDTO();

        void FillDTOFromEntity(T entity);
    }
}
