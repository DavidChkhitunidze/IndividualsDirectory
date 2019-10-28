using IndividualsDirectory.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IndividualsDirectory.Models.ResourceControls.Extensions
{
    public static class ResourceControlsExtensions
    {
        public static PagedList<TViewModel> GetFilteredData<TEntity, TViewModel>
            (this IQueryable<TEntity> entities, ResourceParams<TViewModel> resourceParams, Expression<Func<TEntity, TViewModel>> projection)
            where TEntity : BaseEntity
            where TViewModel : class
        {
            var pageNumber = resourceParams.Page;
            var pageSize = resourceParams.Size;
            var totalItems = entities.Count();

            var orderProperty = TypeDescriptor.GetProperties(typeof(TEntity)).Find(resourceParams.OrderBy, true);
            if (orderProperty == null)
            {
                resourceParams.OrderBy = nameof(BaseEntity.Id).ToLower();
                orderProperty = TypeDescriptor.GetProperties(typeof(TEntity)).Find(resourceParams.OrderBy, true);
            }

            if (resourceParams.Sort.ToLower() == "desc")
                entities = entities.OrderByDescending(e => orderProperty.GetValue(e));
            else
                entities = entities.OrderBy(e => orderProperty.GetValue(e));

            var items = entities.Skip(resourceParams.Size * (resourceParams.Page - 1)).Take(resourceParams.Size).Select(projection);

            return new PagedList<TViewModel>(totalItems, pageNumber, pageSize, items);
        }
    }
}
