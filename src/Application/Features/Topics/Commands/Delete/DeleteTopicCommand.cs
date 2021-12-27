using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;

namespace BlazorHero.CleanArchitecture.Application.Features.Topics.Commands.Delete
{
    public class DeleteTopicCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteTopicCommandHandler : IRequestHandler<DeleteTopicCommand, Result<int>>
    {
        private readonly IStatementRepository _statementRepository;
        private readonly IStringLocalizer<DeleteTopicCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteTopicCommandHandler(IUnitOfWork<int> unitOfWork, IStatementRepository statementRepository, IStringLocalizer<DeleteTopicCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _statementRepository = statementRepository;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteTopicCommand command, CancellationToken cancellationToken)
        {
            var isTopicUsed = await _statementRepository.IsInAgenda(command.Id);
            if (!isTopicUsed)
            {
                var topic = await _unitOfWork.Repository<Topic>().GetByIdAsync(command.Id);
                if (topic != null)
                {
                    await _unitOfWork.Repository<Topic>().DeleteAsync(topic);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllTopicsCacheKey);
                    return await Result<int>.SuccessAsync(topic.Id, _localizer["Topic Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Topic Not Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Deletion Not Allowed"]);
            }
        }
    }
}