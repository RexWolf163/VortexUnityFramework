# `DatabaseSystem`

Централизованное хранилище данных приложения с поддержкой `Singleton` и `MultiInstance` записей.

## Назначение

`DatabaseSystem` предоставляет единую точку доступа к данным приложения:

- **Сущности** — объекты предметной области (персонажи, предметы, здания)
- **Конфигурации** — настройки, шаблоны, пресеты
- **Справочники** — каталоги, словари, перечисления
- **Любые данные** с уникальным идентификатором

## Зависимости

- **Core**: `LoaderSystem`, `SaveSystem`
- **Unity**: Odin Inspector (для редакторных инструментов)

## Архитектура

```
┌─────────────────────────────────────────────────────────────────┐
│                         Core Layer                              │
├─────────────────────────────────────────────────────────────────┤
│  Database (Bus)          - Шина доступа к записям               │
│  Record                  - Базовый класс записи                 │
│  IDriver                 - Контракт драйвера                    │
│  RecordTypes             - Singleton / MultiInstance            │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                        Unity Layer                              │
├─────────────────────────────────────────────────────────────────┤
│  ResourcesDriver         - Загрузка из Resources/Database       │
│  AddressablesDriver      - Загрузка через Addressables          │
│  DatabaseDriverBase      - Shared-логика (DRY)                  │
│  RecordPreset<T>         - ScriptableObject пресет              │
│  DbRecordAttribute       - Dropdown в Inspector                 │
└─────────────────────────────────────────────────────────────────┘
```

## Ключевые концепции

### `Singleton` vs `MultiInstance`

| Тип | Поведение | Сохранение | Пример |
|-----|-----------|------------|--------|
| `Singleton` | Один экземпляр в памяти на весь жизненный цикл | Да, через `SaveSystem` | Профиль пользователя, состояние сессии |
| `MultiInstance` | Новая копия при каждом запросе | Нет (воссоздаётся из пресета) | Шаблоны предметов, справочники |

```csharp
// Singleton — всегда один и тот же объект
var profile = Database.GetRecord<UserProfile>("user-profile-guid");
profile.Counter += 1;  // Изменение сохранится между сессиями

// MultiInstance — каждый раз новая копия из пресета
var template1 = Database.GetNewRecord<DocumentTemplate>("report-template-guid");
var template2 = Database.GetNewRecord<DocumentTemplate>("report-template-guid");
// template1 != template2 (разные экземпляры)
```

### Пресет vs Запись

| Концепция | Слой | Назначение |
|-----------|------|------------|
| `RecordPreset<T>` | Unity | ScriptableObject с данными в редакторе |
| `Record` | Core | Runtime-объект с данными |

Пресет — шаблон в редакторе. При загрузке из него создаётся запись в памяти.

---

## ⚠️ Критические требования

1. **Драйвер обязателен**  
   Драйвер базы данных **должен** быть назначен в `Assets/Resources/DriverConfig.asset`. Без этого система останется неинициализированной.

2. **Уникальность GUID**  
   При загрузке пресетов с одинаковым `GuidPreset` последний загруженный пресет **перезаписывает** предыдущий без предупреждения. Валидация дубликатов — ответственность разработчика.

3. **Автоматическая загрузка**  
   Загрузка данных происходит автоматически через `LoaderSystem`. Ручной вызов методов драйвера запрещён.

---

## Контракт

### Вход
- `DriverConfig` с назначенным драйвером (`ResourcesDriver` или `AddressablesDriver`)
- Пресеты в `Resources/Database/` (для `ResourcesDriver`)
- Пресеты с метками Addressables (для `AddressablesDriver`)

### Выход
- Индексированные записи через `Database.GetRecord<T>()` / `Database.GetNewRecord<T>()`
- O(1) доступ по `GuidPreset`

### Гарантии
- Данные загружаются один раз при старте через `LoaderSystem`
- `Singleton`-записи персистентны между сессиями (сохраняются через `SaveSystem`)
- `MultiInstance` — всегда чистая копия из пресета
- Событие `Database.OnInit` триггерится после полной загрузки

### Ограничения
- Для `ResourcesDriver`: жёстко заданный путь `Resources/Database`
- Дубликаты `GuidPreset` не валидируются — последний перезаписывает предыдущий
- `IDriverEditor` работает только в редакторе (`ReloadDatabase`, `GetPresetForRecord`)

---

## API Reference

### `Database` (шина доступа)

