using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Infrastructure.Repositories
{
    public class StatementRepository : IStatementRepository
    {
        private readonly IRepositoryAsync<Statement, int> _repository;

        public StatementRepository(IRepositoryAsync<Statement, int> repository)
        {
            _repository = repository;
        }

        public async Task<bool> IsInAgenda(int topicId)
        {
            return await _repository.Entities.AnyAsync(b => b.TopicId == topicId);
        }
    }
}