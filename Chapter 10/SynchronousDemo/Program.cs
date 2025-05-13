GetHttpResponse();

static void GetHttpResponse()
{
    var httpClient = new HttpClient();
    HttpResponseMessage result =
        httpClient.GetAsync("https://httpbin.org/get").Result;
    var content = result.Content.ReadAsStringAsync().Result;
    Console.WriteLine(content);
}