```csharp
// ===== Получение записей =====
// Singleton — один экземпляр на весь жизненный цикл
T record = Database.GetRecord<T>(string guid);

// MultiInstance — новая копия при каждом запросе
T record = Database.GetNewRecord<T>(string guid);

// Все Singleton указанного типа
T[] records = Database.GetRecords<T>();

// Все Singleton (без фильтра)
Record[] records = Database.GetRecords();

// Все MultiInstance указанного типа как новые экземпляры
T[] records = Database.GetNewRecords<T>();

// ===== Работа с GUID =====
// Проверить существование записи
bool exists = Database.TestRecord(string guid);

// GUID всех MultiInstance пресетов
string[] guids = Database.GetMultiInstancePresets();

// GUID MultiInstance пресетов указанного типа
string[] guids = Database.GetMultiInstancePresets<T>();
```

### `Record` (базовый класс)

```csharp
public abstract class Record
{
    public string GuidPreset { get; }      // Уникальный идентификатор
    public string Name { get; }            // Название
    public string Description { get; }     // Описание
    public Sprite Icon { get; }            // Иконка (определяется в Unity-слое)

    // Сериализация для сохранения
    public abstract string GetDataForSave();
    public abstract void LoadFromSaveData(string data);
}
```

### `IDriver` (контракт драйвера)

```csharp
public interface IDriver
{
    // Передача индексов для заполнения
    void SetIndex(Dictionary<string, Record> singletonRecords, HashSet<string> uniqRecords);

    // Создание нового экземпляра из пресета
    T GetNewRecord<T>(string guid) where T : Record, new();

    // Создание экземпляров для всех MultiInstance указанного типа
    T[] GetNewRecords<T>() where T : Record, new();

    // Проверка соответствия пресета типу
    bool CheckPresetType<T>(string guid) where T : Record;
}
```

---

## Использование

### 1. Создание модели данных (Core-слой)

```csharp
// Без зависимостей от Unity
public class ProductRecord : Record
{
    public float Price { get; set; }
    public int Quantity { get; set; }
    public ProductCategory Category { get; set; }

    public override string GetDataForSave()
    {
        // Сериализация через сериализатор Vortex
        return this.SerializeProperties();
    }

    public override void LoadFromSaveData(string data)
    {
        // Десериализация + копирование в текущий экземпляр
        var temp = data.DeserializeProperties<ProductRecord>();
        this.CopyFrom(temp);
    }
}
```

> 💡 **Важно**: Для корректной работы сериализатора все данные должны быть объявлены как **публичные свойства** (`public T Prop { get; set; }`), а не поля.

### 2. Создание пресета (Unity-слой)

```csharp
[CreateAssetMenu(menuName = "Database/Product")]
public class ProductPreset : RecordPreset<ProductRecord>
{
    [SerializeField] private float price;
    [SerializeField] private int quantity;
    [SerializeField] private ProductCategory category;

    public float Price => price;
    public int Quantity => quantity;
    public ProductCategory Category => category;
}
```

> ⚠️ **Важно**: Для корректной работы `CopyFrom()` все данные, подлежащие копированию в `Record`, должны быть доступны через **публичные свойства-геттеры**. Приватные поля без соответствующих свойств будут проигнорированы при копировании.

### 3. Получение данных в runtime

```csharp
public class CatalogView : MonoBehaviour
{
    // Атрибут создаёт dropdown со всеми ProductRecord в Inspector
    [DbRecord(typeof(ProductRecord), RecordTypes.MultiInstance)]
    [SerializeField] private string productGuid;

    public void ShowProduct()
    {
        var product = Database.GetNewRecord<ProductRecord>(productGuid);
        if (product == null)
        {
            Debug.LogError($"Product not found for GUID: {productGuid}");
            return;
        }

        DisplayInfo(product);
    }
}
```

### 4. Работа с `Singleton`

```csharp
public class SessionManager : MonoBehaviour
{
    private const string SessionGuid = "current-session";

    public void IncrementCounter()
    {
        var session = Database.GetRecord<SessionRecord>(SessionGuid);
        session.ActionCount += 1;
        // Изменения автоматически сохранятся через SaveSystem при следующем сохранении
    }
}
```

### 5. Атрибут `DbRecord`

```csharp
// Все записи любого типа
[DbRecord]
public string anyRecordGuid;

// Только записи указанного типа
[DbRecord(typeof(ProductRecord))]
public string productGuid;

// Только Singleton записи
[DbRecord(RecordTypes.Singleton)]
public string singletonGuid;

// Комбинация: тип + режим
[DbRecord(typeof(TemplateRecord), RecordTypes.MultiInstance)]
public string templateGuid;
```

