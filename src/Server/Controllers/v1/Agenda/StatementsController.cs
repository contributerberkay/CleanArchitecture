using BlazorHero.CleanArchitecture.Application.Features.Statements.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.Statements.Commands.Delete;
using BlazorHero.CleanArchitecture.Application.Features.Statements.Queries.Export;
using BlazorHero.CleanArchitecture.Application.Features.Statements.Queries.GetAllPaged;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Server.Controllers.v1.Agenda
{
    public class StatementsController : BaseApiController<StatementsController>
    {
        /// <summary>
        /// Get All Statements
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Statements.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var products = await _mediator.Send(new GetAllStatementsQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(products);
        }

        /// <summary>
        /// Add/Edit a Statement
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Statements.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditStatementCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Statement
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Statements.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteStatementCommand { Id = id }));
        }

        /// <summary>
        /// Search Statements and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Statements.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportStatementsQuery(searchString)));
        }
    }
}