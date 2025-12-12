using IPR1App.Models;

namespace IPR1App.Services;

public interface IRateService
{
    Task<IEnumerable<Rate>> GetRatesAsync(DateTime date);
}