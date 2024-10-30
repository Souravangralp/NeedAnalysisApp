using Microsoft.AspNetCore.Components;
using MudBlazor;
using NeedAnalysisApp.Client.Pages.Assessments.Templates;
using NeedAnalysisApp.Client.Repositories.Interfaces;
using NeedAnalysisApp.Shared.Common.Utilities;
using NeedAnalysisApp.Shared.Dto;
using Newtonsoft.Json;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace NeedAnalysisApp.Client.Pages.Assessments;

public partial class Section
{
    #region Fields

    [Inject] public IQuestionClientService _questionClientService { get; set; } = null!;

    [Inject] NavigationManager NavigationManager { get; set; } = null!;

    [Inject] IDialogService DialogService { get; set; } = null!;

    [Inject] ISnackbar Snackbar { get; set; } = null!;

    [Parameter] public required string SectionName { get; set; }

    [Parameter] public required string AssessmentId { get; set; }

    private Type? SelectedQuestionTemplate = Type.GetType(typeof(DefaultTemplate).ToString());

    List<QuestionDto> Questions { get; set; } = [];

    public QuestionDto SelectedQuestion { get; set; } = new();

    public Dictionary<string, object> parameters { get; set; } = new();

    public int SelectedQuestionType { get; set; } = 0;

    #endregion

    #region Methods

    protected override async Task OnInitializedAsync()
    {
        var result = await _questionClientService.GetAll(AssessmentId);

        if (result.Success)
        {
            var questions = JsonConvert.DeserializeObject<List<QuestionDto>>(result.Model.ToString() ?? string.Empty);

            Questions = questions.Where(x => x.GeneralLookUp_SectionTypeId == SectionUtility.GetSectionId(SectionName)).ToList();

            //SelectedQuestion = Questions.OrderByDescending(x => x.DisplayOrder).FirstOrDefault();

            //await GetTemplate((int)SelectedQuestion.GeneralLookUp_QuestionTypeId);
        }

        StateHasChanged();
    }

    public void OpenQuestion(QuestionDto questionDto)
    {
        SelectedQuestion = questionDto;

        parameters.Remove("QuestionUniqueId");
        parameters["QuestionUniqueId"] = questionDto.UniqueId;
        parameters["AssessmentUniqueId"] = AssessmentId;
        parameters["IsTemplate"] = false;
        parameters["OnDelete"] = EventCallback.Factory.Create<bool>(
                                                this, HandleQuestionDelete);
        parameters["OnSave"] = EventCallback.Factory.Create<bool>(
                                                this, HandleQuestionSave);

        SelectedQuestionTemplate = GetQuestionTemplate((int)questionDto.GeneralLookUp_QuestionTypeId);

        StateHasChanged();
    }

    public async void Save()
    {

    }

    public async void AddQuestion()
    {
        if (!string.IsNullOrWhiteSpace(SelectedQuestion.UniqueId))
        {
            var saveExistingQuestionResult = await _questionClientService.Update(SelectedQuestion, AssessmentId);

            if (saveExistingQuestionResult.Success)
            {
                Snackbar.Add("Question saved", Severity.Success);

                SelectedQuestionTemplate = GetQuestionTemplate(0);

                SelectedQuestion = new();
            }
            else
            {
                foreach (var error in saveExistingQuestionResult.Errors)
                {
                    Snackbar.Add(error.Message, Severity.Error);
                }
            }
        }

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };

        var dialog = await DialogService.ShowAsync<QuestionDialog>("Select a question type", /* parameters ,*/ options);

        var result = await dialog.Result;

        if (!result.Canceled)
        {
            SelectedQuestionType = (int)result.Data;

            await SaveQuestion();
        }

