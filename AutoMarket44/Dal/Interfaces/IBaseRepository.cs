namespace AutoMarket44.Dal.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task Create(T entity);

        IQueryable<T> GetAll();

        Task Delete(long id);

        Task<T> Update(T entity);
    }
}
