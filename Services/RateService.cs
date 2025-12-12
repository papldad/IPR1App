using IPR1App.Models;
using System.Text.Json;

namespace IPR1App.Services;

public class RateService : IRateService
{
    private readonly HttpClient _httpClient;

    public RateService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.nbrb.by/");
    }

    public async Task<IEnumerable<Rate>> GetRatesAsync(DateTime date)
    {
        // Формат даты: yyyy-MM-dd (согласно документации)
        string dateString = date.ToString("yyyy-MM-dd");
        string url = $"exrates/rates?ondate={dateString}&periodicity=0";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();
        var rates = await JsonSerializer.DeserializeAsync<List<Rate>>(stream) ?? new List<Rate>();

        // Фильтруем только нужные валюты
        var targetCurrencies = new HashSet<string> { "USD", "EUR", "RUB", "CHF", "CNY", "GBP" };
        return rates.Where(r => targetCurrencies.Contains(r.Cur_Abbreviation));
    }
}