        StateHasChanged();
    }

    public static Type? GetQuestionTemplate(int selectedQuestionTypeId)
    {
        return selectedQuestionTypeId switch
        {
            0 => Type.GetType(typeof(DefaultTemplate).ToString()),
            1 => Type.GetType(typeof(MultipleChoice).ToString()),
            2 => Type.GetType(typeof(TrueFalse).ToString()),
            3 => Type.GetType(typeof(Text).ToString()),
            4 => Type.GetType(typeof(Percentage).ToString()),
            5 => Type.GetType(typeof(Label).ToString()),
            _ => null,
        };
    }

    public void GetTemplate(int selectedQuestionTypeId)
    {
        parameters["QuestionUniqueId"] = "";
        parameters["AssessmentUniqueId"] = AssessmentId;
        parameters["IsTemplate"] = false;
        parameters["OnDelete"] = EventCallback.Factory.Create<bool>(
                                                this, HandleQuestionDelete);
        parameters["OnSave"] = EventCallback.Factory.Create<bool>(
                                                this, HandleQuestionSave);

        SelectedQuestionTemplate = GetQuestionTemplate(selectedQuestionTypeId);
    }

    public async Task SaveQuestion()
    {
        var sectionTypeId = SectionUtility.GetSectionId(SectionName);

        var question = SectionUtility.GetDemoQuestion(SelectedQuestionType, sectionTypeId);

        question.DisplayOrder = SectionUtility.GetNewDisplayOrder(Questions);

        var result = await _questionClientService.Create(question, AssessmentId);

        if (result.Success)
        {
            question = JsonConvert.DeserializeObject<QuestionDto>(result.Model.ToString() ?? string.Empty);

            Questions.Add(question);

            Snackbar.Add("Question successfully added", Severity.Success);

            SelectedQuestion = question;

            StateHasChanged();
        }
        else
        {
            foreach (var error in result.Errors)
            {
                Snackbar.Add(error.Message, Severity.Error);
            }
        }

    }

    private async Task HandleQuestionDelete(bool onDelete)
    {
        var result = await _questionClientService.Delete(SelectedQuestion.UniqueId); // call api service here

        if (result.Success)
        {
            Questions.Remove(SelectedQuestion);

            SelectedQuestion = new();

            Snackbar.Add("Question removed", Severity.Success);

            SelectedQuestionTemplate = GetQuestionTemplate(0);
        }
        else
        {
            foreach (var error in result.Errors)
            {
                Snackbar.Add(error.Message, Severity.Error);
            }
        }

        StateHasChanged();
    }

    private async Task HandleQuestionSave(bool isSavePerformed)
    {
        if (isSavePerformed)
        {
            var result = await _questionClientService.GetAll(AssessmentId);

            if (result.Success)
            {
                var questions = JsonConvert.DeserializeObject<List<QuestionDto>>(result.Model.ToString() ?? string.Empty);

                Questions = questions.Where(x => x.GeneralLookUp_SectionTypeId == SectionUtility.GetSectionId(SectionName)).ToList();

                SelectedQuestionTemplate = GetQuestionTemplate(0);
            }

            StateHasChanged();

            //Questions.Add(SelectedQuestion);
        }

        SelectedQuestionType = 0;

        StateHasChanged();
    }

    public async void AddLabel()
    {
        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };

        var dialog = await DialogService.ShowAsync<AddEditLabelDialog>("Add Label", /* parameters ,*/ options);

        var result = await dialog.Result;

        if (!result.Canceled)
        {

            var question = (QuestionDto)result.Data;

            var lastDisplayOrder = Questions.Count > 0 ? Questions.Max(x => x.DisplayOrder) : 1;

            question.DisplayOrder = lastDisplayOrder + 1;
            question.GeneralLookUp_SectionTypeId = SectionUtility.GetSectionId(SectionName);

            var questionResult = await _questionClientService.Create(question, AssessmentId);

            if (questionResult.Success)
            {
                Snackbar.Add("Label added successfully", Severity.Success);

                SelectedQuestionTemplate = GetQuestionTemplate(0);

                var questions = await _questionClientService.GetAll(AssessmentId);

                if (questions.Success)
                {
                    var questions1 = JsonConvert.DeserializeObject<List<QuestionDto>>(questions.Model.ToString() ?? string.Empty);

                    Questions = questions1.Where(x => x.GeneralLookUp_SectionTypeId == SectionUtility.GetSectionId(SectionName)).ToList();
                }
            }
            else
            {
                if (questionResult.Errors.Any())
                {
                    foreach (var error in questionResult.Errors)
                    {
                        Snackbar.Add(error.Message, Severity.Warning);
                    }
                }
            }
        }

        SelectedQuestionTemplate = GetQuestionTemplate(0);

        StateHasChanged();
    }

    public void OnNavigateBack()
    {
        NavigationManager.NavigateTo($"/assessment/add-edit/{AssessmentId}");
    }

    #endregion
}
