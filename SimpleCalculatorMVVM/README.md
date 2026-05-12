# SimpleCalculatorMVVM - Калькулятор с архитектурой MVVM

![Язык](https://img.shields.io/badge/Язык-C%23-blue)
![Фреймворк](https://img.shields.io/badge/Фреймворк-WPF-blue)
![.NET](https://img.shields.io/badge/.NET-8.0-blue)

Простой, но функциональный калькулятор, разработанный с использованием архитектурного паттерна **MVVM (Model-View-ViewModel)**. Проект демонстрирует правильное разделение ответственности и лучшие практики в разработке WPF приложений.

## 🎯 Цель проекта

Изучение и применение архитектурных паттернов проектирования:
- **MVVM (Model-View-ViewModel)** - разделение представления от бизнес-логики
- **Factory Pattern** - фабричный метод для создания объектов
- **Command Pattern** - реализация команд для взаимодействия пользователя с UI
- **Data Binding** - привязка данных в WPF

## ✨ Особенности

### Функциональность калькулятора
- ✅ Базовые арифметические операции: `+`, `-`, `*`, `/`
- ✅ Операция процента: `%`
- ✅ Смена знака числа: `±`
- ✅ Ввод десятичных чисел: `.`
- ✅ Очистка дисплея: `C`
- ✅ Вычисление результата: `=`
- ✅ Обработка ошибок (деление на ноль)
- ✅ Отображение выражения в реальном времени

### Архитектурные особенности
- 🏗️ **Чистая архитектура MVVM** - полное разделение логики и UI
- 🎨 **Современный дизайн** - темная тема с миними интерфейсом
- 🔄 **Data Binding** - автоматическое обновление UI при изменении данных
- 📦 **Модульная структура** - легко расширяемо и тестируемо

## 📁 Структура проекта

```
SimpleCalculatorMVVM/
├── App.xaml                           # Конфигурация приложения
├── App.xaml.cs                        # Code-behind для App
├── AssemblyInfo.cs                    # Информация о сборке
├── SimpleCalculatorMVVM.csproj        # Файл проекта
├── ARCHITECTURE.md                    # Документация архитектуры
├── README.md                          # Этот файл
│
├── Commands/
│   └── RelayCommand.cs               # Реализация ICommand
│
├── Models/
│   └── CalculatorModel.cs            # Модель калькулятора
│
├── ViewModels/
│   ├── ViewModelBase.cs              # Базовый класс для ViewModel
│   └── CalculatorViewModel.cs        # ViewModel калькулятора
│
└── Views/
	├── MainWindow.xaml               # Интерфейс приложения
	└── MainWindow.xaml.cs            # Code-behind для View
```

## 🏛️ Архитектура

Проект следует классической архитектуре MVVM с четкими слоями:

### Model (Models/)
- Содержит чистую бизнес-логику
- Не зависит от UI
- Отвечает за математические операции

### View (Views/)
- Определяет пользовательский интерфейс (XAML)
- Привязывается к ViewModel через DataContext
- Не содержит бизнес-логику

### ViewModel (ViewModels/)
- Промежуточный слой между View и Model
- Реализует команды (ICommand) для взаимодействия
- Реализует INotifyPropertyChanged для уведомления об изменениях

### Commands (Commands/)
- RelayCommand - реализация ICommand
- Позволяет вызывать методы ViewModel из XAML

## 🚀 Начало работы

### Требования
- .NET 8.0 или выше
- Visual Studio 2022+ или Visual Studio Code
- C# 11+

### Установка и запуск

1. **Клонирование репозитория**
```bash
git clone https://github.com/Vetrikc/calculator-C-.git
cd calculator-C-
```

2. **Открытие решения**
```bash
# Через Visual Studio
start SimpleCalculator.sln

# Или через командную строку
dotnet open SimpleCalculator.sln
```

3. **Запуск приложения**
```bash
# Через Visual Studio - нажмите F5

# Или через командную строку
dotnet run --project SimpleCalculatorMVVM
```

## 📖 Использование

1. **Введите число** - кликните на кнопки с цифрами или используйте клавиатуру
2. **Выберите операцию** - нажмите на одну из кнопок: `+`, `-`, `*`, `/`
3. **Введите второе число**
4. **Нажмите равно** - получите результат

### Примеры операций

| Операция | Результат |
|----------|-----------|
| 5 + 3 = | 8 |
| 10 - 2 = | 8 |
| 4 * 5 = | 20 |
| 20 / 4 = | 5 |
| 50 % | 0.5 |
| -5 ± | 5 |

## 🎨 Дизайн

### Цветовая схема
```
Основной фон:     #0F0F0F (черный)
Фон дисплея:      #1A1A1A (темно-серый)
Кнопки цифр:      #222 (фон), #F0F0F0 (текст)
Кнопки операций:  #FF6B35 (оранжево-красный)
Кнопка равно:     #00C853 (зеленый)
Кнопки служебные: #333 (фон), #00E5FF (голубой)
```

### Интерфейс
- Размер окна: 300x460 пикселей (зафиксирован)
- Шрифт: Segoe UI
- Стиль кнопок: круглые с эффектами наведения

## 📚 Паттерны проектирования

### 1. MVVM (Model-View-ViewModel)
Основной паттерн архитектуры приложения

### 2. Command Pattern
Реализация через `ICommand` и `RelayCommand` для привязки действий к кнопкам

### 3. Observer Pattern
Реализация через `INotifyPropertyChanged` для уведомления об изменениях

### 4. Factory Pattern
Использовано в `SimpleCalculatorFactory` проекте для создания кнопок

## 🧪 Тестирование

Бизнес-логика в Model слое может легко тестироваться:

```csharp
// Пример теста
[TestClass]
public class CalculatorModelTests
{
	[TestMethod]
	public void Calculate_Addition_ReturnsCorrectResult()
	{
		var model = new CalculatorModel();
		double result = model.Calculate(5, 3, "+");
		Assert.AreEqual(8, result);
	}
}
```

## 📝 Примеры кода

### Привязка данных в XAML
```xaml
<TextBlock Text="{Binding Display, Mode=OneWay}" />
```

### Привязка команды в XAML
```xaml
<Button Command="{Binding DigitCommand}" CommandParameter="5" />
```

### Реализация команды в ViewModel
```csharp
private void OnDigit(object? parameter)
{
	if (parameter is string digit)
		AppendDigit(digit);
}
```

## 🔄 Данные о версиях

| Версия | Дата | Описание |
|--------|------|---------|
| 1.0.0 | 2026-05-12 | Начальный релиз MVVM калькулятора |

## 🤝 Содействие

Приветствуются предложения по улучшению! Пожалуйста:

1. Форкните репозиторий
2. Создайте ветку для вашей функции (`git checkout -b feature/AmazingFeature`)
3. Внесите изменения (`git commit -m 'Add some AmazingFeature'`)
4. Загрузите ветку (`git push origin feature/AmazingFeature`)
5. Откройте Pull Request

## 📄 Лицензия

Этот проект распространяется под лицензией MIT. Смотрите файл `LICENSE` для более подробной информации.

## 📞 Контакты

- 👤 **Автор**: Vetrikc
- 🔗 **GitHub**: [Vetrikc](https://github.com/Vetrikc)
- 📧 **Email**: [your-email@example.com]

## 🙏 Благодарности

- Спасибо сообществу C# и WPF за отличные инструменты и примеры
- Вдохновение от современных приложений калькулятора

## 📚 Дополнительные ресурсы

- [MVVM на MSDN](https://docs.microsoft.com/en-us/archive/msdn-magazine/2009/february/patterns-wpf-apps-with-the-model-view-viewmodel-design-pattern)
- [WPF Data Binding](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/data/)
- [Command Pattern](https://refactoring.guru/design-patterns/command)
- [Factory Pattern](https://refactoring.guru/design-patterns/factory-method)

---

**Последнее обновление**: 12 мая 2026
