using System.Threading.Tasks;

namespace Payment.Api.Repositories
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}
