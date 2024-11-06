namespace NeedAnalysisApp.Client.Pages.Assessments;

public partial class AddEditLabelDialog
{
    #region Fields

    [Parameter] public QuestionDto? Label { get; set; }

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

    #endregion

    #region Methods

    protected override async Task OnInitializedAsync()
    {
        if (Label == null)
        {
            Label = new QuestionDto() { GeneralLookUp_QuestionTypeId = 5, Value = "Add your label here" };
        }
    }

    private void Submit() => MudDialog.Close(DialogResult.Ok(Label));

    private void Cancel() => MudDialog.Cancel();

    #endregion
}
