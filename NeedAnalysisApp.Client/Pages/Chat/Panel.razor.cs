using Microsoft.AspNetCore.Components;
using NeedAnalysisApp.Client.Repositories.Interfaces;
using NeedAnalysisApp.Shared.Dto;

namespace NeedAnalysisApp.Client.Pages.Chat;

public partial class Panel
{
    [Inject] public IUserClientService _userClientService { get; set; } = null!;

    UserDto User { get; set; } = new();

    [Parameter] public required string UserId { get; set; }

    [Parameter] public bool IsDefault { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (!string.IsNullOrWhiteSpace(UserId))
        {
            User = await _userClientService.GetWithId(UserId);
        }

        StateHasChanged();
    }

    //private bool _parametersInitialized = false; // To avoid unnecessary calls

    protected override async Task OnParametersSetAsync()
    {
        if (!string.IsNullOrWhiteSpace(UserId))
        {
            User = await _userClientService.GetWithId(UserId);
        }

        // Check if parameters have been initialized
        //if (_parametersInitialized &&
        //    (!string.IsNullOrWhiteSpace(UserId)) && !IsDefault)
        //{
        //    User = await _userClientService.GetWithId(UserId);
        //}

        //_parametersInitialized = true; // Set this to true after the first initialization

        StateHasChanged();
    }

    private string MessageText { get; set; } = "";
    private List<MessageTT> Messages { get; set; } = new List<MessageTT>();

    private void SendMessage()
    {
        if (!string.IsNullOrWhiteSpace(MessageText))
        {
            Messages.Add(new MessageTT { Content = MessageText, User = User });
            MessageText = ""; // Clear the input field
        }
    }

    public class MessageTT
    {
        public string Content { get; set; }
        public UserDto User { get; set; }
    }

}
