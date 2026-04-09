using RestSharp;
using Newtonsoft.Json;

namespace TB5.ConsoleApp;

public class RestBirdApiService
{
    private readonly RestClient _client;
    private const string BaseUrl = "https://fake-brids-apis.vercel.app/api/v1/birds";

    public RestBirdApiService()
    {
        _client = new RestClient(BaseUrl);
    }

    public async Task<List<BirdModel>> Read()
    {
        var request = new RestRequest();
        var response = await _client.ExecuteGetAsync(request);
        if (!response.IsSuccessful)
            throw new Exception($"Error fetching birds: {response.ErrorMessage}");

        return JsonConvert.DeserializeObject<List<BirdModel>>(response.Content!) ?? new List<BirdModel>();
    }

    public async Task<BirdModel?> Read(int id)
    {
        var request = new RestRequest("{id}").AddUrlSegment("id", id);
        var response = await _client.ExecuteGetAsync(request);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;

        if (!response.IsSuccessful)
            throw new Exception($"Error fetching bird {id}: {response.ErrorMessage}");

        return JsonConvert.DeserializeObject<BirdModel>(response.Content!);
    }

    public async Task Create(BirdModel bird)
    {
        var request = new RestRequest().AddJsonBody(bird);
        var response = await _client.ExecutePostAsync(request);
        if (!response.IsSuccessful)
            throw new Exception($"Error creating bird: {response.ErrorMessage}");

        Console.WriteLine("Create successful.");
    }

    public async Task Update(int id, BirdModel bird)
    {
        var request = new RestRequest("{id}").AddUrlSegment("id", id).AddJsonBody(bird);
        var response = await _client.ExecutePutAsync(request);
        if (!response.IsSuccessful)
            throw new Exception($"Error updating bird {id}: {response.ErrorMessage}");

        Console.WriteLine("Update successful.");
    }

    public async Task Patch(int id, BirdModel bird)
    {
        var request = new RestRequest("{id}").AddUrlSegment("id", id).AddJsonBody(bird);
        var response = await _client.ExecuteAsync(request, Method.Patch);
        if (!response.IsSuccessful)
            throw new Exception($"Error patching bird {id}: {response.ErrorMessage}");

        Console.WriteLine("Patch successful.");
    }

    public async Task Delete(int id)
    {
        var request = new RestRequest("{id}").AddUrlSegment("id", id);
        var response = await _client.ExecuteAsync(request, Method.Delete);
        if (!response.IsSuccessful)
            throw new Exception($"Error deleting bird {id}: {response.ErrorMessage}");

        Console.WriteLine("Delete successful.");
    }
}
