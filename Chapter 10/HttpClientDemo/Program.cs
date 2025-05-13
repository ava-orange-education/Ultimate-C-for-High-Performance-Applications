using var httpClient = new HttpClient();
var result = await httpClient.GetStringAsync("https://httpbin.org/get");
Console.WriteLine(result);
