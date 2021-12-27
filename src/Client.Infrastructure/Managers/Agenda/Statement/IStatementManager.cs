using BlazorHero.CleanArchitecture.Application.Features.Statements.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.Statements.Queries.GetAllPaged;
using BlazorHero.CleanArchitecture.Application.Requests.Agenda;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Agenda.Statement
{
    public interface IStatementManager : IManager
    {
        Task<PaginatedResult<GetAllPagedStatementsResponse>> GetStatementsAsync(GetAllPagedStatementsRequest request);

        Task<IResult<int>> SaveAsync(AddEditStatementCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}