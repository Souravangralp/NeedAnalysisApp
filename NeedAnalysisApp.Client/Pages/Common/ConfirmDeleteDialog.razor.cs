namespace NeedAnalysisApp.Client.Pages.Common;

public partial class ConfirmDeleteDialog
{
    #region Fields

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

    [Parameter] public string ContentText { get; set; }

    [Parameter] public string ButtonText { get; set; }

    [Parameter] public Color Color { get; set; }

    #endregion

    #region Methods

    private void Submit() => MudDialog.Close(DialogResult.Ok(true));

    private void Cancel() => MudDialog.Cancel();

    #endregion
}