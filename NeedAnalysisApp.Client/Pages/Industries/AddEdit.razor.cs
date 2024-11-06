namespace NeedAnalysisApp.Client.Pages.Industries;

public partial class AddEdit
{
    #region Fields

    [Parameter] public IndustryDto? Model { get; set; }

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;

    [Inject] IIndustryClientService industryClientService { get; set; } = null!;

    [Inject] ISnackbar SnackBar { get; set; } = null!;

    #endregion

    #region Methods

    protected override async Task OnInitializedAsync()
    {
        if (Model is null)
        {
            MudDialog.SetTitle("Add new industry");

            Model = new IndustryDto() { Name = "", Code = "", Description = "", IsActive = true };
        }
        else
        {
            MudDialog.SetTitle($"Edit : {Model.Name}");
        }

        StateHasChanged();
    }

    private async void Save()
    {
        var isNew = string.IsNullOrWhiteSpace(Model.UniqueId);
        var result = isNew
            ? await industryClientService.CreateAsync(Model)
            : await industryClientService.UpdateAsync(Model);

        HandleResult(result, isNew);
    }

    private void HandleResult(Result result, bool isNew)
    {
        if (!result.Success && result.Errors.Any())
        {
            foreach (var error in result.Errors)
            {
                SnackBar.Add(error.Message, Severity.Error);
            }
        }
        else
        {
            SnackBar.Add(isNew ? "Industry Added" : "Industry Updated", Severity.Success);

            MudDialog.Close();
        }
    }

    private void Cancel() => MudDialog.Cancel();

    #endregion
}
