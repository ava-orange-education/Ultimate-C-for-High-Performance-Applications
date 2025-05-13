await GetHttpResponse();

static async Task GetHttpResponse()
{
    using var httpClient = new HttpClient();
    HttpResponseMessage result =
        await httpClient.GetAsync("https://httpbin.org/get");
    var content = await result.Content.ReadAsStringAsync();
    Console.WriteLine(content);
}