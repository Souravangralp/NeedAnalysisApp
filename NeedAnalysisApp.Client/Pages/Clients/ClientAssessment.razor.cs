namespace NeedAnalysisApp.Client.Pages.Clients;

public partial class ClientAssessment
{
    #region Fields

    [Inject] AuthenticationStateProvider _persistentAuthenticationStateProvider { get; set; } = null!;

    [Inject] IAssessmentClientService _assessmentClientService { get; set; } = null!;

    [Inject] IDialogService DialogService { get; set; } = null!;

    [Inject] ISnackbar SnackBar { get; set; } = null!;

    [Inject] NavigationManager NavigationManager { get; set; } = null!;

    public List<AssessmentDto> AllAssessments = [];

    public List<AssessmentDto> MyAssessments = [];

    MudTabs MudTabs;
    MudTabPanel AllAssessmentsTab;
    MudTabPanel MyAssessmentsTab;

    public string userId = "";

    #endregion

    #region Methods

    protected override async Task OnInitializedAsync()
    {
        var authenticationState = await _persistentAuthenticationStateProvider.GetAuthenticationStateAsync();

        userId = authenticationState.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var userAssessment = await _assessmentClientService.GetUserAssessmentAsync(userId);

        if (userAssessment.Success)
        {
            if (userAssessment.Model != null)
            {
                MyAssessments = JsonConvert.DeserializeObject<List<AssessmentDto>>(userAssessment.Model.ToString() ?? string.Empty);
            }
        }

        var assessments = await _assessmentClientService.GetAllAsync();

        if (MyAssessments.Any())
        {
            var myAssessmentIds = new HashSet<string>(MyAssessments.Select(a => a.UniqueId));

            AllAssessments = assessments.Where(assessment => !myAssessmentIds.Contains(assessment.UniqueId)).ToList();
        }
        else
        {
            AllAssessments = assessments;
        }

        StateHasChanged();
    }

    public async Task OnAddAssessmentAsync(AssessmentDto assessment)
    {
        var parameters = new DialogParameters<ConfirmDeleteDialog>
        {
            { x => x.ContentText, $"Do you want to add `{assessment.Name}` To your collection?" },
            { x => x.ButtonText, "Yes" },
            { x => x.Color, Color.Success }
        };

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Large };

        var dialog = await DialogService.ShowAsync<ConfirmDeleteDialog>("Confirm", parameters, options);

        var result = await dialog.Result;

        if ((bool)result.Data)
        {
            var response = await _assessmentClientService.AssignAssessmentAsync(assessment.UniqueId, userId);

            if (response.Success)
            {
                SnackBar.Add($"You have successfully added : {assessment.Name}", Severity.Success);

                var myAssessmentIds = new HashSet<string>(MyAssessments.Select(a => a.UniqueId));

                AllAssessments = AllAssessments.Where(assessment => !myAssessmentIds.Contains(assessment.UniqueId)).ToList();

                AllAssessments.Remove(assessment);

                var userAssessment = await _assessmentClientService.GetUserAssessmentAsync(userId);

                if (userAssessment.Success)
                {
                    if (userAssessment.Model != null)
                    {
                        MyAssessments = JsonConvert.DeserializeObject<List<AssessmentDto>>(userAssessment.Model.ToString() ?? string.Empty);
                    }
                }

                MudTabs.ActivatePanel(MyAssessmentsTab);

                StateHasChanged();
            }
            else
            {
                foreach (var error in response.Errors)
                {
                    SnackBar.Add(error.Message, Severity.Error);
                }
            }
        }
    }

    public async Task OnPerformAssessmentClickedAsync(string uniqueId)
    {
        NavigationManager.NavigateTo($"/client/analysis/{uniqueId}");
    }

    #endregion
}