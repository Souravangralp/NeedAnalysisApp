namespace NeedAnalysisApp.Client.Pages.Assessments;

public partial class AddEdit
{
    #region Fields

    [Inject] IQuestionClientService _questionClientService { get; set; } = null!;

    [Inject] IIndustryClientService industryService { get; set; } = null!;

    [Inject] IAssessmentClientService AssessmentClientService { get; set; } = null!;

    [Inject] NavigationManager NavigationManager { get; set; } = null!;

    [Inject] ISnackbar SnackBar { get; set; } = null!;

    [Parameter] public string? AssessmentId { get; set; } = "";

    private int WhoWeAreQuestionCount { get; set; } = 0;

    private int WhatWeDoQuestionCount { get; set; } = 0;

    private int HowWeDoItQuestionCount { get; set; } = 0;

    public bool IsLoading { get; set; } = false;

    private string value1 = "";

    private List<IndustryDto> Industries { get; set; } = [];

    private List<QuestionDto> Questions { get; set; } = [];

    private AssessmentDto Model { get; set; } = new AssessmentDto() { Name = "" };

    #endregion

    #region Methods

    protected override async Task OnInitializedAsync()
    {
        if (string.IsNullOrWhiteSpace(AssessmentId))
        {
            Model = new() { Name = "Assessment Name", IndustryType = "", IsActive = true, TotalScore = 300 };
        }
        else
        {
            Model = await AssessmentClientService.GetWithIdAsync(AssessmentId);

            var result = await _questionClientService.GetAll(AssessmentId);

            var questions = JsonConvert.DeserializeObject<List<QuestionDto>>(result.Model.ToString() ?? string.Empty);

            Questions = questions;

            if (questions.Any())
            {
                WhoWeAreQuestionCount = questions.Count(x => x.GeneralLookUp_SectionTypeId == 6);
                WhatWeDoQuestionCount = questions.Count(x => x.GeneralLookUp_SectionTypeId == 7);
                HowWeDoItQuestionCount = questions.Count(x => x.GeneralLookUp_SectionTypeId == 8);
            }
        }

        Industries = await industryService.GetAllAsync();

        value1 = Model.IndustryType ?? "";

        StateHasChanged();
    }

    private async Task<IEnumerable<string>> Search(string value, CancellationToken token)
    {
        await Task.Delay(5, token);

        if (string.IsNullOrEmpty(value))
            return Industries.Select(x => x.Name);

        StateHasChanged();

        return Industries.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase)).Select(x => x.Name);
    }

    private async Task OnSectionClickAsync(string sectionName)
    {
        if (string.IsNullOrWhiteSpace(Model.UniqueId))
        {
            var success = await OnSaveAssessmentAsync();

            if (success) { NavigationManager.NavigateTo($"/section/{Model.UniqueId}/{sectionName}"); }
        }
        else
        {
            NavigationManager.NavigateTo($"/section/{Model.UniqueId}/{sectionName}");
        }
    }

    private void OnIndustryTypeChange(string value)
    {
        Model.IndustryType = value;
    }

    private async Task<bool> OnSaveAssessmentAsync()
    {
        Result result = string.IsNullOrWhiteSpace(Model.UniqueId) ? result = await AssessmentClientService.CreateAsync(Model) : result = await AssessmentClientService.UpdateAsync(Model);

        if (result.Success)
        {
            SnackBar.Add($"Cheers! You have successfully {(string.IsNullOrWhiteSpace(Model.UniqueId) ? "added " : "updated ")}{Model.Name}", Severity.Success);

            Model = JsonConvert.DeserializeObject<AssessmentDto>(result.Model.ToString() ?? string.Empty);

            StateHasChanged();

            return true;
        }
        else
        {
            foreach (var error in result.Errors)
            {
                SnackBar.Add(error.Message, Severity.Error);
            }

            return false;
        }
    }

    private async void OnSendForReviewAsync()
    {
        Model.IsLive = true;

        var result = await OnSaveAssessmentAsync();

        if (result)
        {
            SnackBar.Add($"{Model.Name} sent for review!", Severity.Success);

            NavigationManager.NavigateTo($"/assessment");
        }
    }

    private async void OnNavigateBackAsync()
    {
        NavigationManager.NavigateTo($"/assessment");
    }

    #endregion
}