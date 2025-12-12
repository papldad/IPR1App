## Описание

Проект реализует два задания*:

1. **Вычисление интеграла**  
   - Функция: `y = sin(x)` на отрезке `[0, 1]`  
   - Метод: прямоугольников с шагом `0.00000001`  
   - Асинхронное вычисление с прогрессом и возможностью отмены

2. **Конвертер валют**  
   - Использует официальный API Национального банка РБ: `https://api.nbrb.by/`  
   - Поддерживаемые валюты: USD, EUR, RUB, CHF, CNY, GBP  
   - Двусторонняя конвертация: BYN ↔ выбранная валюта  
   - Выбор даты и валюты через UI

## Стек технологий

- .NET MAUI
- C#, XAML
- HttpClient, Dependency Injection
- Асинхронность (`async`/`await`, `CancellationToken`)

## Сборка и запуск

1. Установить [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
2. Открыть решение в **Visual Studio 2022**
3. Нажать **F5** для запуска на Windows (или выбрать другую платформу)

## Скриншоты
![1](https://github.com/user-attachments/assets/30ba5ded-ae62-4165-ae60-35abe955997e)
![2](https://github.com/user-attachments/assets/5bb5bb83-08af-4e3a-88f5-9c0d4563a584)
