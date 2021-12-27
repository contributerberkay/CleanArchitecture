using BlazorHero.CleanArchitecture.Application.Features.Topics.Queries.GetAll;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Extensions;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Features.Topics.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.Topics.Commands.Import;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Agenda.Topic
{
    public class TopicManager : ITopicManager
    {
        private readonly HttpClient _httpClient;

        public TopicManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.TopicsEndpoints.Export
                : Routes.TopicsEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.TopicsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllTopicsResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.TopicsEndpoints.GetAll);
            return await response.ToResult<List<GetAllTopicsResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditTopicCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.TopicsEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> ImportAsync(ImportTopicsCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.TopicsEndpoints.Import, request);
            return await response.ToResult<int>();
        }
    }
}