---

## Настройка драйвера

### `ResourcesDriver` (простой вариант)

1. Поместите пресеты в `Assets/Resources/Database/`
2. В `DriverConfig` выберите `ResourcesDriver`

### `AddressablesDriver` (production)

1. Пометьте пресеты нужными метками (labels) в окне Addressables
2. В `DatabaseSettings` укажите эти метки
3. В `DriverConfig` выберите `AddressablesDriver`

---

## Интеграция с `SaveSystem`

`Singleton`-записи автоматически сохраняются через `SaveSystem`:

1. При сохранении вызывается `Record.GetDataForSave()` для каждой `Singleton`-записи
2. При загрузке вызывается `Record.LoadFromSaveData(string)`
3. `MultiInstance` записи **не сохраняются** — логика их сохранения лежит на контроллерах, которые хранят экземпляры данных

---

## Подписка на инициализацию

```csharp
// Подписка на событие готовности базы данных
Database.OnInit += OnDatabaseReady;

private void OnDatabaseReady()
{
    // База данных загружена, можно безопасно получать записи
    var settings = Database.GetRecord<GameSettings>("game-settings");
    ApplySettings(settings);
}
```

> 💡 Если подписка происходит после инициализации, колбэк **не вызовется**. Используйте проверку `Database.IsInit` для безопасного доступа.

---

## Кодогенерация (Editor)

Для ускорения создания Record и Preset доступны генераторы через контекстное меню Project:

| Команда | Путь | Описание |
|---------|------|----------|
| **Create Record** | `Assets/Create/Vortex/Record` | Создаёт пустой класс-наследник `Record` с реализацией сериализации |
| **Create Preset** | `Assets/Create/Vortex/Preset for Record` | Создаёт `RecordPreset<T>` для выбранного Record-класса |

### Workflow

1. ПКМ по папке → `Create/Vortex/Record` → переименовать класс
2. Добавить свойства в Record
3. ПКМ по .cs файлу Record → `Create/Vortex/Preset for Record`
4. Генератор автоматически извлечёт свойства и создаст соответствующий Preset
5. (Опционально) раскомментировать фиксацию типа пресета нужного типа на OnValidate

---

## Редакторные инструменты

### Перезагрузка базы данных

```csharp
// Обновить кэш без перезапуска сцены (только в редакторе)
var driver = Database.GetDriver() as IDriverEditor;
driver?.ReloadDatabase();
```

### Получение пресета по GUID

```csharp
#if UNITY_EDITOR
var preset = driver.GetPresetForRecord(guid);
#endif
```

---

## Граничные случаи

| Ситуация | Поведение |
|----------|-----------|
| Пустая папка `Database` | Драйвер инициализируется без данных |
| Несуществующий `GUID` | Возвращает `null` + лог ошибки |
| `Singleton` запрошен как `MultiInstance` | Возвращает `null` + лог ошибки |
| `MultiInstance` запрошен как `Singleton` | Возвращает `null` + лог ошибки |
| Драйвер не назначен в `DriverConfig` | `Database` остаётся неинициализированной, все методы возвращают `null` |

---

## Файловая структура

```
DatabaseSystem/
├── Core/
│   ├── Bus/
│   │   ├── Database.cs              # Шина доступа
│   │   ├── DatabaseExtSave.cs       # Интеграция с SaveSystem
│   │   └── DatabaseExtEditor.cs     # Editor API
│   ├── Model/
│   │   ├── Record.cs                # Базовый класс записи
│   │   └── Enums/RecordTypes.cs     # Singleton/MultiInstance
│   ├── IDriver.cs                   # Контракт драйвера
│   ├── IDriverEditor.cs             # Editor-контракт
│   └── IRecord.cs                   # Marker interface
│
└── Unity/
    ├── Drivers/
    │   ├── DatabaseDriverBase.cs    # Shared-логика
    │   ├── ResourcesDriver/         # Resources драйвер
    │   └── AddressablesDriver/      # Addressables драйвер
    ├── Presets/
    │   ├── RecordPreset.cs          # Базовый ScriptableObject
    │   └── IRecordPreset.cs         # Интерфейс пресета
    ├── Model/
    │   └── Record.cs                # Partial: добавляет Icon
    ├── Attributes/
    │   └── DbRecordAttribute.cs     # Inspector dropdown
    └── DbSettings/
        └── DatabaseSettings.cs      # Настройки Addressables
```