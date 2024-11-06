namespace NeedAnalysisApp.Client.Pages.ScoreCategories;

public partial class Home
{
    #region Fields

    [Inject] IAssessmentClientService _assessmentClientService { get; set; } = null!;

    [Inject] IDialogService DialogService { get; set; } = null!;

    [Inject] ISnackbar SnackBar { get; set; } = null!;

    List<ScoreCategoryDto> ScoreCategories = [];

    int TotalScore = 300;

    private string searchedValue = "";

    #endregion

    #region Methods

    protected override async Task OnInitializedAsync()
    {
        var result = await _assessmentClientService.GetAllScoreCategoryAsync();

        if (result.Success)
        {
            ScoreCategories = JsonConvert.DeserializeObject<List<ScoreCategoryDto>>(result.Model.ToString() ?? string.Empty);
        }
    }

    public async void OnAddScoreCategoryAsync()
    {
        var parameters = new DialogParameters<AddEdit>
        {
            { x => x.ScoreCategoryId, string.Empty }
        };

        var options = new DialogOptions() { CloseButton = true, FullWidth = true, MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<AddEdit>("Add new", parameters, options);

        var result = await dialog.Result;

        if (!result.Canceled)
        {
            if ((bool)result.Data)
            {
                var assessmentResult = await _assessmentClientService.GetAllScoreCategoryAsync();

                if (assessmentResult.Success)
                {
                    ScoreCategories = JsonConvert.DeserializeObject<List<ScoreCategoryDto>>(assessmentResult.Model.ToString() ?? string.Empty);
                }
            }
        }

        StateHasChanged();
    }

    public async void OnEditScoreCategoryAsync(ScoreCategoryDto scoreCategory)
    {
        var parameters = new DialogParameters<AddEdit>
        {
            { x => x.ScoreCategoryId, scoreCategory.UniqueId}
        };

        var options = new DialogOptions() { CloseButton = true, FullWidth = true, MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<AddEdit>("Add new", parameters, options);

        var result = await dialog.Result;

        if (!result.Canceled)
        {
            if ((bool)result.Data)
            {
                var assessmentResult = await _assessmentClientService.GetAllScoreCategoryAsync();

                if (assessmentResult.Success)
                {
                    ScoreCategories = JsonConvert.DeserializeObject<List<ScoreCategoryDto>>(assessmentResult.Model.ToString() ?? string.Empty);
                }
            }
        }

        StateHasChanged();
    }

    #endregion
}