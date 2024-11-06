namespace NeedAnalysisApp.Client.Pages.Assessments;

public partial class Admin
{
    #region Fields

    [Inject] ISnackbar Snackbar { get; set; } = null!;

    [Inject] IAssessmentClientService AssessmentClientService { get; set; } = null!;

    [Inject] IDialogService DialogService { get; set; } = null!;

    [Inject] NavigationManager NavigationManager { get; set; } = null!;

    private bool dense = true;

    private bool hover = true;

    private bool bordered = true;

    private string searchString1 = "";

    private AssessmentDto selectedItem1 = null;

    private HashSet<AssessmentDto> selectedItems = new HashSet<AssessmentDto>();

    private bool IsLoading { get; set; } = true;

    private bool? IncludeInActive { get; set; } = null;

    private List<AssessmentDto> Assessments = [];

    #endregion

    #region Methods

    protected override async Task OnInitializedAsync()
    {
        Assessments = await AssessmentClientService.GetAllAsync();
    }

    private async Task GeneratePdfAsync()
    {
    }

    private async Task PreviewAsync(string assessmentId)
    {
        NavigationManager.NavigateTo($"/client/analysis/{assessmentId}?isPreview=true");
    }

    private bool FilterFunc1(AssessmentDto element) => FilterFunc(element, searchString1);

    private bool FilterFunc(AssessmentDto element, string searchString)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.IndustryType.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if ($"{element.Name} {element.Name} {element.IndustryType}".Contains(searchString))
            return true;
        return false;
    }

    private async void OnAddAssessmentAsync()
    {
        NavigationManager.NavigateTo("/assessment/add-edit");
    }

    private async void OnEditAsync(AssessmentDto assessmentDto)
    {
        NavigationManager.NavigateTo($"/assessment/add-edit/{assessmentDto.UniqueId}");
    }

    private async void OnDeleteAsync(AssessmentDto assessmentDto)
    {
        var parameters = new DialogParameters<ConfirmDeleteDialog>
        {
            { x => x.ContentText, $"Do you really want to delete {assessmentDto.Name}? This process cannot be undone." },
            { x => x.ButtonText, "DeleteAsync" },
            { x => x.Color, Color.Error }
        };

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Large };

        var dialog = await DialogService.ShowAsync<ConfirmDeleteDialog>("DeleteAsync", parameters, options);

        var result = await dialog.Result;

        if (!result.Canceled)
        {
            var actionResult = await AssessmentClientService.DeleteAsync(assessmentDto.UniqueId);

            if (!actionResult.Success && actionResult.Errors.Any())
            {
                foreach (var error in actionResult.Errors)
                {
                    Snackbar.Add(error.Message, Severity.Error);
                }
            }
            else
            {
                Snackbar.Add("Industry Deleted", Severity.Success);
            }

            Assessments = await AssessmentClientService.GetAllAsync();

            StateHasChanged();
        }
    }

    private async void OnChangeIncludeActiveAsync(bool? value)
    {
        IncludeInActive = value;

        var assessments = await AssessmentClientService.GetAllAsync();

        IsLoading = true;

        switch (value)
        {
            case null:
                Assessments = assessments;
                break;
            case true:
                Assessments = assessments.Where(x => x.IsLive).ToList();
                break;
            case false:
                Assessments = assessments.Where(x => !x.IsLive).ToList();
                break;
        }

        IncludeInActive = value;

        IsLoading = false;

        StateHasChanged();
    }

    #endregion
}