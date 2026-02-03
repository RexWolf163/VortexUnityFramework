# Vortex Framework

Модульный фреймворк для разработки приложений на Unity с четким разделением на слои и паттерном шины данных.

## Философия

Программирование сводится к трем задачам:
1. **Получение данных** — данные доступны из любой точки через статические шины
2. **Обработка данных** — непрерывная обработка без внешних коррекций промежуточных результатов
3. **Отображение данных** — компоненты отображения работают с моделью через шину, не напрямую

## Архитектура слоев

```
┌─────────────────────────────────────────────────────────────────┐
│  Layer 4: AppLocale                                             │
│  Частные скрипты конкретного проекта                            │
├─────────────────────────────────────────────────────────────────┤
│  Layer 3: AppSDK                                                │
│  Универсальные механики для семейства приложений                │
├─────────────────────────────────────────────────────────────────┤
│  Layer 2: Framework Adaptation (Unity)                          │
│  Драйверы, пресеты, платформозависимые реализации               │
├─────────────────────────────────────────────────────────────────┤
│  Layer 1: Framework Core                                        │
│  Нейтральные модели, шины, абстракции (без Unity API)           │
└─────────────────────────────────────────────────────────────────┘
```

### Layer 1: Framework Core

Платформонезависимая логика и модели без использования Unity API.

**Содержит:**
- Абстрактные модели данных (`Record`, `SystemModel`)
- Статические шины доступа (`Database`, `Settings`, `UIProvider`, `Localization`)
- Интерфейсы драйверов (`IDriver`, `ISystemDriver`)
- Системы загрузки и сохранения (`Loader`, `SaveSystem`)

### Layer 2: Framework Adaptation

Привязка ядра к Unity: драйверы, пресеты (`ScriptableObject`), MonoBehaviour-компоненты.

**Содержит:**
- Драйверы систем (`ResourcesDriver`, `AddressablesDriver`)
- Пресеты данных (`RecordPreset<T>`, `SettingsPreset`)
- UI-компоненты (`UserInterface`, `TweenerBase`)

### Layer 3: AppSDK

Универсальные механики для типа приложения (например, idle-игры).

**Структура пакета:**
1. Модель данных и контроллеры расширения
2. Шины доступа и хендлеры управления
3. Интерфейсы для работы с данными пакета

### Layer 4: AppLocale

Уникальные механики конкретного проекта, не предназначенные для переноса.

---

## Паттерн «Шина данных»

Центральный архитектурный паттерн фреймворка — статический класс с Dictionary-индексом для O(1) доступа.

```csharp
public static class Bus
{
    private static readonly Dictionary<string, Record> _records = new();

    public static T GetRecord<T>(string guid) where T : Record, new()
    {
        return _records.TryGetValue(guid, out var record) ? record as T : null;
    }

    public static T[] GetRecords<T>() where T : Record, new()
    {
        return _records.Values.OfType<T>().ToArray();
    }
}
```

**Критерии шины:**
1. Данные доступны для чтения из любой точки проекта
2. Запрашивающий компонент точно знает что ищет (по GUID)
3. Выборка по однозначному признаку
4. Максимальное быстродействие (Dictionary O(1))

---

## Принципы построения пакетов

### Одиночная моно-модель

| Компонент | Слой | Описание |
|-----------|------|----------|
| `{System}` | Core | Шина: кэш, логика изменения/вывода, события |
| `{System}Model` | Core | Модель: публичные свойства (`get; private set;`) |
| `{System}Preset` | Adaptation | Пресет: публичные свойства (`get;`), ScriptableObject |

### Множественные модели (Database)

| Компонент | Слой | Описание |
|-----------|------|----------|
| `{System}` | Core | Шина: индекс-реестр, методы получения |
| `Record` | Core | Модель: свойства, события, `CopyFrom()` |
| `RecordPreset<T>` | Adaptation | Пресет: метод создания Record из данных |

---

## Пакеты фреймворка

### System (Core)

Базовые абстракции и перечисления.

| Класс | Назначение |
|-------|------------|
| `Singleton<T>` | Абстрактный синглтон с lazy-инициализацией |
| `SystemController` | Контроллер с драйвером (EBS-паттерн) |
| `SystemModel` | Модель с `CopyFrom(source)` для заливки данных из пресетов |
| `ISystemDriver` | Интерфейс драйвера (`Init`, `Destroy`) |
| `Timer` | Модель таймера (без логики обработки) |

### Database

Централизованное хранилище данных с поддержкой `Singleton` и `MultiInstance` записей.

