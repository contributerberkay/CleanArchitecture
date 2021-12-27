using BlazorHero.CleanArchitecture.Application.Features.Topics.Queries.GetAll;
using BlazorHero.CleanArchitecture.Application.Features.Topics.Queries.GetById;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Features.Topics.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.Topics.Commands.Delete;
using BlazorHero.CleanArchitecture.Application.Features.Topics.Queries.Export;
using BlazorHero.CleanArchitecture.Application.Features.Topics.Commands.Import;

namespace BlazorHero.CleanArchitecture.Server.Controllers.v1.Agenda
{
    public class TopicsController : BaseApiController<TopicsController>
    {
        /// <summary>
        /// Get All Topics
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Topics.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var brands = await _mediator.Send(new GetAllTopicsQuery());
            return Ok(brands);
        }

        /// <summary>
        /// Get a Topic By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Topics.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var brand = await _mediator.Send(new GetTopicByIdQuery() { Id = id });
            return Ok(brand);
        }

        /// <summary>
        /// Create/Update a Topic
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Topics.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditTopicCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Topic
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Topics.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteTopicCommand { Id = id }));
        }

        /// <summary>
        /// Search Topics and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Topics.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportTopicsQuery(searchString)));
        }

        /// <summary>
        /// Import Topics from Excel
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Topics.Import)]
        [HttpPost("import")]
        public async Task<IActionResult> Import(ImportTopicsCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}