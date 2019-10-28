using IndividualsDirectory.Entities;
using IndividualsDirectory.Entities.Context;
using IndividualsDirectory.Models.Response;
using IndividualsDirectory.Repositories.Abstracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndividualsDirectory.Repositories
{
    public class EntityRepository : IEntityRepository
    {
        #region PrivateFields

        private readonly IndividualsDirectoryDbContext _context;

        #endregion

        #region Constructor

        public EntityRepository(IndividualsDirectoryDbContext context)
        {
            _context = context;
        }

        #endregion

        #region DbMethods

        public async Task<Response<IQueryable<TEntity>>> GetEntitiesAsync<TEntity>(params string[] includeParams) where TEntity : BaseEntity
        {
            var response = new Response<IQueryable<TEntity>>();

            var entities = _context.Set<TEntity>().AsQueryable();
            foreach (var includeParam in includeParams)
                entities = entities.Include(includeParam);

            if (entities == null || !await entities.AnyAsync())
            {
                response.SetErrorMessages("No records found.");
                return response;
            }

            response.SetSuccess();
            response.SetModel(entities);

            return response;
        }

        public async Task<Response<IQueryable<TEntity>>> GetEntitiesAsync<TEntity>(FormattableString sql, params string[] includeParams) where TEntity : BaseEntity
        {
            var response = new Response<IQueryable<TEntity>>();

            var entities = _context.Set<TEntity>().FromSql(sql).AsQueryable();
            foreach (var includeParam in includeParams)
                entities = entities.Include(includeParam);

            if (entities == null || !await entities.AnyAsync())
            {
                response.SetErrorMessages("No records found.");
                return response;
            }

            response.SetSuccess();
            response.SetModel(entities);

            return response;
        }

        public async Task<Response<TEntity>> GetEntityAsync<TEntity>(int id, params string[] includeParams) where TEntity : BaseEntity
        {
            var response = new Response<TEntity>();

            var context = _context.Set<TEntity>().AsQueryable();
            foreach (var includeParam in includeParams)
                context = context.Include(includeParam);

            var entity = await context.FirstOrDefaultAsync(i => i.Id.Equals(id));
            if (entity == null)
            {
                response.SetErrorMessages("No record found.");
                return response;
            }

            response.SetSuccess();
            response.SetModel(entity);

            return response;
        }

        public async Task<Response<TEntity>> CreateEntityAsync<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            var response = new Response<TEntity>();

            await _context.Set<TEntity>().AddAsync(entity);
            if (await _context.SaveChangesAsync() <= 0)
            {
                response.SetErrorMessages("Creating object failed.");
                return response;
            }

            response.SetSuccess("Object has been created successfully.");
            response.SetModel(entity);

            return response;
        }

        public async Task<Response<TEntity>> UpdateEntityAsync<TEntity>(TEntity entity, params byte[] rowVersion) where TEntity : BaseEntity
        {
            var response = new Response<TEntity>();

            try
            {
                if (rowVersion.Any())
                    _context.Entry(entity).OriginalValues["RowVersion"] = rowVersion;

                var updateResult = _context.Set<TEntity>().Update(entity);
                if (await _context.SaveChangesAsync() <= 0)
                {
                    response.SetErrorMessages("Updating object failed.");
                    return response;
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                response.SetErrorMessages("The object has already been modified by another user. " +
                    "Please refresh the page to see modifications.");
                return response;
            }

            response.SetSuccess("Object has been updated successfully.");
            response.SetModel(entity);

            return response;
        }

        public async Task<Response> DeleteEntityAsync<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            var response = new Response();

            try
            {
                var deleteResult = _context.Set<TEntity>().Remove(entity);
                if (await _context.SaveChangesAsync() <= 0)
                {
                    response.SetErrorMessages("Deleting object failed.");
                    return response;
                }
            }
            catch (DbUpdateException)
            {
                response.SetErrorMessages("The object has already been deleted by another user.");
                return response;
            }

            response.SetSuccess("Object has been deleted successfully.");

            return response;
        }

        #endregion
    }
}
