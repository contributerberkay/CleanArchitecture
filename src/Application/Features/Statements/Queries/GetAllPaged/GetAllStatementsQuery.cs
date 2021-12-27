using BlazorHero.CleanArchitecture.Application.Extensions;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Application.Specifications.Agenda;
using BlazorHero.CleanArchitecture.Domain.Entities;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Statements.Queries.GetAllPaged
{
    public class GetAllStatementsQuery : IRequest<PaginatedResult<GetAllPagedStatementsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; } // of the form fieldname [ascending|descending],fieldname [ascending|descending]...

        public GetAllStatementsQuery(int pageNumber, int pageSize, string searchString, string orderBy)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
        }
    }

    internal class GetAllStatementsQueryHandler : IRequestHandler<GetAllStatementsQuery, PaginatedResult<GetAllPagedStatementsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllStatementsQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedStatementsResponse>> Handle(GetAllStatementsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Statement, GetAllPagedStatementsResponse>> expression = e => new GetAllPagedStatementsResponse
            {
                Id = e.Id,
                Body = e.Body,
                Topic = e.Topic.Name,
                TopicId = (int)e.TopicId
            };
            var statementFilterSpec = new StatementFilterSpecification(request.SearchString);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<Statement>().Entities
                   .Specify(statementFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<Statement>().Entities
                   .Specify(statementFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}