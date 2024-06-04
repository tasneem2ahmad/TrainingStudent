namespace Training.BLL.Interfaces
{
    public interface IGenericRepository<T>
    {
       Task<T> Get(int? id);
       Task< IEnumerable<T>> GetAll();
        Task<int> Add(T department);
        Task<int> Update(T department);
        Task<int> Delete(T department);
    }
}
