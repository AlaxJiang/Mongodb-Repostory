using AdeSAAS.Models;
using AdeSAAS.Models.PagedList;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AdeSAAS.DataService
{
    public interface IRepository<T, TKey> : IQueryable<T>, IDisposable
         where T : IEntityModel<TKey>
    {
        #region Methods
        IMongoCollection<T> MongoCollection();
        IConnect MongoConnect();
        #endregion Methods

        #region CRUD
        Task<T> AddAsync(T Model);
        //Task<T> AddAsync(T Model, System.Threading.CancellationToken CancellationToken);
        Task<IEnumerable<T>> AddAsync(IEnumerable<T> Models);
        Task<IEnumerable<T>> AddAsync(IEnumerable<T> Models, InsertManyOptions Options);
        Task<IEnumerable<T>> AddAsync(IEnumerable<T> Models, InsertManyOptions Options, System.Threading.CancellationToken CancellationToken);

        Task<ReplaceOneResult> EditAsync(Expression<Func<T, bool>> Where, T Model);
        Task<ReplaceOneResult> EditAsync(FilterDefinition<T> Query, T Model);
        Task<ReplaceOneResult> EditAsync(Expression<Func<T, bool>> Where, T Model, UpdateOptions Options);
        Task<ReplaceOneResult> EditAsync(Expression<Func<T, bool>> Where, T Model, UpdateOptions Options, System.Threading.CancellationToken CancellationToken);

        Task<UpdateResult> UpdateAsync(Expression<Func<T, bool>> Where, UpdateDefinition<T> Update);
        Task<UpdateResult> UpdateAsync(Expression<Func<T, bool>> Where, UpdateDefinition<T> Update, UpdateOptions Options);
        Task<UpdateResult> UpdateAsync(Expression<Func<T, bool>> Where, UpdateDefinition<T> Update, UpdateOptions Options, System.Threading.CancellationToken CancellationToken);
        Task<UpdateResult> UpdateAsync(FilterDefinition<T> Query, UpdateDefinition<T> Update);

        Task<DeleteResult> DeleteAsync(Expression<Func<T, bool>> Where);
        Task<DeleteResult> DeleteAsync(Expression<Func<T, bool>> Query, System.Threading.CancellationToken CancellationToken);
        Task<DeleteResult> DeleteAsync(FilterDefinition<T> Query);
        Task<DeleteResult> DeleteAsync(FilterDefinition<T> Query, System.Threading.CancellationToken CancellationToken);

        #endregion CRUD

        #region Find
        Task<T> FindAsync(ObjectId Id);
        Task<T> FindAsync<TTKey>(TTKey Id, string Name = "_id");
        Task<T> FindAsync(FilterDefinition<T> Query);
        Task<T> FindAsync(Expression<Func<T, bool>> Query);
        Task<IAsyncCursor<T>> FindAsync(FilterDefinition<T> Query, FindOptions<T, T> Options = null, System.Threading.CancellationToken CancellationToken = default(System.Threading.CancellationToken));
        Task<IAsyncCursor<T>> FindAsync(Expression<Func<T, bool>> Query, FindOptions<T, T> Options = null, System.Threading.CancellationToken CancellationToken = default(System.Threading.CancellationToken));

        T Find(Expression<Func<T, bool>> Where);

        #endregion Find

        #region All
        Task<IEnumerable<T>> AllAsync();
        Task<IEnumerable<T>> AllAsync(SortDefinition<T> SortBy);
        Task<IEnumerable<T>> AllAsync(FilterDefinition<T> Query, SortDefinition<T> SortBy = null);
        Task<IEnumerable<T>> AllAsync(Expression<Func<T, bool>> Query, SortDefinition<T> SortBy = null);
        Task<IEnumerable<T>> AllAsync<TTKey>(Expression<Func<T, bool>> Query, Expression<Func<T, TTKey>> OrderBy);
        Task<IEnumerable<T>> AllAsync<TTKey>(int Page, int Total, Expression<Func<T, bool>> Query, Expression<Func<T, TTKey>> OrderBy);

        IList<T> All();
        IList<T> All<TTKey>(Expression<Func<T, TTKey>> OrderBy);
        IList<T> All<TTKey>(Expression<Func<T, TTKey>> OrderBy, Expression<Func<T, bool>> Where);
        IList<T> All<TTKey>(int Page, int Total, Expression<Func<T, TTKey>> OrderBy, Expression<Func<T, bool>> Where);

        #endregion All

        #region Count
        Task<long> CountAsync();
        Task<long> CountAsync(FilterDefinition<T> Query, CountOptions Options = null, System.Threading.CancellationToken CancellationToken = default(System.Threading.CancellationToken));
        Task<long> CountAsync(Expression<Func<T, bool>> Query, CountOptions Options = null, System.Threading.CancellationToken CancellationToken = default(System.Threading.CancellationToken));

        long Count();
        long Count(Expression<Func<T, bool>> Query);

        #endregion Count

        #region Create
        T Create();
        ObjectId CreateObjectId(string Value);

        #endregion Create

        #region StaticPagedList
        Task<StaticPagedList<T>> PaginationAsync<TTKey>(int pageIndex, int pageSize, Expression<Func<T, TTKey>> OrderBy, bool isOrderByAsc = true, Expression<Func<T, bool>> Where = null);
        Task<StaticPagedList<T>> PaginationAsync(int pageIndex, int pageSize, SortDefinition<T> SortBy, Expression<Func<T, bool>> Where = null);
        Task<StaticPagedList<T>> PaginationAsync(int pageIndex, int pageSize, SortDefinition<T> SortBy, FilterDefinition<T> Query);

        StaticPagedList<T> Pagination<TTKey>(int pageIndex, int pageSize, Expression<Func<T, TTKey>> OrderBy, bool isOrderByAsc = true, Expression<Func<T, bool>> Where = null);

        #endregion StaticPagedList

        #region AsQueryable
        IMongoQueryable<T> Query();
        IMongoQueryable<T> Query(params Expression<Func<T, bool>>[] Where);
        IMongoQueryable<T> Query<TTKey>(Expression<Func<T, TTKey>> OrderBy, params Expression<Func<T, bool>>[] Where);
        #endregion AsQueryable
    }

    public interface IRepository<T> : IRepository<T, string>
       where T : IEntityModel<string>
    { }
}
