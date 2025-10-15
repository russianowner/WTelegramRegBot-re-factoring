# WTelegramRegBot-re-factoring
---
- Бот для регистрации тг профилей через WTelegramClient
---
- Позволяет авторизовать аккаунты Telegram с помощью телефона, API ID и API Hash
---
- A bot for registering TG profiles via WTelegramClient
---
- Allows you to authorize Telegram accounts using your phone, API ID, and API Hash
---
- Simple Project - https://github.com/russianowner/WTgRegBot-simple
---

## Что в боте

- Регистрация аккаунтов через WTelegramClient
- Сохранение сессий в базе данных
- Авторизация через код, e-mail или пароль 2FA
- Тг бот интерфейс
- Мультиаккаунтность на будущее
- Использование базы данных SQLite

---

## What's in the bot

- Account registration via WTelegramClient
- Saving sessions in the database
- Authorization via code, e-mail or 2FA password
- TG bot interface
- Multi-accountancy for the future
- Using an SQLite database

---

## NuGet Packages
- dotnet add package Telegram.Bot
- dotnet add package WTelegramClient
- dotnet add package Microsoft.EntityFrameworkCore
- dotnet add package Microsoft.EntityFrameworkCore.Sqlite
- dotnet add package Microsoft.EntityFrameworkCore.Tools
- dotnet add package Microsoft.Extensions.DependencyInjection

---

## Как запустить
- Скопировать репозиторий
- Зайти в Config.cs и поменять "Токен" на токен вашего бота
- В терминале ввести dotnet run или запустить проект
- В боте ввести /start
- Выйдет главное меню в нем две кнопки "Добавить профиль" и "Список профилей"
- Нажимаем добавить профиль, вводим номер телефона с кодом страны, потом API_ID, API_HASH, код с телеги, 2FA если есть на аккаунте.
- Профиль сохранен, можно добавлять еще
- В списке профилей можно посмотреть добавленные профили

---

## How to launch
- Copy the repository
- Go to Config.cs and change the "Token" to your bot's token
- Enter dotnet run in the terminal or run the project
- Enter /start in the bot
- The main menu will appear with two buttons "Add profile" and "Profile list"
- Click add profile, enter the phone number with the country code, then API_ID, API_HASH, cart code, 2FA if available on the account.
- The profile is saved, you can add more
- You can view the added profiles in the profile list.

---

## Архитектура бота

---
- Папка Bot:
- Program.cs - Создаёт TelegramBotClient, ProfileService, Logger, Config и BotHandler, после запускает апдейт через StartReceiving
---
- BotHandler.cs:
- Получает апдейты от телеги
- Определяет тип входящего события (сообщение, кнопка)
- Управляет UserSession (состоянием пользователя)
- Отправляет сообщения и меню
---
- UserSession.cs:
- Запоминает введенные значения (телефон, API ID, API Hash и т.д.)
- Знает, на каком шаге регистрации находится пользователь
- Управляет последовательностью шагов через ProcessInput
---
- UserMode.cs - состояния пользователя при регистрации профиля
---
- Папка Common:
- Config.cs - настройки проекта (токен бота, путь к базе, API ключи и т.д)
---
- Logger.cs - используется во всех слоях для отладки и ошибок
--- 
- Папка Core:
- Profile.cs - модель тг профиля в бд
---
- IProfileService.cs - Интерфейс для ProfileService (контракты такие некие)
---
- Папка Data:
- BotDbContext.cs:
- Настраивает SQLite
- Определяет таблицы (в частности Profiles)
- Отвечает только за хранение и получение данных
---
- Папка Services:
- ProfileService.cs:
- создаёт запись в базе
- инициализирует WTelegramClient
- обрабатывает ввод кода, email, пароля
- завершает авторизацию
---
- TelegramClientService.cs:
- создание клиента
- получение кода авторизации
- логин и сохранение .session
---

## Bot Architecture


- Bot Folder:
- Program.cs - Creates a TelegramBotClient, ProfileService, Logger, Config and BotHandler, then launches the update via StartReceiving
---
- BotHandler.cs:
- Receives updates from cart
- Defines the type of incoming event (message, button)
- Manages the UserSession (user status)
- Sends messages and menus
---
- UserSession.cs:
- Remembers the entered values (phone, API ID, API Hash, etc.)
- Knows which registration step the user is at
- Manages the sequence of steps via ProcessInput
---
- UserMode.cs - User status during profile registration
---
- Common folder:
- Config.cs - project settings (bot token, database path, API keys, etc.)
---
- Logger.cs - used in all layers for debugging and errors
--- 
- The Core folder:
- Profile.cs - tg profile model in the database
---
- IProfileService.cs - Interface for ProfileService (some kind of contracts)
---
- Data folder:
- BotDbContext.cs:
- Configures SQLite
- Defines tables (in particular Profiles)
- Responsible only for storing and receiving data
---
- The Services folder:
- ProfileService.cs:
- creates an entry in the database
- initializes the WTelegramClient
- processes the input of a code, email, password
- completes authorization
---
- TelegramClientService.cs:
- creating a client
- getting the authorization code
- login and save .session
---
- Проект можно развивать дальше, тут сделана только авторизация и сохранение профиля, можно добавить функции по работе с аккаунтом или аккаунтами! 
---
- The project can be further developed, only authorization and profile saving are done here, you can add functions for working with your account or accounts!
