using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace BlazorHero.CleanArchitecture.Application.Features.Statements.Commands.Delete
{
    public class DeleteStatementCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteStatementCommandHandler : IRequestHandler<DeleteStatementCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteStatementCommandHandler> _localizer;

        public DeleteStatementCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteStatementCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteStatementCommand command, CancellationToken cancellationToken)
        {
            var statement = await _unitOfWork.Repository<Statement>().GetByIdAsync(command.Id);
            if (statement != null)
            {
                await _unitOfWork.Repository<Statement>().DeleteAsync(statement);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(statement.Id, _localizer["Statement Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Statement Not Found!"]);
            }
        }
    }
}