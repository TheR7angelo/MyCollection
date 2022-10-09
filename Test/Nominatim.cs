using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using GMap.NET;
using Newtonsoft.Json;

namespace MyCollection.Test;

public partial class Nominatim : Http
{
    private static HttpClient HttpClient { get; } = GetHttpClient("C#", "https://nominatim.openstreetmap.org");
    
    public static PointLatLng AddressToPoint(string address) => _AddressToPoint(address).Result;
    public static string? PointToAddress(PointLatLng point) => _PointToAddress(point).Result;
    
    private static async Task<PointLatLng> _AddressToPoint(string address)
    {
        var r = new List<NominatimStruc>{ new() };
        try
        {
            var httpResult = await HttpClient.GetAsync($"search?q={address.ParseToUrlFormat()}&format=json&polygon=1&addressdetails=1").ConfigureAwait(false);
            var result = await httpResult.Content.ReadAsStringAsync();
        
            r = JsonConvert.DeserializeObject<List<NominatimStruc>>(result);
        }
        catch (Exception)
        {
            // ignored
        }

        return new PointLatLng { Lat = r![0].lat, Lng = r[0].lon };
    }

    private static async Task<string?> _PointToAddress(PointLatLng point)
    {
        var r = new NominatimStruc();
        try
        {
            var httpResult = await HttpClient.GetAsync($"reverse?format=json&lat={point.Lat.ToString(CultureInfo.InvariantCulture)}&lon={point.Lng.ToString(CultureInfo.InvariantCulture)}").ConfigureAwait(false);
            var result = await httpResult.Content.ReadAsStringAsync();
            r = JsonConvert.DeserializeObject<NominatimStruc>(result);
        }
        catch (Exception)
        {
            // ignored
        }

        return r.display_name;
    }
}