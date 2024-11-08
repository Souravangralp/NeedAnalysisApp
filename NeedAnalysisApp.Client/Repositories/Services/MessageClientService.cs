namespace NeedAnalysisApp.Client.Repositories.Services;

public class MessageClientService : IMessageClientService
{
    #region     Fields

    private readonly HttpClient _httpClient;

    #endregion

    #region Ctor

    public MessageClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    #endregion

    #region Methods

    public async Task<List<MessageDto>> GetAll(string senderId, string receiverId)
    {
        //var response = await _httpClient.GetAsync($"https://localhost:7028/api/messages?senderId={senderId}&receiverId={receiverId}");
        var response = await _httpClient.GetAsync(string.Format(Message.GetAll, senderId, receiverId));

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
        //var response = await _httpClient.PostAsJsonAsync("https://localhost:7028/api/messages", message);
        var response = await _httpClient.PostAsJsonAsync(Message.Send, message);

        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            throw new HttpRequestException($"Failed to send Messages. Status code: {response.StatusCode}");
        }
    }

    public async Task<bool> MarkMessageRead(MessageDto message)
    {
        if (string.IsNullOrWhiteSpace(message.UniqueId))
        {
            return false;
        }

        //var response = await _httpClient.PostAsync($"https://localhost:7028/api/messages/{messageId}/markRead/{receiverId}", null);
        var response = await _httpClient.PostAsync(string.Format(Message.MarkRead, message.UniqueId, message.SenderId, message.ReceiverId), null);

        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            throw new HttpRequestException($"Failed to send Messages. Status code: {response.StatusCode}");
        }
    }

    #endregion
}