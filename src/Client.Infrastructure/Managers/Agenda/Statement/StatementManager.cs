using BlazorHero.CleanArchitecture.Application.Features.Statements.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.Statements.Queries.GetAllPaged;
using BlazorHero.CleanArchitecture.Application.Requests.Agenda;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Extensions;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Agenda.Statement
{
    public class StatementManager : IStatementManager
    {
        private readonly HttpClient _httpClient;

        public StatementManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.StatementsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.StatementsEndpoints.Export
                : Routes.StatementsEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<PaginatedResult<GetAllPagedStatementsResponse>> GetStatementsAsync(GetAllPagedStatementsRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.StatementsEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllPagedStatementsResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditStatementCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.StatementsEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}