namespace NeedAnalysisApp.Client.Pages.Industries;

public partial class Home
{
    #region Fields

    [Inject] IIndustryClientService IIndustryClientService { get; set; } = null!;

    [Inject] IDialogService DialogService { get; set; } = null!;

    [Inject] ISnackbar ISnackBar { get; set; } = null!;

    public bool IsLoading { get; set; } = true;

    public bool? IncludeInActive { get; set; } = null;

    private bool dense = true;

    private bool hover = true;

    private bool bordered = true;

    private string searchedValue = "";

    private IndustryDto selectedIndustry = null;

    private HashSet<IndustryDto> selectedItems = [];

    private List<IndustryDto> Industries = [];

    #endregion

    #region Methods

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;

        var industries = await IIndustryClientService.GetAllAsync();

        Industries = industries.Where(x => x.IsActive == true).ToList();

        IncludeInActive = true;

        IsLoading = false;

        StateHasChanged();
    }

    private async void OnChangeIncludeActiveAsync(bool? value)
    {
        IncludeInActive = value;

        List<IndustryDto> industries = [];

        industries = await IIndustryClientService.GetAllAsync();

        IsLoading = true;

        if (IncludeInActive == null)
        {
            Industries = industries;
        }
        if (IncludeInActive == true)
        {
            Industries = industries.Where(x => x.IsActive == true).ToList();
        }
        if (IncludeInActive == false)
        {
            Industries = industries.Where(x => x.IsActive == false).ToList();
        }

        IsLoading = false;

        StateHasChanged();
    }

    private bool FilterFunc1(IndustryDto element) => FilterFunc(element, searchedValue);

    private static bool FilterFunc(IndustryDto element, string searchString)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.Code.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if ($"{element.Code} {element.Name} {element.Description}".Contains(searchString))
            return true;
        return false;
    }

    public async void AddIndustryAsync()
    {
        var parameters = new DialogParameters<AddEdit>
        {
            { x => x.Model, null }
        };

        var options = new DialogOptions() { CloseButton = true, FullWidth = true, MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<AddEdit>("Add new", parameters, options);

        var result = await dialog.Result;

        Industries = await IIndustryClientService.GetAllAsync();

        StateHasChanged();
    }

    public async void OnEditAsync(IndustryDto industry)
    {
        var parameters = new DialogParameters<AddEdit>
        {
            { x => x.Model, industry }
        };

        var options = new DialogOptions() { CloseButton = true, FullWidth = true, MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<AddEdit>($"edit :{industry.Name}", parameters, options);

        var result = await dialog.Result;

        Industries = await IIndustryClientService.GetAllAsync();

        StateHasChanged();
    }

    public async void OnDeleteAsync(IndustryDto industry)
    {
        var parameters = new DialogParameters<ConfirmDeleteDialog>
        {
            { x => x.ContentText, $"Do you really want to delete {industry.Name}? This process cannot be undone." },
            { x => x.ButtonText, "Delete" },
            { x => x.Color, Color.Error }
        };

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Large };

        var dialog = await DialogService.ShowAsync<ConfirmDeleteDialog>("Delete", parameters, options);

        var result = await dialog.Result;

        if (!result.Canceled)
        {
            var actionResult = await IIndustryClientService.DeleteAsync(industry.UniqueId);

            if (!actionResult.Success && actionResult.Errors.Any())
            {
                foreach (var error in actionResult.Errors)
                {
                    ISnackBar.Add(error.Message, Severity.Error);
                }
            }
            else
            {
                ISnackBar.Add("Industry Deleted", Severity.Success);
            }

            Industries = await IIndustryClientService.GetAllAsync();

            StateHasChanged();
        }
    }

    #endregion
}