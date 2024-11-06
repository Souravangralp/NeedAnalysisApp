namespace NeedAnalysisApp.Client.Pages.ScoreCategories;

public partial class AddEdit
{
    #region Fields

    [Inject] IAssessmentClientService _assessmentClientService { get; set; } = null!;

    [Inject] ISnackbar Snackbar { get; set; } = null!;

    [Parameter] public string? ScoreCategoryId { get; set; }

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;

    private List<ScoreCategoryDto> ScoreCategories = [];

    private ScoreCategoryDto Model = new ScoreCategoryDto() { Value = "Test Value", Recommendation = "Test Recommendation", IsActive = true };

    private MudForm? form;

    private bool IsSecondStep { get; set; } = false;

    #endregion

    #region Methods

    protected override async Task OnInitializedAsync()
    {
        if (string.IsNullOrWhiteSpace(ScoreCategoryId))
        {
            MudDialog.SetTitle("Add score category");
            Model = new ScoreCategoryDto() { Value = "Test Value", Recommendation = "Test Recommendation", IsActive = true };
        }
        else
        {
            var result = await _assessmentClientService.GetScoreCategoryWithIdAsync(ScoreCategoryId);

            if (result.Success)
            {
                var model = JsonConvert.DeserializeObject<ScoreCategoryDto>(result.Model.ToString() ?? string.Empty) ?? new();

                MudDialog.SetTitle($"Edit : {model.Value}");

                Model = model;
            }
        }

        StateHasChanged();
    }

    private void Cancel() => MudDialog.Cancel();

    private void OnCancelClick()
    {
        if (IsSecondStep)
        {
            IsSecondStep = false; 
        }
        else
        {
            MudDialog.Close(DialogResult.Ok(false));
        }
    }

    private async void OnSaveClickAsync()
    {
        if (!IsSecondStep)
        {
            if (string.IsNullOrWhiteSpace(Model.UniqueId))
            {
                var createScoreCategoryResult = await _assessmentClientService.CreateScoreCategoryAsync(Model);

                if (createScoreCategoryResult.Success)
                {
                    IsSecondStep = true;

                    var GetAllScoreCategoriesResult = await _assessmentClientService.GetAllScoreCategoryAsync();

                    if (GetAllScoreCategoriesResult.Success)
                    {
                        ScoreCategories = JsonConvert.DeserializeObject<List<ScoreCategoryDto>>(GetAllScoreCategoriesResult.Model.ToString() ?? string.Empty);

                        StateHasChanged();
                    }
                    else
                    {
                        foreach (var error in GetAllScoreCategoriesResult.Errors)
                        {
                            Snackbar.Add(error.Message, Severity.Warning);
                        }
                    }
                }
                else
                {
                    foreach (var error in createScoreCategoryResult.Errors)
                    {
                        Snackbar.Add(error.Message, Severity.Warning);
                    }
                }
            }
            else
            {
                var updateScoreCategoryResult = await _assessmentClientService.UpdateScoreCategoryAsync(Model);

                if (updateScoreCategoryResult.Success)
                {
                    IsSecondStep = true;

                    var getAllScoreCategoryResult = await _assessmentClientService.GetAllScoreCategoryAsync();

                    if (getAllScoreCategoryResult.Success)
                    {
                        ScoreCategories = JsonConvert.DeserializeObject<List<ScoreCategoryDto>>(getAllScoreCategoryResult.Model.ToString() ?? string.Empty);

                        StateHasChanged();
                    }
                    else
                    {
                        foreach (var error in getAllScoreCategoryResult.Errors)
                        {
                            Snackbar.Add(error.Message, Severity.Warning);
                        }
                    }
                }
                else
                {
                    foreach (var error in updateScoreCategoryResult.Errors)
                    {
                        Snackbar.Add(error.Message, Severity.Warning);
                    }
                }
            }
        }
        else
        {
            var createScoreCategoriesResult = await _assessmentClientService.CreateScoreCategoriesAsync(ScoreCategories);

            if (createScoreCategoriesResult.Success)
            {
                MudDialog.Close(DialogResult.Ok(true));
            }
            else
            {
                foreach (var error in createScoreCategoriesResult.Errors)
                {
                    Snackbar.Add(error.Message, Severity.Warning);
                }
            }
        }
    }


    #endregion
}