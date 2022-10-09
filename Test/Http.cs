using System;
using System.Net.Http;

namespace MyCollection.Test;

public class Http
{
    public static HttpClient GetHttpClient(string userAgent, string baseUrl)
    {
        var httpClient = new HttpClient { BaseAddress = new Uri(baseUrl)};
        httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
        return httpClient;
    }
    
}


public static class Parser
{
    public static string ParseToUrlFormat(this string str) => str.Replace(" ", "+");
}