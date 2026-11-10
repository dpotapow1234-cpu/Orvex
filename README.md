# Orvex

![Счетчик просмотров](https://komarev.com/ghpvc/?username=dpotapow1234-cpu)

**Orvex** is an open-source voxel sandbox game focused on exploration, procedural worlds, modular architecture, and multiplayer gameplay.

The project aims to provide a flexible foundation for creating and exploring voxel-based worlds, with support for modifications and dedicated multiplayer servers.

**Orvex** — это открытая voxel-песочница, ориентированная на исследование мира, процедурную генерацию, модульную архитектуру и многопользовательский режим.

Проект призван предоставить гибкую основу для создания и исследования voxel-миров с поддержкой модификаций и выделенных многопользовательских серверов.

---

## Features | Возможности

- **Voxel World** — a world built from individual blocks.
- **Procedural Generation** — dynamically generated environments.
- **Dynamic Day and Night Cycle** — changing world lighting and time.
- **Mod System** — a hybrid architecture designed to support game modifications.
- **Multiplayer Architecture** — a separate dedicated server and shared networking protocol.
- **Extensible Design** — modular systems intended to make future development easier.

- **Voxel-мир** — игровой мир, состоящий из отдельных блоков.
- **Процедурная генерация** — динамическое создание игровых окружений.
- **Смена дня и ночи** — динамическое изменение времени суток и освещения.
- **Система модификаций** — гибридная архитектура для поддержки пользовательских модификаций.
- **Многопользовательская архитектура** — отдельный выделенный сервер и общий сетевой протокол.
- **Расширяемая архитектура** — модульная структура, упрощающая дальнейшее развитие проекта.

> Orvex is under active development. Features and architecture may change.

> Orvex находится в активной разработке. Функциональность и архитектура проекта могут изменяться.

---

## Project Structure | Структура проекта

```text
Orvex/
├── Orvex/                   # Unity game client / Игровой клиент Unity
├── Orvex.Server/            # Dedicated server / Выделенный сервер
├── Orvex.Shared/            # Shared protocol and data structures / Общие протоколы и структуры данных
├── Orvex.Client.Networking/ # Client networking library / Сетевая библиотека клиента
└── scripts/                 # Development scripts / Скрипты разработки
```

## Technology Stack | Используемые технологии

| Technology | Purpose | Технология | Назначение |
|---|---|---|---|
| Unity | Game engine | Unity | Игровой движок |
| C# | Main programming language | C# | Основной язык программирования |
| .NET 10 | Dedicated server and shared libraries | .NET 10 | Выделенный сервер и общие библиотеки |
| TCP | Initial reliable network communication | TCP | Начальный надёжный сетевой обмен |
| JSON | Server configuration | JSON | Конфигурация сервера |

---

## Getting Started | Начало работы

### Requirements | Требования

- Unity (compatible project version) / Unity (совместимая версия проекта)
- .NET 10 SDK
- Visual Studio or another C# IDE / Visual Studio или другая среда разработки C#
- Git

### Clone the repository | Клонирование репозитория

```bash
git clone https://github.com/dpotapow1234-cpu/Orvex.git
cd Orvex
```

### Build the server | Сборка сервера

```bash
dotnet build Orvex.Server/Orvex.Server.csproj
```

### Run the server | Запуск сервера

```bash
dotnet run --project Orvex.Server/Orvex.Server.csproj
```

---

## Development | Разработка

Orvex is developed as a modular project. The game client, networking layer, shared protocol, and dedicated server are maintained as separate components.

Contributions, bug reports, and suggestions are welcome.

Orvex разрабатывается как модульный проект. Игровой клиент, сетевой уровень, общий протокол и выделенный сервер поддерживаются как отдельные компоненты.

Мы приветствуем участие разработчиков, сообщения об ошибках и предложения по улучшению проекта.

---

## Roadmap | Планы развития

- [x] Initial voxel world implementation / Начальная реализация voxel-мира
- [x] Dynamic time system / Система динамического времени
- [x] Hybrid Mods System architecture / Гибридная архитектура системы модификаций
- [x] Dedicated server foundation / Основа выделенного сервера
- [x] Initial TCP networking / Начальная реализация TCP-сети
- [x] Shared packet protocol / Общий протокол сетевых пакетов
- [ ] Client-server handshake integration / Интеграция рукопожатия клиента и сервера
- [ ] Player authentication and sessions / Авторизация игроков и управление сессиями
- [ ] Multiplayer player synchronization / Синхронизация игроков в мультиплеере
- [ ] World and chunk synchronization / Синхронизация мира и чанков
- [ ] UDP gameplay transport / UDP-транспорт для игрового процесса
- [ ] Mod loading and management improvements / Улучшение загрузки и управления модификациями

---

## License | Лицензия

See [LICENSE](LICENSE) for license information.

Информация о лицензии проекта находится в файле [LICENSE](LICENSE).

## Links | Ссылки

- [Repository](https://github.com/dpotapow1234-cpu/Orvex) — Репозиторий проекта
- [Repository](https://github.com/dpotapow1234-cpu/OrvexServer) — Репозиторий сервера
- [Issues](https://github.com/dpotapow1234-cpu/Orvex/issues) — Сообщить об ошибке
- [Wiki](https://github.com/dpotapow1234-cpu/Orvex/wiki) — Документация проекта
