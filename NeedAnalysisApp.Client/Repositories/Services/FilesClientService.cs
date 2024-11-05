using Microsoft.AspNetCore.Components.Forms;
using NeedAnalysisApp.Shared.Dto.Chat;
using System.Net.Http.Headers;

namespace NeedAnalysisApp.Client.Repositories.Services;

public class FilesClientService : IFilesClientService
{
    private readonly HttpClient _httpClient;

    public FilesClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public Task<FileDto> Get(string fileId)
    {
        throw new NotImplementedException();
    }

    public async Task<FileDto> Upload(IBrowserFile browserFile)
    {
        // Create a form to send the file
        using var form = new MultipartFormDataContent();

        // Open a stream to read the file
        using var fileStream = browserFile.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024); // 10 MB limit
        var streamContent = new StreamContent(fileStream)
        {
            Headers = { ContentType = new MediaTypeHeaderValue(browserFile.ContentType) }
        };

        // Add the file content to the form
        form.Add(streamContent, "file", browserFile.Name); // "file" is the form field name

        // Send the POST request to the server
        var response = await _httpClient.PostAsync("https://localhost:7028/api/files", form);

        // Check the response status
        if (response.IsSuccessStatusCode)
        {
            // Deserialize the response content to FileDto
            return await response.Content.ReadFromJsonAsync<FileDto>();
        }
        else
        {
            // Handle error (you can throw an exception or return null, based on your needs)
            throw new HttpRequestException($"File upload failed. Status code: {response.StatusCode}");
        }
    }

    //public Task<FileDto> Upload(IBrowserFile browserFile)
    //{
    //    throw new NotImplementedException();
    //}
}
