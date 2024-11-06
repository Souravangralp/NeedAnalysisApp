namespace NeedAnalysisApp.Client.Repositories.Services;

public class FilesClientService : IFilesClientService
{
    #region Fields

    private readonly HttpClient _httpClient;

    #endregion

    #region Ctor

    public FilesClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    #endregion

    #region Methods

    public async Task<FileDto> Upload(IBrowserFile browserFile)
    {
        using var form = new MultipartFormDataContent();

        using var fileStream = browserFile.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024); // 10 MB limit

        var streamContent = new StreamContent(fileStream)
        {
            Headers = { ContentType = new MediaTypeHeaderValue(browserFile.ContentType) }
        };

        form.Add(streamContent, "file", browserFile.Name); // "file" is the form field name


        //var response = await _httpClient.PostAsync("https://localhost:7028/api/files", form);
        var response = await _httpClient.PostAsync(Shared.Common.Constants.Routes.File.Create, form);


        if (response.IsSuccessStatusCode)
        {

            return await response.Content.ReadFromJsonAsync<FileDto>();
        }
        else
        {

            throw new HttpRequestException($"File upload failed. Status code: {response.StatusCode}");
        }
    }

    public Task<FileDto> Get(string fileId)
    {
        throw new NotImplementedException();
    }

    #endregion
}