```csharp
// Singleton — один экземпляр на весь жизненный цикл (сохраняется)
var profile = Database.GetRecord<UserProfile>("user-profile-guid");

// MultiInstance — новая копия при каждом запросе (не сохраняется)
var template = Database.GetNewRecord<DocumentTemplate>("template-guid");
```

**Ключевые концепции:**
- `Record` — базовый класс записи (Core)
- `RecordPreset<T>` — ScriptableObject пресет (Adaptation)
- `DbRecordAttribute` — выпадающий список в Inspector
- Драйверы: `ResourcesDriver`, `AddressablesDriver`

[Подробная документация](Core/DatabaseSystem/README.md)

### Loader

Система асинхронной загрузки приложения с автоматическим разрешением зависимостей.

```csharp
// Регистрация системы
Loader.Register(myProcess);

// Запуск загрузки
await Loader.Run();
```

**Особенности:**
- Автоматическое определение порядка по `WaitingFor()`
- Поддержка `ISystemController` через статическое свойство `IsInit`
- Кэширование PropertyInfo для оптимизации рефлексии
- События `OnLoad`, `OnComplete`

[Подробная документация](Core/LoaderSystem/README.md.txt)

### Settings

Система настроек с поддержкой сбора данных из нескольких ScriptableObject.

```csharp
var settings = Settings.Data();
if (settings.DebugMode) { /* ... */ }
```

**Расширение:**
- Модель: через `partial class SettingsModel`
- Пресет: наследование от `SettingsPreset`

### SaveSystem

Сохранение и загрузка данных с компрессией и шифрованием.

**Логика работы:**
1. Компонент реализует `ISaveable`
2. Регистрируется в `SaveController`
3. При сохранении формирует строку через `GetSaveData()`
4. Данные собираются в XML → сжимаются → base64 → `PlayerPrefs`

### UIProvider

Управление жизненным циклом интерфейсов на основе декларативных условий.

```csharp
// Регистрация и управление
UIProvider.Register("menu-guid");
UIProvider.Open("menu-guid");
UIProvider.CloseAll();
```

**Типы интерфейсов:**
- `Common` — базовые меню
- `Panel` — информационные панели
- `Overlay` — хэлсбары, динамические списки
- `Popup` — всплывающие окна подтверждения

[Подробная документация](Unity/UIProviderSystem/README.md)

### Localization

Локализация из Google Sheets с поддержкой множества языков.

```csharp
string text = Localization.Translate("KEY");
// или
string text = "KEY".Translate();
```

### LogicChains (Quests)

Машины состояний для сценариев и квестов.

```csharp
LogicChains.AddChain(chainPreset);
LogicChains.RunChain(chainGuid);
```

### Pool

Репликация однотипных GameObject с переиспользованием.

```csharp
pool.AddItem(data);    // Создание/реактивация
pool.RemoveItem(data); // Деактивация для реиспользования
```

### ComplexModel

Модульная расширяемая модель данных с автоматическим обнаружением компонентов.

```csharp
public class GameModel : ComplexModelUnity<IGameData> { }

// Компонент автоматически обнаруживается
[ProtoContract]
public class HeroParameters : IGameData
{
    [ProtoMember(1)] public int Strength { get; set; }
}

// Использование
var model = new GameModel();
model.Init();
var hero = model.Get<HeroParameters>();
```

---

## Extensions (Расширения)

### ActionExt

```csharp
action.Fire();              // action?.Invoke()
func.FireOr();              // true если хотя бы один true
func.FireAnd();             // true если все true
func.FirstNotNull<T>();     // первый не-null результат
func.Accumulate<T>();       // массив всех результатов
```

### DictionaryExtAdding

```csharp
dict.AddNew(key, value);    // добавить с проверкой дубликата
dict.Get(key);              // получить с логированием ошибки
```

### StringExtCompress

```csharp
string compressed = original.Compress(key);
string restored = compressed.Decompress(key);
```

### DateTimeExtConvert

```csharp
long unix = dateTime.ToUnixTime();
DateTime dt = unix.FromUnixTime();
```

---

## UI-компоненты (Adaptation)

### UIStateSwitcher

Машина состояний для переключения параметров компонентов.

### TweenerBase

Абстрактный класс для плавного изменения параметров (анимации).

### UIComponent

Система вью-компонентов для скинов-примитивов.

```csharp
component.PutData(data);
component.SetText("Title");
component.SetSprite(icon);
component.SetAction(OnClick);
```

---

## Кодогенерация (Editor)

