using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities;

namespace BlazorHero.CleanArchitecture.Infrastructure.Repositories
{
    public class TopicRepository : ITopicRepository
    {
        private readonly IRepositoryAsync<Topic, int> _repository;

        public TopicRepository(IRepositoryAsync<Topic, int> repository)
        {
            _repository = repository;
        }
    }
}