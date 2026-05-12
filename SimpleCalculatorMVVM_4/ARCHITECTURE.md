# SimpleCalculatorMVVM - Документация архитектуры

## Описание проекта
SimpleCalculatorMVVM - это реализация простого калькулятора с использованием архитектурного паттерна MVVM (Model-View-ViewModel). 
Дизайн инспирирован современными калькуляторами с темной темой и интуитивным интерфейсом.

## Архитектура MVVM

### Model (Модель) - папка Models/
**Файл: CalculatorModel.cs**
- Содержит бизнес-логику вычисления
- Отвечает за математические операции
- Не зависит от UI

Методы:
- `Calculate(double, double, string)` - выполняет математическую операцию
- `Reset()` - сброс данных калькулятора

Свойства:
- `FirstOperand` - первый операнд
- `SecondOperand` - второй операнд
- `CurrentOperator` - текущий оператор
- `Result` - результат вычисления

### View (Представление) - папка Views/
**Файлы: MainWindow.xaml, MainWindow.xaml.cs**
- Определяет пользовательский интерфейс (XAML)
- Привязывается к ViewModel через DataContext
- Не содержит бизнес-логику

Элементы UI:
- Border с дисплеем (выражение + результат)
- Grid с кнопками калькулятора (0-9, операторы, очистка)
- Стилизованные кнопки с эффектами наведения и нажатия

### ViewModel (Модель представления) - папка ViewModels/
**Файл: CalculatorViewModel.cs**
- Промежуточный слой между View и Model
- Реализует привязку данных (Data Binding)
- Содержит команды (ICommand) для взаимодействия с View

Свойства:
- `Display` - текущее значение на дисплее (привязано к View)

Команды:
- `DigitCommand` - ввод цифр
- `OperatorCommand` - выбор оператора
- `EqualsCommand` - вычисление результата
- `ClearCommand` - очистка
- `DotCommand` - ввод десятичной точки

**Файл: ViewModelBase.cs**
- Базовый класс для всех ViewModels
- Реализует INotifyPropertyChanged для уведомления об изменении свойств

### Commands - папка Commands/
**Файл: RelayCommand.cs**
- Реализация ICommand для привязки команд к кнопкам
- Позволяет вызывать методы ViewModel из View

## Дизайн и стили

### Цветовая схема
- **Основной фон**: #0F0F0F (почти черный)
- **Фон дисплея**: #1A1A1A (темно-серый)
- **Цифры**: #222 (фон), #F0F0F0 (текст)
- **Операторы**: #FF6B35 (оранжево-красный)
- **Равно**: #00C853 (зеленый)
- **Служебные**: #333 (фон), #00E5FF (голубой текст)

### Стили кнопок
- **CornerRadius="50"** - полностью круглые кнопки
- **Transition эффекты**: наведение (Opacity 0.75), нажатие (Opacity 0.5)
- **Font**: Segoe UI, 17pt

### Дисплей
- Двухстрочный дисплей (выражение + результат)
- FontSize: 13pt для выражения, 38pt для результата
- TextTrimming: CharacterEllipsis (обрезка текста при необходимости)

## Диаграмма классов

