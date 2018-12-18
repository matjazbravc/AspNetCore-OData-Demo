using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EmployeesWebApiOData.DbContexts;
using EmployeesWebApiOData.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmployeesWebApiOData.Services.Repositories.Common
{
	public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IBaseEntity
	{
		private readonly ApplicationDbContext _context;
		private readonly ILogger _logger;

		public Repository(ApplicationDbContext context, ILoggerFactory loggerFactory)
		{
			_context = context;
			_logger = loggerFactory.CreateLogger<Repository<TEntity>>();
		}

		public IEnumerable<TEntity> Find(params Expression<Func<TEntity, dynamic>>[] includePaths)
		{
			var query = _context.Set<TEntity>().AsNoTracking();
			query = includePaths.Aggregate(query, (current, path) => current.Include(path));
			return query.ToHashSet();
		}

		public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, dynamic>>[] includePaths)
		{
			var query = _context.Set<TEntity>().AsNoTracking().Where(predicate);
			query = includePaths.Aggregate(query, (current, path) => current.Include(path));
			return query.ToHashSet();
		}

		public async Task<IEnumerable<TEntity>> FindAsync(params Expression<Func<TEntity, dynamic>>[] includePaths)
		{
			var query = _context.Set<TEntity>().AsNoTracking();
			query = includePaths.Aggregate(query, (current, path) => current.Include(path));
			return await query.ToListAsync();
		}

		public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, dynamic>>[] includePaths)
		{
			var query = _context.Set<TEntity>().AsNoTracking().Where(predicate);
			query = includePaths.Aggregate(query, (current, path) => current.Include(path));
			return await query.ToListAsync();
		}

		public TEntity FindOne(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, dynamic>>[] includePaths)
		{
			var query = _context.Set<TEntity>().AsNoTracking().Where(predicate);
			query = includePaths.Aggregate(query, (current, path) => current.Include(path));
			return query.FirstOrDefault();
		}

		public async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, dynamic>>[] includePaths)
		{
			var query = _context.Set<TEntity>().AsNoTracking().Where(predicate);
			query = includePaths.Aggregate(query, (current, path) => current.Include(path));
			return await query.FirstOrDefaultAsync();
		}

		public int Count()
		{
			return _context.Set<TEntity>().AsNoTracking().Count();
		}

		public int Count(Expression<Func<TEntity, bool>> predicate)
		{
			return _context.Set<TEntity>().AsNoTracking().Count(predicate);
		}

		public async Task<int> CountAsync()
		{
			return await _context.Set<TEntity>().AsNoTracking().CountAsync();
		}

		public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
		{
			return await _context.Set<TEntity>().AsNoTracking().CountAsync(predicate);
		}

		public int DeleteAll()
		{
			var set = _context.Set<TEntity>().AsNoTracking();
			var entities = set.ToHashSet();
			return Delete(entities);
		}

		public int Delete(Expression<Func<TEntity, bool>> predicate)
		{
			var set = _context.Set<TEntity>();
			var entities = set.Where(predicate).ToHashSet();
			return Delete(entities);
		}

		public int Delete(TEntity entity)
		{
			var set = _context.Set<TEntity>();
			if (_context.Entry(entity).State == EntityState.Detached)
			{
				var localEntity = set.Local.FirstOrDefault(x => x.Id == entity.Id);
				if (localEntity != null)
				{
					_context.Entry(localEntity).State = EntityState.Deleted;
				}
				else
				{
					set.Attach(entity);
					_context.Entry(entity).State = EntityState.Deleted;
				}
			}
			else
			{
				set.Remove(entity);
			}
			return _context.SaveChanges();
		}

		public int Delete(IEnumerable<TEntity> entities)
		{
			var set = _context.Set<TEntity>();
			foreach (var entity in entities)
			{
				if (_context.Entry(entity).State == EntityState.Detached)
				{
					var localEntity = set.Local.FirstOrDefault(x => x.Id == entity.Id);
					if (localEntity != null)
					{
						_context.Entry(localEntity).State = EntityState.Deleted;
					}
					else
					{
						set.Attach(entity);
						_context.Entry(entity).State = EntityState.Deleted;
					}
				}
				else
				{
					set.Remove(entity);
				}
			}
			return _context.SaveChanges();
		}

		public async Task<int> DeleteAllAsync()
		{
			var set = _context.Set<TEntity>().AsNoTracking();
			var entities = set.ToHashSet();
			return await DeleteAsync(entities);
		}

		public async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
		{
			var set = _context.Set<TEntity>().AsNoTracking();
			var entities = set.Where(predicate).ToHashSet();
			return await DeleteAsync(entities);
		}

		public async Task<int> DeleteAsync(TEntity entity)
		{
			var set = _context.Set<TEntity>();
			if (_context.Entry(entity).State == EntityState.Detached)
			{
				var localEntity = set.Local.FirstOrDefault(x => x.Id == entity.Id);

				if (localEntity != null)
				{
					_context.Entry(localEntity).State = EntityState.Deleted;
				}
				else
				{
					set.Attach(entity);
					_context.Entry(entity).State = EntityState.Deleted;
				}
			}
			else
			{
				set.Remove(entity);
			}
			return await _context.SaveChangesAsync();
		}

		public async Task<int> DeleteAsync(IEnumerable<TEntity> entities)
		{
			var set = _context.Set<TEntity>();
			foreach (var entity in entities)
			{
				if (_context.Entry(entity).State == EntityState.Detached)
				{
					var localEntity = set.Local.FirstOrDefault(x => x.Id == entity.Id);
					if (localEntity != null)
					{
						_context.Entry(localEntity).State = EntityState.Deleted;
					}
					else
					{
						set.Attach(entity);
						_context.Entry(entity).State = EntityState.Deleted;
					}
				}
				else
				{
					set.Remove(entity);
				}
			}
			return await _context.SaveChangesAsync();
		}

		public int Insert(TEntity entity)
		{
			_context.Set<TEntity>().Add(entity);
			return _context.SaveChanges();
		}

		public int Insert(IEnumerable<TEntity> entities)
		{
			foreach (var entity in entities)
			{
				_context.Set<TEntity>().Add(entity);
			}
			return _context.SaveChanges();
		}

		public async Task<int> InsertAsync(TEntity entity)
		{
			_context.Set<TEntity>().Add(entity);
			return await _context.SaveChangesAsync();
		}

		public async Task<int> InsertAsync(IEnumerable<TEntity> entities)
		{
			foreach (var entity in entities)
			{
				_context.Set<TEntity>().Add(entity);
			}
			return await _context.SaveChangesAsync();
		}

		public int Update(TEntity entity)
		{
			try
			{
				if (entity == null)
				{
					throw new ArgumentNullException(nameof(entity));
				}
				var set = _context.Set<TEntity>();
				if (_context.Entry(entity).State == EntityState.Detached)
				{
					var localEntity = set.Local.FirstOrDefault(x => x.Id == entity.Id);
					if (localEntity != null)
					{
						_context.Entry(localEntity).CurrentValues.SetValues(entity);
						_context.Entry(localEntity).State = EntityState.Modified;
					}
					else
					{
						entity = set.Attach(entity).Entity;
						_context.Entry(entity).State = EntityState.Modified;
					}
				}
				else
				{
					_context.Entry(entity).State = EntityState.Modified;
				}
				return _context.SaveChanges();
			}
			catch (Exception x)
			{
				string message = x.GetBaseException().Message;
				_logger.LogError(new EventId(), x, message);
				throw new ApplicationException(message);
			}
		}

		public int Update(IEnumerable<TEntity> entities)
		{
			try
			{
				if (entities == null)
				{
					throw new ArgumentNullException(nameof(entities));
				}
				var set = _context.Set<TEntity>();
				foreach (var entity in entities)
				{
					if (_context.Entry(entity).State == EntityState.Detached)
					{
						var localEntity = set.Local.FirstOrDefault(x => x.Id == entity.Id);

						if (localEntity != null)
						{
							_context.Entry(localEntity).CurrentValues.SetValues(entity);
						}
						else
						{
							set.Attach(entity);
							_context.Entry(entity).State = EntityState.Modified;
						}
					}
					else
					{
						_context.Entry(entity).State = EntityState.Modified;
					}
				}
				return _context.SaveChanges();
			}
			catch (Exception x)
			{
				string message = x.GetBaseException().Message;
				_logger.LogError(new EventId(), x, message);
				throw new ApplicationException(message);
			}
		}

		public async Task<int> UpdateAsync(TEntity entity)
		{
			try
			{
				if (entity == null)
				{
					throw new ArgumentNullException(nameof(entity));
				}
				var set = _context.Set<TEntity>();
				if (_context.Entry(entity).State == EntityState.Detached)
				{
					var localEntity = set.Local.FirstOrDefault(x => x.Id == entity.Id);

					if (localEntity != null)
					{
						_context.Entry(localEntity).CurrentValues.SetValues(entity);
						_context.Entry(localEntity).State = EntityState.Modified;
					}
					else
					{
						entity = set.Attach(entity).Entity;
						_context.Entry(entity).State = EntityState.Modified;
					}
				}
				else
				{
					_context.Entry(entity).State = EntityState.Modified;
				}
				return await _context.SaveChangesAsync();
			}
			catch (Exception x)
			{
				string message = x.GetBaseException().Message;
				_logger.LogError(new EventId(), x, message);
				throw new ApplicationException(message);
			}
		}

		public async Task<int> UpdateAsync(IEnumerable<TEntity> entities)
		{
			try
			{
				if (entities == null)
				{
					throw new ArgumentNullException(nameof(entities));
				}

				var set = _context.Set<TEntity>();

				foreach (var entity in entities)
				{
					if (_context.Entry(entity).State == EntityState.Detached)
					{
						var localEntity = set.Local.FirstOrDefault(x => x.Id == entity.Id);

						if (localEntity != null)
						{
							_context.Entry(localEntity).CurrentValues.SetValues(entity);
						}
						else
						{
							set.Attach(entity);
							_context.Entry(entity).State = EntityState.Modified;
						}
					}
					else
					{
						_context.Entry(entity).State = EntityState.Modified;
					}
				}
				return await _context.SaveChangesAsync();
			}
			catch (Exception x)
			{
				string message = x.GetBaseException().Message;
				_logger.LogError(new EventId(), x, message);
				throw new ApplicationException(message);
			}
		}
	}
}