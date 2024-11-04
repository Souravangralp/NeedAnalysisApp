using NeedAnalysisApp.Shared.Dto.Chat;

namespace NeedAnalysisApp.Client.Repositories.Services;

public class MessageClientService : IMessageClientService
{
    private readonly HttpClient _httpClient;

    public MessageClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<MessageDto>> GetAll(string senderId, string receiverId)
    {
        var response = await _httpClient.GetAsync($"https://localhost:7028/api/messages?senderId={senderId}&receiverId={receiverId}");

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<List<MessageDto>>();

            return result ?? [];
            //return JsonConvert.DeserializeObject<List<MessageDto>>(result.Model.ToString() ?? string.Empty) ?? [];
        }
        else
        {
            throw new HttpRequestException($"Failed to retrieve industries. Status code: {response.StatusCode}");
        }
    }

    public async Task<bool> Send(MessageDto message)
    {
        var response = await _httpClient.PostAsJsonAsync("https://localhost:7028/api/messages", message);

        if (response.IsSuccessStatusCode)
        {
            return true; //await response.Content.ReadFromJsonAsync<Result>(); // Deserializing the response to 'Result' object
            //return result; // Return or use the result as needed
        }
        else
        {
            throw new HttpRequestException($"Failed to send Messages. Status code: {response.StatusCode}");
        }
    }
}

