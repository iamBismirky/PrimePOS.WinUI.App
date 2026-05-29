namespace PrimePOS.DAL.Repositories
{
    public interface IGenericRepository
    {
        Task<int> CountAsync();
    }
}