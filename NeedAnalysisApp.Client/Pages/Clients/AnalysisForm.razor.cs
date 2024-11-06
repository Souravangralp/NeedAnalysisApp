namespace NeedAnalysisApp.Client.Pages.Clients;

public partial class AnalysisForm
{
    #region Fideld

    [Inject] ISnackbar Snackbar { get; set; } = null!;

    [Inject] NavigationManager NavigationManager { get; set; } = null!;

    [Inject] IQuestionClientService _questionClientService { get; set; } = null!;

    [Inject] IAssessmentClientService _assessmentClientService { get; set; } = null!;

    [Parameter] public string AssessmentId { get; set; } = "";

    [Parameter] public bool IsPreview { get; set; } = false;

    public List<QuestionDto> Questions { get; set; } = [];

    public bool Dense_Radio { get; set; } = false;

    public AssessmentDto Model { get; set; } = new AssessmentDto() { Name = "" };

    private Dictionary<string, string> SelectedOptions = new Dictionary<string, string>();

    //[Comment("This property is being used to get the Selected Option")]
    //string SelectedOption { get; set; }

    MudTabs tabs;
    MudTabPanel WhoWeAreTab;
    MudTabPanel WhatWeDoTab;
    MudTabPanel HowWeDoItTab;

    #endregion

    #region Methods

    protected override async Task OnInitializedAsync()
    {
        var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);

        var query = QueryHelpers.ParseQuery(uri.Query);

        if (query.TryGetValue("isPreview", out var isPreviewValue))
        {
            IsPreview = bool.TryParse(isPreviewValue, out var isPreview) && isPreview;
        }

        Model = await _assessmentClientService.GetWithIdAsync(AssessmentId);

        var result = await _questionClientService.GetAll(AssessmentId);

        if (result.Success)
        {
            var questions = JsonConvert.DeserializeObject<List<QuestionDto>>(result.Model.ToString() ?? string.Empty);

            Questions = questions;
        }

        StateHasChanged();
    }

    private void OnSelectedOptionChanged(string selectedValue, string questionId)
    {
        if (SelectedOptions.ContainsKey(questionId))
        {
            SelectedOptions[questionId] = selectedValue;
        }
        else
        {
            SelectedOptions.Add(questionId, selectedValue);
        }
    }

    private string GetSelectedValue(string questionId)
    {
        return SelectedOptions.TryGetValue(questionId, out var selectedValue) ? selectedValue : null;
    }

    public void OnNextClick(MudTabPanel mudTabPanel)
    {
        tabs.ActivatePanel(mudTabPanel);
    }

    public void ClosePreview()
    {
        NavigationManager.NavigateTo("/assessment");
    }

    public void OnSubmitClick()
    {
        double marks = 0.0;
        double totalMarks = 0.0;
        double WhoWeAre_Marks = 0.0;
        double WhatWeDo_Marks = 0.0;
        double HowWeDoIt_Marks = 0.0;

        //check if selected options contains the values of user selected options
        if (SelectedOptions.Any() && Questions.Any())
        {
            foreach (var question in Questions)
            {
                totalMarks += question.Options.Sum(x => x.Point);

                if (question.GeneralLookUp_SectionTypeId == 6)
                {
                    WhoWeAre_Marks += question.Options.Sum(x => x.Point);
                }
                if (question.GeneralLookUp_SectionTypeId == 7)
                {
                    WhatWeDo_Marks += question.Options.Sum(x => x.Point);
                }
                if (question.GeneralLookUp_SectionTypeId == 8)
                {
                    HowWeDoIt_Marks += question.Options.Sum(x => x.Point);
                }
            }

            foreach (var selectedOption in SelectedOptions)
            {
                foreach (var question in Questions)
                {
                    var option = question.Options.Where(x => x.Value == selectedOption.Value).FirstOrDefault();

                    if (option != null) { marks += option.Point; }
                }
            }
        }

        var percentage = (marks / totalMarks) * 100;

        if (percentage <= 60)
        {
            Snackbar.Add("You are under Innovation-Lagging Organization", Severity.Warning);
        }
        if (percentage >= 61 && percentage <= 120)
        {
            Snackbar.Add("You are under Innovation-Aware Organization", Severity.Success);
        }
        Snackbar.Add($"You have got {marks} out of {totalMarks}", Severity.Success);
    }

    #endregion
}