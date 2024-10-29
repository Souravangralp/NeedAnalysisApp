using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace NeedAnalysisApp.Client.Pages.Assessments;

public partial class QuestionDialog
{
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; }

    public List<QuestionType> QuestionTypes { get; set; } = [];

    public int SelectedQuestionTypeId { get; set; }

    private void Submit() => MudDialog.Close(DialogResult.Ok(SelectedQuestionTypeId));

    private void Cancel() => MudDialog.Cancel();

    protected override async Task OnInitializedAsync()
    {
        QuestionTypes = new List<QuestionType>()
        {
            new() { Id = 1, Question = "Multiple Choice"},
            new() { Id = 2, Question = "True False"},
            new() { Id = 3, Question = "Text"},
            new() { Id = 4, Question = "Percentage"},
        };

        SelectedQuestionTypeId = 1;
    }

    public void onOptionSelect(int id)
    {
        SelectedQuestionTypeId = id;

        StateHasChanged();
    }

    public class QuestionType
    {
        public int Id { get; set; }

        public string Question { get; set; }
    }
}
