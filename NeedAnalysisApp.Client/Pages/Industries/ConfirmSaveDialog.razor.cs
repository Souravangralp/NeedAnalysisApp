namespace NeedAnalysisApp.Client.Pages.Industries;

public partial class ConfirmSaveDialog
{
    #region Fields

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

    [Parameter] public string ContentText { get; set; } = "";
    
    #endregion

    #region Methods

    private void UpdateRange() => MudDialog.Close(DialogResult.Ok(true));

    private void Cancel() => MudDialog.Cancel();

    #endregion
}
