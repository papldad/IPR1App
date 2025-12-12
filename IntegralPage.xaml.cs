using System.Threading;

namespace IPR1App;

public partial class IntegralPage : ContentPage
{
    private CancellationTokenSource? _cts;

    public IntegralPage()
    {
        InitializeComponent();
    }

    private async void OnStartClicked(object sender, EventArgs e)
    {
        StartButton.IsEnabled = false;
        CancelButton.IsEnabled = true;
        StatusLabel.Text = "Вычисление";
        ProgressBar.Progress = 0;

        _cts = new CancellationTokenSource();

        try
        {
            double result = await Task.Run(() => ComputeIntegral(_cts.Token), _cts.Token);
            StatusLabel.Text = $"Результат: {result:F8}";
        }
        catch (OperationCanceledException)
        {
            StatusLabel.Text = "Задание отменено";
        }
        finally
        {
            StartButton.IsEnabled = true;
            CancelButton.IsEnabled = false;
            _cts?.Dispose();
            _cts = null;
        }
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        _cts?.Cancel();
    }

    private double ComputeIntegral(CancellationToken token)
    {
        const double a = 0.0;
        const double b = 1.0;
        const double h = 0.00000001;
        long totalSteps = (long)((b - a) / h);
        double sum = 0.0;

        for (long i = 0; i < totalSteps; i++)
        {
            if (token.IsCancellationRequested)
                throw new OperationCanceledException();

            // Метод прямоугольников: левый конец
            double x = a + i * h;
            double y = Math.Sin(x);
            sum += y * h;

            // Имитация нагрузки
            for (int j = 0; j < 100000; j++)
            {
                _ = j * j;
            }

            if (i % (totalSteps / 100) == 0)
            {
                double progress = (double)i / totalSteps;
                string percentText = $"{progress:P0}";

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    ProgressBar.Progress = progress;
                    StatusLabel.Text = $"Вычисление ({percentText})";
                });
            }
        }

        return sum;
    }
}