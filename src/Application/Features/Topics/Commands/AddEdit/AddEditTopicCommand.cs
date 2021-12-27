using System.ComponentModel.DataAnnotations;
using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;

namespace BlazorHero.CleanArchitecture.Application.Features.Topics.Commands.AddEdit
{
    public partial class AddEditTopicCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }

    internal class AddEditTopicCommandHandler : IRequestHandler<AddEditTopicCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditTopicCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditTopicCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditTopicCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditTopicCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var brand = _mapper.Map<Topic>(command);
                await _unitOfWork.Repository<Topic>().AddAsync(brand);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllTopicsCacheKey);
                return await Result<int>.SuccessAsync(brand.Id, _localizer["Topic Saved"]);
            }
            else
            {
                var brand = await _unitOfWork.Repository<Topic>().GetByIdAsync(command.Id);
                if (brand != null)
                {
                    brand.Name = command.Name ?? brand.Name;
                    await _unitOfWork.Repository<Topic>().UpdateAsync(brand);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllTopicsCacheKey);
                    return await Result<int>.SuccessAsync(brand.Id, _localizer["Topic Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Topic Not Found!"]);
                }
            }
        }
    }
}