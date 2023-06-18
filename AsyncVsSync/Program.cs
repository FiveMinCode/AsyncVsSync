using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Text;
using System.Text.RegularExpressions;

//var demo = new AsyncVsSyncDemo();
//var data1 = demo.GetDotNetCountSync();
//Console.WriteLine("Result from sync method: " + data1);
//var data2 = demo.GetDotNetCountAsync();
//Console.WriteLine("Result from Async method: " + data2.Result);

var summary = BenchmarkRunner.Run<AsyncVsSyncDemo>();

[RankColumn]
[MemoryDiagnoser]
public class AsyncVsSyncDemo
{
    private readonly HttpClient _httpClient = new HttpClient();
    [Benchmark]
    public async Task<int> GetDotNetCountAsync()
    {
        // Suspends GetDotNetCount() to allow the caller (the web server)
        // to accept another request, rather than blocking on this one.
        var html = await _httpClient.GetStringAsync("https://dotnetfoundation.org");

        return Regex.Matches(html, @"\.NET").Count;
    }
    [Benchmark]
    public int GetDotNetCountSync()
    {
        var html = _httpClient.GetStringAsync("https://dotnetfoundation.org");

        return Regex.Matches(html.Result, @"\.NET").Count;
    }
}