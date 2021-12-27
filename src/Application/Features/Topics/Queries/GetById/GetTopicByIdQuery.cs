using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Topics.Queries.GetById
{
    public class GetTopicByIdQuery : IRequest<Result<GetTopicByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetProductByIdQueryHandler : IRequestHandler<GetTopicByIdQuery, Result<GetTopicByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetTopicByIdResponse>> Handle(GetTopicByIdQuery query, CancellationToken cancellationToken)
        {
            var Topic = await _unitOfWork.Repository<Topic>().GetByIdAsync(query.Id);
            var mappedTopic = _mapper.Map<GetTopicByIdResponse>(Topic);
            return await Result<GetTopicByIdResponse>.SuccessAsync(mappedTopic);
        }
    }
}