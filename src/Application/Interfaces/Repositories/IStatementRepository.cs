using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Interfaces.Repositories
{
    public interface IStatementRepository
    {
        Task<bool> IsInAgenda(int topicId);
    }
}