using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Application.Requests;
using BlazorHero.CleanArchitecture.Domain.Entities;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Features.Topics.Commands.AddEdit;
using FluentValidation;

namespace BlazorHero.CleanArchitecture.Application.Features.Topics.Commands.Import
{
    public partial class ImportTopicsCommand : IRequest<Result<int>>
    {
        public UploadRequest UploadRequest { get; set; }
    }

    internal class ImportTopicsCommandHandler : IRequestHandler<ImportTopicsCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IExcelService _excelService;
        private readonly IMapper _mapper;
        private readonly IValidator<AddEditTopicCommand> _addTopicValidator;
        private readonly IStringLocalizer<ImportTopicsCommandHandler> _localizer;

        public ImportTopicsCommandHandler(
            IUnitOfWork<int> unitOfWork,
            IExcelService excelService,
            IMapper mapper,
            IValidator<AddEditTopicCommand> addTopicValidator,
            IStringLocalizer<ImportTopicsCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _excelService = excelService;
            _mapper = mapper;
            _addTopicValidator = addTopicValidator;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(ImportTopicsCommand request, CancellationToken cancellationToken)
        {
            var stream = new MemoryStream(request.UploadRequest.Data);
            var result = (await _excelService.ImportAsync(stream, mappers: new Dictionary<string, Func<DataRow, Topic, object>>
            {
                { _localizer["Name"], (row,item) => item.Name = row[_localizer["Name"]].ToString() },
                //{ _localizer["Description"], (row,item) => item.Description = row[_localizer["Description"]].ToString() },
                //{ _localizer["Tax"], (row,item) => item.Tax = decimal.TryParse(row[_localizer["Tax"]].ToString(), out var tax) ? tax : 1 }
            }, _localizer["Topics"]));

            if (result.Succeeded)
            {
                var importedTopics = result.Data;
                var errors = new List<string>();
                var errorsOccurred = false;
                foreach (var topic in importedTopics)
                {
                    var validationResult = await _addTopicValidator.ValidateAsync(_mapper.Map<AddEditTopicCommand>(topic), cancellationToken);
                    if (validationResult.IsValid)
                    {
                        await _unitOfWork.Repository<Topic>().AddAsync(topic);
                    }
                    else
                    {
                        errorsOccurred = true;
                        errors.AddRange(validationResult.Errors.Select(e => $"{(!string.IsNullOrWhiteSpace(topic.Name) ? $"{topic.Name} - " : string.Empty)}{e.ErrorMessage}"));
                    }
                }

                if (errorsOccurred)
                {
                    return await Result<int>.FailAsync(errors);
                }

                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllTopicsCacheKey);
                return await Result<int>.SuccessAsync(result.Data.Count(), result.Messages[0]);
            }
            else
            {
                return await Result<int>.FailAsync(result.Messages);
            }
        }
    }
}