using BlazorHero.CleanArchitecture.Application.Features.Topics.Queries.GetAll;
using BlazorHero.CleanArchitecture.Client.Extensions;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Features.Topics.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Agenda.Topic;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using BlazorHero.CleanArchitecture.Application.Features.Topics.Commands.Import;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using BlazorHero.CleanArchitecture.Application.Requests;
using BlazorHero.CleanArchitecture.Client.Shared.Components;

namespace BlazorHero.CleanArchitecture.Client.Pages.Agenda
{
    public partial class Topics
    {
        [Inject] private ITopicManager TopicManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllTopicsResponse> _topicList = new();
        private GetAllTopicsResponse _topic = new();
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateTopics;
        private bool _canEditTopics;
        private bool _canDeleteTopics;
        private bool _canExportTopics;
        private bool _canSearchTopics;
        private bool _canImportTopics;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateTopics = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Topics.Create)).Succeeded;
            _canEditTopics = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Topics.Edit)).Succeeded;
            _canDeleteTopics = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Topics.Delete)).Succeeded;
            _canExportTopics = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Topics.Export)).Succeeded;
            _canSearchTopics = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Topics.Search)).Succeeded;
            _canImportTopics = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Topics.Import)).Succeeded;

            await GetTopicsAsync();
            _loaded = true;

            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetTopicsAsync()
        {
            var response = await TopicManager.GetAllAsync();
            if (response.Succeeded)
            {
                _topicList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task Delete(int id)
        {
            string deleteContent = _localizer["Delete Content"];
            var parameters = new DialogParameters
            {
                { nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id) }
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await TopicManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    await Reset();
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    await Reset();
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }

        private async Task ExportToExcel()
        {
            var response = await TopicManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Topics).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Topics exported"]
                    : _localizer["Filtered Topics exported"], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                _topic = _topicList.FirstOrDefault(c => c.Id == id);
                if (_topic != null)
                {
                    parameters.Add(nameof(AddEditTopicModal.AddEditTopicModel), new AddEditTopicCommand
                    {
                        Id = _topic.Id,
                        Name = _topic.Name,
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditTopicModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task<IResult<int>> ImportExcel(UploadRequest uploadFile)
        {
            var request = new ImportTopicsCommand { UploadRequest = uploadFile };
            var result = await TopicManager.ImportAsync(request);
            return result;
        }

        private async Task InvokeImportModal()
        {
            var parameters = new DialogParameters
            {
                { nameof(ImportExcelModal.ModelName), _localizer["Topics"].ToString() }
            };
            Func<UploadRequest, Task<IResult<int>>> importExcel = ImportExcel;
            parameters.Add(nameof(ImportExcelModal.OnSaved), importExcel);
            var options = new DialogOptions
            {
                CloseButton = true,
                MaxWidth = MaxWidth.Small,
                FullWidth = true,
                DisableBackdropClick = true
            };
            var dialog = _dialogService.Show<ImportExcelModal>(_localizer["Import"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _topic = new GetAllTopicsResponse();
            await GetTopicsAsync();
        }

        private bool Search(GetAllTopicsResponse topic)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (topic.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return /*topic.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ==*/ true;
        }
    }
}