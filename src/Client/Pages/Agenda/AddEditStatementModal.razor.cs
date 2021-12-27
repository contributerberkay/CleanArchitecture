using BlazorHero.CleanArchitecture.Application.Features.Topics.Queries.GetAll;
using BlazorHero.CleanArchitecture.Application.Features.Statements.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Requests;
using BlazorHero.CleanArchitecture.Client.Extensions;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blazored.FluentValidation;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Agenda.Topic;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Agenda.Statement;

namespace BlazorHero.CleanArchitecture.Client.Pages.Agenda
{
    public partial class AddEditStatementModal
    {
        [Inject] private IStatementManager StatementManager { get; set; }
        [Inject] private ITopicManager TopicManager { get; set; }

        [Parameter] public AddEditStatementCommand AddEditStatementModel { get; set; } = new();
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private List<GetAllTopicsResponse> _topics = new();

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            var response = await StatementManager.SaveAsync(AddEditStatementModel);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                MudDialog.Close();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task LoadDataAsync()
        {
            await LoadTopicsAsync();
        }

        private async Task LoadTopicsAsync()
        {
            var data = await TopicManager.GetAllAsync();
            if (data.Succeeded)
            {
                _topics = data.Data;
            }
        }


        private void DeleteAsync()
        {
        }

        private IBrowserFile _file;


        private async Task<IEnumerable<int>> SearchTopics(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _topics.Select(x => x.Id);

            return _topics.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }
    }
}