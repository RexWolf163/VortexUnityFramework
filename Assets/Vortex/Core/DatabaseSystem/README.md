# Vortex Database System (Unity)

**Версия:** 1.0  
**Платформа:** Unity 2022.3 LTS  
**Уровень архитектуры:** 2 (`Vortex.Unity.DatabaseSystem`)  
**Зависимости:**
- Vortex Core (`Vortex.Core.DatabaseSystem`)
- Vortex Unity (`LoaderSystem`, `SaveSystem`)
- UnityEngine (`Resources`, `ScriptableObject`)
- (опционально) Unity Addressables

---

## Назначение

Пакет реализует **платформозависимый, но доменно-нейтральный** адаптер к ядру `Vortex.Core.DatabaseSystem`. Он отвечает за загрузку неизменяемых пресетов данных (`IRecordPreset`, представленных как `ScriptableObject`) из Unity-ресурсов (`Resources` или `Addressables`) и передачу их в платформонезависимое ядро для последующего использования.

Пакет предоставляет два драйвера:
- `ResourcesDriver` — для простой загрузки из папки `Resources`.
- `AddressablesDriver` — для гибкой и асинхронной загрузки через систему Addressables. Требует пакет com.unity.addressables;

Выбор драйвера осуществляется централизованно через `DriverConfig`.

---

## Контракт

### Вход
*   **`DatabaseDriver.SetDriver()`**: Регистрация одного из драйверов (`ResourcesDriver`, `AddressablesDriver`) в ядре `Database`.
*   **`ResourcesDriver`**: Ожидает `ScriptableObject`, реализующие `IRecordPreset`, находящиеся в папке `Assets/Resources/Database/`.
*   **`AddressablesDriver`**: Ожидает `ScriptableObject`, реализующие `IRecordPreset`, помеченные в Addressables соответствующими метками, указанными в `DatabaseSettings`.

### Выход
*   Данные из загруженных `IRecordPreset` становятся доступны через ядро `Database` (`Database.GetRecord`, `Database.GetNewRecord`).

### Гарантии
*   Данные загружаются один раз при старте приложения.
*   Загрузка интегрирована в `LoaderSystem`, обеспечивая предсказуемый порядок инициализации.
*   Данные индексируются ядром по `GuidPreset`, обеспечивая быстрый доступ.
*   MultiInstance-записи создаются как копии шаблонов `IRecordPreset` и не сохраняются системой `SaveSystem`.
*   Singleton-записи могут быть сериализованы и десериализованы `SaveSystem`.

### Ограничения
*   Требует предварительной регистрации драйвера через `Database.SetDriver()`.
*   Зависит от Unity (`Resources`, `Addressables`, `ScriptableObject`).
*   В `ResourcesDriver` путь к ресурсам жестко задан как `Database`.
*   В `AddressablesDriver` метки для загрузки берутся из `DatabaseSettings`.
*   Нет встроенной валидации дубликатов `GuidPreset` между различными `IRecordPreset` на этапе загрузки. Последний загруженный пресет с дублирующимся GUID перезапишет предыдущий в индексе ядра.
*   Методы интерфейса `IDriverEditor` доступны только в редакторе Unity.

---

## Применение в редакторе

*   **`DbRecordAttribute`**: Позволяет в инспекторе Unity выбрать `RecordPreset` из базы данных и получить его `GuidPreset`. Работает через `DbRecordAttributeDrawer`, который использует метод `IDriverEditor.GetPresetForRecord` для поиска пресета по GUID. Этот механизм доступен только в редакторе.
*   **Перезагрузка кэша:** Метод `IDriverEditor.ReloadDatabase` (реализован в `ResourcesDriver.Editor.cs` и `AddressablesDriver.Editor.cs`) позволяет принудительно перезагрузить пресеты из источника (`Resources` или `Addressables`) и обновить индекс ядра `Database`. Это полезно для визуализации изменений в пресетах без перезапуска сцены. В рантайме эти методы не выполняют никаких действий.
*   **`DatabaseDriverExtEditor`**: Предоставляет вспомогательные методы, такие как `GetPresetByGuid`, для работы с пресетами в редакторе, например, для проверки целостности связей.

---

## Поведение в граничных случаях

*   **`ResourcesDriver`:** Если папка `Assets/Resources/Database/` отсутствует или пуста, драйвер успешно инициализируется без загрузки данных.
*   **`AddressablesDriver`:** Если метки в `DatabaseSettings` не заданы или не соответствуют никаким ресурсам, драйвер успешно инициализируется без загрузки данных.
*   **Отсутствие конфигурации драйвера:** Если в `DriverConfig` не указан драйвер для `Database`, ни `ResourcesDriver`, ни `AddressablesDriver` не смогут зарегистрироваться, так как не пройдут проверку белого списка в `SetDriver`. Система `Database` останется неинициализированной.