| Команда | Путь в меню | Описание |
|---------|-------------|----------|
| **Create Record** | `Assets/Create/Vortex/Record` | Генерация класса-наследника `Record` |
| **Create Preset** | `Assets/Create/Vortex/Preset for Record` | Генерация `RecordPreset<T>` для выбранного Record |
| **Create UI Condition** | `Assets/Create/Vortex/UI Condition` | Генерация условия интерфейса |

---

## Принципы обработки данных

### 1. Непрерывность обработки

Нельзя допускать внешнюю коррекцию промежуточных результатов:

```csharp
// ❌ Плохо: событие срабатывает на каждое изменение
model.HP = 50;  // → OnChange → внешняя коррекция
model.MP = 30;  // → OnChange → рекурсия

// ✓ Хорошо: ручной вызов после всех изменений
model.HP = 50;
model.MP = 30;
model.NotifyChanged();
```

### 2. Аккумуляция вызовов

При многократных изменениях в одном кадре — обработка один раз в конце:

```csharp
// Множество изменений в кадре
soldier.SetTarget(enemy);
soldier.SetPosture(Crouching);
soldier.TakeDamage(10);

// Визуальное обновление — один раз в конце кадра
```

### 3. Разделение модели и представления

```csharp
// ❌ Плохо: прямая передача данных
interface.SetData(model);

// ✓ Хорошо: компонент сам получает данные из шины
public class HeroPanel : MonoBehaviour
{
    void OnEnable()
    {
        var hero = HeroBus.GetCurrent();
        UpdateView(hero);
        HeroBus.OnChanged += UpdateView;
    }
}
```

---

## Файловая структура

```
Assets/Vortex/
├── Core/                           # Layer 1: Framework Core
│   ├── System/                     # Базовые абстракции
│   ├── DatabaseSystem/             # Система базы данных
│   ├── LoaderSystem/               # Асинхронная загрузка
│   ├── SaveSystem/                 # Сохранение/загрузка
│   ├── SettingsSystem/             # Настройки
│   ├── LoggerSystem/               # Логирование
│   ├── AppSystem/                  # Состояние приложения
│   ├── UIProviderSystem/           # Управление UI (Core)
│   ├── LocalizationSystem/         # Локализация
│   ├── LogicChainsSystem/          # Квесты/сценарии
│   ├── ComplexModelSystem/         # Модульные модели
│   ├── MappedParametersSystem/     # Маппинг параметров
│   └── Extensions/                 # Расширения
│
├── Unity/                          # Layer 2: Framework Adaptation
│   ├── DatabaseSystem/             # Драйверы БД
│   ├── UIProviderSystem/           # UI-компоненты
│   ├── SettingsSystem/             # Пресеты настроек
│   ├── AppSystem/                  # FocusHandler, TimeController
│   ├── LocalizationSystem/         # Драйвер локализации
│   ├── DriverManagerSystem/        # Управление драйверами
│   ├── CoreAssetsSystem/           # Базовые ассеты
│   ├── UI/                         # UI-компоненты
│   ├── Components/                 # MonoBehaviour компоненты
│   └── Extensions/                 # Unity-расширения
│
└── README.md                       # Эта документация
```

---

## Быстрый старт

### 1. Настройка драйвера базы данных

1. Создайте `DriverConfig.asset` в `Resources/`
2. Назначьте `ResourcesDriver` или `AddressablesDriver`

### 2. Создание записи базы данных

```csharp
// 1. Создайте Record (Core)
public class ProductRecord : Record
{
    public float Price { get; set; }
    public int Quantity { get; set; }

    public override string GetDataForSave()
        => this.SerializeProperties();

    public override void LoadFromSaveData(string data)
    {
        var temp = data.DeserializeProperties<ProductRecord>();
        this.CopyFrom(temp);
    }
}

// 2. Создайте Preset (используйте кодогенерацию или вручную)
[CreateAssetMenu(menuName = "Database/Product")]
public class ProductPreset : RecordPreset<ProductRecord>
{
    [SerializeField] private float price;
    public float Price => price;
}

// 3. Используйте в коде
var product = Database.GetNewRecord<ProductRecord>("product-guid");
```

### 3. Создание интерфейса

1. Создайте `UserInterfacePreset` через `Create > Database > UserInterface Preset`
2. Настройте тип и условия
3. Добавьте `UserInterface` компонент на объект сцены
4. Укажите GUID пресета

---

## Зависимости

- Unity 2021.3+
- Odin Inspector (опционально, для редакторных инструментов)
- Addressables (опционально, для `AddressablesDriver`)
- protobuf-net (для `ComplexModel` сериализации)

---

## Лицензия

Proprietary. Все права защищены.
