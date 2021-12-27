using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using LazyCache;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Topics.Queries.GetAll
{
    public class GetAllTopicsQuery : IRequest<Result<List<GetAllTopicsResponse>>>
    {
        public GetAllTopicsQuery()
        {
        }
    }

    internal class GetAllTopicsCachedQueryHandler : IRequestHandler<GetAllTopicsQuery, Result<List<GetAllTopicsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllTopicsCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllTopicsResponse>>> Handle(GetAllTopicsQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Topic>>> getAllTopics = () => _unitOfWork.Repository<Topic>().GetAllAsync();
            var TopicList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllTopicsCacheKey, getAllTopics);
            var mappedTopics = _mapper.Map<List<GetAllTopicsResponse>>(TopicList);
            return await Result<List<GetAllTopicsResponse>>.SuccessAsync(mappedTopics);
        }
    }
}