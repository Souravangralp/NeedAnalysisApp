namespace NeedAnalysisApp.Client.Pages.Admins;

public partial class Dashboard
{
    #region Fields

    [Inject] private IIndustryClientService _industryClientService { get; set; } = null!;

    [Inject] private IAssessmentClientService _assessmentClientService { get; set; } = null!;

    [Inject] private IUserClientService _userClientService { get; set; } = null!;

    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    private double[] data = Array.Empty<double>();

    private string[] labels = Array.Empty<string>();

    private int Index = 1; //default value cannot be 0 -> first selected index is 0.

    private List<IndustryDto> Industries = [];

    private List<UserDto> Users = [];

    private double SelectedIndex { get; set; } = 0;

    private int ActiveAssessment { get; set; } = 0;

    private int InActiveAssessment { get; set; } = 0;

    private string SelectedIndustryType { get; set; } = "";

    #endregion

    #region Methods

    protected override async Task OnInitializedAsync()
    {
        Users = await _userClientService.GetAllAsync("User");

        Industries = await _industryClientService.GetAllAsync();

        labels = Industries.Select(x => x.Name).ToArray();

        var assessments = await _assessmentClientService.GetAllAsync();

        ActiveAssessment = assessments.Where(x => x.IsLive).Count();

        InActiveAssessment = assessments.Where(x => !x.IsLive).Count();

        data = new double[labels.Length];

        foreach (var assessment in assessments)
        {
            var index = Array.IndexOf(labels, assessment.IndustryType);
            if (index >= 0)
            {
                data[index]++;
            }
        }

        StateHasChanged();
    }

    public async void OnAssessmentIndexChanged(int currentValue)
    {
        Index = currentValue;

        var selectedIndustryType = labels[currentValue];

        var assessments = await _assessmentClientService.GetAllAsync();

        var count = assessments.Count(assessment => assessment.IndustryType == selectedIndustryType);

        SelectedIndex = count;

        SelectedIndustryType = selectedIndustryType;

        StateHasChanged();
    }

    public void OnAddAssessment()
    {
        NavigationManager.NavigateTo("/assessment");
    }

    #endregion
}