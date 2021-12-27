using System.ComponentModel.DataAnnotations;
using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Application.Requests;
using BlazorHero.CleanArchitecture.Domain.Entities;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BlazorHero.CleanArchitecture.Application.Features.Statements.Commands.AddEdit
{
    public partial class AddEditStatementCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Body { get; set; }
        [Required]
        public int TopicId { get; set; }
    }

    internal class AddEditStatementCommandHandler : IRequestHandler<AddEditStatementCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditStatementCommandHandler> _localizer;

        public AddEditStatementCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IStringLocalizer<AddEditStatementCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditStatementCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var statement = _mapper.Map<Statement>(command);
                await _unitOfWork.Repository<Statement>().AddAsync(statement);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(statement.Id, _localizer["Statement Saved"]);
            }
            else
            {
                var statement = await _unitOfWork.Repository<Statement>().GetByIdAsync(command.Id);
                if (statement != null)
                {
                    statement.Body = command.Body ?? statement.Body;
                    statement.TopicId = (command.TopicId == 0) ? statement.TopicId : command.TopicId;
                    await _unitOfWork.Repository<Statement>().UpdateAsync(statement);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(statement.Id, _localizer["Statement Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Statement Not Found!"]);
                }
            }
        }
    }
}