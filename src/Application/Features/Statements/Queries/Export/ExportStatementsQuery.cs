using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Extensions;
using BlazorHero.CleanArchitecture.Application.Specifications.Agenda;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BlazorHero.CleanArchitecture.Application.Features.Statements.Queries.Export
{
    public class ExportStatementsQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportStatementsQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportStatementsQueryHandler : IRequestHandler<ExportStatementsQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportStatementsQueryHandler> _localizer;

        public ExportStatementsQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportStatementsQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportStatementsQuery request, CancellationToken cancellationToken)
        {
            var statementFilterSpec = new StatementFilterSpecification(request.SearchString);
            var statements = await _unitOfWork.Repository<Statement>().Entities
                .Specify(statementFilterSpec)
                .ToListAsync( cancellationToken);
            var data = await _excelService.ExportAsync(statements, mappers: new Dictionary<string, Func<Statement, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Body }
            }, sheetName: _localizer["Statements"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}