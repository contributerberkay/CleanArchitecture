using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Extensions;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Application.Specifications.Agenda;
using BlazorHero.CleanArchitecture.Domain.Entities;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BlazorHero.CleanArchitecture.Application.Features.Topics.Queries.Export
{
    public class ExportTopicsQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportTopicsQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportTopicsQueryHandler : IRequestHandler<ExportTopicsQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportTopicsQueryHandler> _localizer;

        public ExportTopicsQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportTopicsQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportTopicsQuery request, CancellationToken cancellationToken)
        {
            var TopicFilterSpec = new TopicFilterSpecification(request.SearchString);
            var Topics = await _unitOfWork.Repository<Topic>().Entities
                .Specify(TopicFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(Topics, mappers: new Dictionary<string, Func<Topic, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name },
            }, sheetName: _localizer["Topics"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
