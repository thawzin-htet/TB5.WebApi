using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;

namespace TB5.ConsoleApp;

public class BirdApiService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://fake-brids-apis.vercel.app/api/v1/birds";

    public BirdApiService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<List<BirdModel>> Read()
    {
        var response = await _httpClient.GetAsync(BaseUrl);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<BirdModel>>(content) ?? new List<BirdModel>();
    }

    public async Task<BirdModel?> Read(int id)
    {
        var response = await _httpClient.GetAsync($"{BaseUrl}/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<BirdModel>(content);
    }

    public async Task Create(BirdModel bird)
    {
        var response = await _httpClient.PostAsJsonAsync(BaseUrl, bird);
        response.EnsureSuccessStatusCode();
        Console.WriteLine("Create successful.");
    }

    public async Task Update(int id, BirdModel bird)
    {
        var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", bird);
        response.EnsureSuccessStatusCode();
        Console.WriteLine("Update successful.");
    }

    public async Task Patch(int id, BirdModel bird)
    {
        // For Patch, we typically only send changed fields. 
        // Since we're using a fake API, we'll just send the model as a simple demonstration.
        // Usually you'd use a dynamic object or a specific DTO for partial updates.
        var response = await _httpClient.PatchAsJsonAsync($"{BaseUrl}/{id}", bird);
        response.EnsureSuccessStatusCode();
        Console.WriteLine("Patch successful.");
    }

    public async Task Delete(int id)
    {
        var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
        response.EnsureSuccessStatusCode();
        Console.WriteLine("Delete successful.");
    }
}
