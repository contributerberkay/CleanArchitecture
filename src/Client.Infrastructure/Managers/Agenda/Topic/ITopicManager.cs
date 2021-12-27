using BlazorHero.CleanArchitecture.Application.Features.Topics.Queries.GetAll;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Features.Topics.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.Topics.Commands.Import;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Agenda.Topic
{
    public interface ITopicManager : IManager
    {
        Task<IResult<List<GetAllTopicsResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditTopicCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");

        Task<IResult<int>> ImportAsync(ImportTopicsCommand request);
    }
}