using System.Collections.Generic;

namespace FCT.LLC.BusinessService.DataAccess
{
    public interface IEntityMapper<TEntity, TData>
    {
        TData MapToData(TEntity tEntity, object parameters=null);

        TEntity MapToEntity(TData tData);

    }
}
