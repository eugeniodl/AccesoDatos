public interface IRepository<T>
{
    IEnumerable<T> GetAll();
    T GetValue(int id);
    void Insert(T entity);
    void Update(T entity);
    void Delete(int id);
}

