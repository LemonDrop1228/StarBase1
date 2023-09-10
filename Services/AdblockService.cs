using System.Net.Http;
using Microsoft.Extensions.Options;
using StarBase1.Options;

namespace StarBase1.Services;

public class AdblockService
{
    private List<string> _blockedDomains = new List<string>();
    private HttpClient _httpClient = new HttpClient();
    private readonly string _filterListUrl;
    
    public AdblockService(IOptions<AdblockOptions> options)
    {
        _filterListUrl = options.Value.FilterListUrl;
    }
    
    public async Task Initialize()
    {
        try
        {
            var response = await _httpClient.GetAsync(_filterListUrl);
            response.EnsureSuccessStatusCode();
            var filterList = await response.Content.ReadAsStringAsync();
            
            LoadFilterList(filterList);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load filter list: {ex.Message}");
        }
    }

    private void LoadFilterList(string filterList)
    {
        _blockedDomains.Clear();
        var lines = filterList.Split('\n');
        foreach (var line in lines)
        {
            if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith('!') && line.Contains('^'))
            {
                var domain = line.TrimStart('|').Split('^').FirstOrDefault();
                if (!string.IsNullOrEmpty(domain))
                {
                    _blockedDomains.Add(domain);
                }
            }
        }
    }

    public bool ShouldBlock(string url)
    {
        if (_blockedDomains.Count == 0)
            return false;

        var uri = new Uri(url);
        return _blockedDomains.Any(domain => uri.Host.EndsWith(domain));
    }    
}
