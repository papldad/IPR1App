using IPR1App.Models;
using IPR1App.Services;

namespace IPR1App;

public partial class CurrencyConverterPage : ContentPage
{
    private readonly IRateService _rateService;
    private List<Rate> _rates = new();
    private Rate? _selectedRate;
    private bool _isUpdating = false;

    public CurrencyConverterPage(IRateService rateService)
    {
        InitializeComponent();
        _rateService = rateService;
        DatePicker.Date = DateTime.Today;
        LoadRatesAsync(DateTime.Today);
    }

    private async void OnDateSelected(object sender, DateChangedEventArgs e)
    {
        await LoadRatesAsync(e.NewDate);
    }

    private async Task LoadRatesAsync(DateTime date)
    {
        try
        {
            _rates = new List<Rate>(await _rateService.GetRatesAsync(date));
            CurrencyPicker.ItemsSource = _rates.Select(r => r.Cur_Name).ToList();
            if (CurrencyPicker.ItemsSource.Count > 0)
            {
                CurrencyPicker.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", $"Не удалось загрузить курсы: {ex.Message}", "OK");
            _rates.Clear();
            CurrencyPicker.ItemsSource = null;
        }
    }

    private void OnCurrencySelected(object sender, EventArgs e)
    {
        if (CurrencyPicker.SelectedIndex >= 0 && CurrencyPicker.SelectedIndex < _rates.Count)
        {
            _selectedRate = _rates[CurrencyPicker.SelectedIndex];
            UpdateRateLabel();
            // Сброс полей
            SetBynText("");
            SetForeignText("");
        }
    }

    private void OnBynTextChanged(object sender, TextChangedEventArgs e)
    {
        if (_isUpdating || _selectedRate == null) return;

        if (decimal.TryParse(e.NewTextValue, out decimal byn))
        {
            decimal foreign = (byn / _selectedRate.Cur_OfficialRate) * _selectedRate.Cur_Scale;
            SetForeignText(foreign.ToString("F4"));
        }
        else if (string.IsNullOrEmpty(e.NewTextValue))
        {
            SetForeignText("");
        }
    }

    private void OnForeignTextChanged(object sender, TextChangedEventArgs e)
    {
        if (_isUpdating || _selectedRate == null) return;

        if (decimal.TryParse(e.NewTextValue, out decimal foreign))
        {
            decimal byn = (foreign / _selectedRate.Cur_Scale) * _selectedRate.Cur_OfficialRate;
            SetBynText(byn.ToString("F4"));
        }
        else if (string.IsNullOrEmpty(e.NewTextValue))
        {
            SetBynText("");
        }
    }

    private void SetBynText(string text)
    {
        _isUpdating = true;
        BynEntry.Text = text;
        _isUpdating = false;
    }

    private void SetForeignText(string text)
    {
        _isUpdating = true;
        ForeignEntry.Text = text;
        _isUpdating = false;
    }

    private void UpdateRateLabel()
    {
        if (_selectedRate != null)
        {
            RateLabel.Text = $"Курс: {_selectedRate.Cur_OfficialRate} BYN за {_selectedRate.Cur_Scale} {_selectedRate.Cur_Abbreviation}";
        }
    }
}