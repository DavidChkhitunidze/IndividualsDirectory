using IndividualsDirectory.Entities;
using IndividualsDirectory.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndividualsDirectory.Repositories.Abstracts
{
    public interface IEntityRepository
    {
        Task<Response<IQueryable<TEntity>>> GetEntitiesAsync<TEntity>(params string[] includeParams) where TEntity : BaseEntity;
        Task<Response<IQueryable<TEntity>>> GetEntitiesAsync<TEntity>(FormattableString sql, params string[] includeParams) where TEntity : BaseEntity;
        Task<Response<TEntity>> GetEntityAsync<TEntity>(int id, params string[] includeParams) where TEntity : BaseEntity;
        Task<Response<TEntity>> CreateEntityAsync<TEntity>(TEntity entity) where TEntity : BaseEntity;
        Task<Response<TEntity>> UpdateEntityAsync<TEntity>(TEntity entity, params byte[] rowVersion) where TEntity : BaseEntity;
        Task<Response> DeleteEntityAsync<TEntity>(TEntity entity) where TEntity : BaseEntity;
    }
}
