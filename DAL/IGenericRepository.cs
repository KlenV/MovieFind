namespace DAL
{ 
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int Id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);

    }
}
