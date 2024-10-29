using Microsoft.AspNetCore.Components;
using NeedAnalysisApp.Client.Repositories.Interfaces;
using NeedAnalysisApp.Shared.Dto;

namespace NeedAnalysisApp.Client.Pages.Admins;

public partial class Dashboard
{
    [Inject] public IIndustryClientService _industryClientService { get; set; } = null!;

    [Inject] public IAssessmentClientService _assessmentClientService { get; set; } = null!;

    [Inject] public IUserClientService _userClientService { get; set; } = null!;

    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    public double[] data = Array.Empty<double>();

    public string[] labels = Array.Empty<string>();

    private int Index = 1; //default value cannot be 0 -> first selected index is 0.

    private List<IndustryDto> Industries = [];

    private List<UserDto> Users = [];

    public double SelectedIndex { get; set; } = 0;

    public int ActiveAssessment { get; set; } = 0;

    public int InActiveAssessment { get; set; } = 0;

    public string SelectedIndustryType { get; set; } = "";

    protected override async Task OnInitializedAsync()
    {
        Users = await _userClientService.GetAll("User");

        Industries = await _industryClientService.GetAll();

        labels = Industries.Select(x => x.Name).ToArray();

        var assessments = await _assessmentClientService.GetAll();

        ActiveAssessment = assessments.Where(x => x.IsLive).Count();

        InActiveAssessment = assessments.Where(x => !x.IsLive).Count();

        data = new double[labels.Length];

        foreach (var assessment in assessments)
        {
            // Check if the IndustryType of the assessment is in the labels
            var index = Array.IndexOf(labels, assessment.IndustryType);
            if (index >= 0)
            {
                // Increment the count at the found index
                data[index]++;
            }
        }

        StateHasChanged();
    }

    public async void OnAssessmentIndexChanged(int currentValue)
    {
        Index = currentValue;

        var selectedIndustryType = labels[currentValue];

        // Get the count of assessments for the selected index
        var assessments = await _assessmentClientService.GetAll();

        var count = assessments.Count(assessment => assessment.IndustryType == selectedIndustryType);

        // You can store the count in a field if needed
        SelectedIndex = count;

        SelectedIndustryType = selectedIndustryType;

        StateHasChanged();
    }

    public void OnAddAssessment()
    {
        NavigationManager.NavigateTo("/assessment");
    }




}