```
┌─────────────────────────────────────────────────────────┐
│                       View Layer                        │
│  ┌────────────────────────────────────────────────────┐ │
│  │            MainWindow (View)                       │ │
│  │  - Определяет UI элементы (XAML)                   │ │
│  │  - DataContext = CalculatorViewModel               │ │
│  │  - Display TextBlock (привязка двусторонняя)       │ │
│  │  - Кнопки с Command привязкой                      │ │
│  └──────────────────────┬─────────────────────────────┘ │
└─────────────────────────┼────────────────────────────────┘
						  │ Binding
						  ↓
┌─────────────────────────────────────────────────────────┐
│                  ViewModel Layer                        │
│  ┌────────────────────────────────────────────────────┐ │
│  │         CalculatorViewModel                       │ │
│  │  - Display : string (INotifyPropertyChanged)       │ │
│  │  - _newInput : bool                                │ │
│  │  - _model : CalculatorModel                        │ │
│  │                                                    │ │
│  │  Команды:                                          │ │
│  │  - DigitCommand : ICommand                         │ │
│  │  - OperatorCommand : ICommand                      │ │
│  │  - EqualsCommand : ICommand                        │ │
│  │  - ClearCommand : ICommand                         │ │
│  │  - DotCommand : ICommand                           │ │
│  │                                                    │ │
│  │  Методы:                                           │ │
│  │  - OnDigit() : void                                │ │
│  │  - OnOperator() : void                             │ │
│  │  - OnEquals() : void                               │ │
│  │  - OnDot() : void                                  │ │
│  │  - OnClear() : void                                │ │
│  └──────────────────────┬─────────────────────────────┘ │
│                         │                                 │
│  ┌────────────────────────────────────────────────────┐ │
│  │         ViewModelBase                              │ │
│  │  - PropertyChanged : PropertyChangedEventHandler   │ │
│  │  - OnPropertyChanged(string) : void                │ │
│  │  - SetProperty<T>(ref T, T, string) : void         │ │
│  └────────────────────────────────────────────────────┘ │
└─────────────────────────┬────────────────────────────────┘
						  │ Uses
						  ↓
┌─────────────────────────────────────────────────────────┐
│                   Model Layer                           │
│  ┌────────────────────────────────────────────────────┐ │
│  │          CalculatorModel                          │ │
│  │  - FirstOperand : double                           │ │
│  │  - SecondOperand : double                          │ │
│  │  - CurrentOperator : string                        │ │
│  │  - Result : double                                 │ │
│  │  - Calculate(double, double, string) : double     │ │
│  │  - Reset() : void                                  │ │
│  └────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────┐
│              Commands Layer                          │
│  ┌──────────────────────────────────────────────────┐│
│  │           RelayCommand : ICommand                ││
│  │  - _execute : Action<object>                     ││
│  │  - _canExecute : Predicate<object>               ││
│  │  - Execute(object) : void                        ││
│  │  - CanExecute(object) : bool                     ││
│  │  - CanExecuteChanged : EventHandler              ││
│  └──────────────────────────────────────────────────┘│
└──────────────────────────────────────────────────────┘
```

## Поток данных

1. **Пользователь нажимает кнопку** → Срабатывает Command в ViewModel
2. **ViewModel получает команду** → Обновляет Model и свойства
3. **PropertyChanged срабатывает** → View обновляется через Binding
4. **Display обновляется** → Пользователь видит результат

## Преимущества архитектуры MVVM

✓ **Разделение ответственности** - каждый слой имеет четкую функцию
✓ **Легко тестировать** - ViewModel и Model можно тестировать без UI
✓ **Переиспользуемость** - одну Model можно использовать с разными Views
✓ **Гибкость** - легко менять UI без изменения логики
✓ **Поддерживаемость** - код более организован и понятен

## Структура проекта

```
SimpleCalculatorMVVM/
├── App.xaml                          # Конфигурация приложения
├── App.xaml.cs                       # Code-behind для App
├── SimpleCalculatorMVVM.csproj       # Файл проекта
├── AssemblyInfo.cs                   # Информация сборки
├── ARCHITECTURE.md                   # Этот файл
├── Commands/
│   └── RelayCommand.cs              # Реализация команд
├── Models/
│   └── CalculatorModel.cs           # Модель с бизнес-логикой
├── ViewModels/
│   ├── ViewModelBase.cs             # Базовый класс ViewModel
│   └── CalculatorViewModel.cs       # ViewModel для калькулятора
└── Views/
	├── MainWindow.xaml              # Интерфейс
	└── MainWindow.xaml.cs           # Code-behind для View
```

## Запуск проекта

```bash
dotnet build
dotnet run --project SimpleCalculatorMVVM
```

Или откройте решение в Visual Studio и нажмите F5 для отладки.

## Функциональность

- ✓ Базовые арифметические операции (+, -, *, /)
- ✓ Ввод десятичных чисел
- ✓ Очистка дисплея (C)
- ✓ Вычисление результата (=)
- ✓ Привязка данных в реальном времени
- ✓ Обработка ошибок (деление на ноль)

## Дополнительные возможности (для развития)

- Операция процента (%)
- Смена знака (±)
- История операций
- Клавиатурный ввод
- Запомнение результата

