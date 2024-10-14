# Бот для сохранения сообщений группы 14221 ФБКИ ИГУ

## Почему так много текста / душно
![Душно](https://media1.tenor.com/m/efTuGQciB3cAAAAC/%D0%B4%D1%83%D1%88%D0%BD%D0%BE-%D0%B4%D1%83%D1%85%D0%BE%D1%82%D0%B0.gif)
1. Вдохнули.
2. Выдохнули.
3. Читаем дальше

## ВАЖНО: После миграции с SQLite на PostgreSQL
1. Для запуска бота во время тестирования запускаем PostgreSQL с настройками как в файле appsettings.Development.json (`docker run -e POSTGRES_USER=FbkiBot -e POSTGRES_DB=FbkiBot -e POSTGRES_PASSWORD=fbki1337 -p 5432:5432 -d postgres:latest`)
2. После изменений схемы данных в БД (класса BotDbContext или содержащихся в нём моделей), добавляем миграцию `dotnet ef migrations add <название>`

## Перед написанием кода
Можно (и категорически рекомендуется!) поставить git hook, который будет автоматически делать форматирование (`dotnet format`) и прогонять тесты (`dotnet test`). Если тесты не проходят, хук запрещает делать коммит. Коммитим только работающий код, проходящий тесты.

Хук можно взять [здесь](https://gist.github.com/1ffycat/54729c77c026a774e431a59c305f5565). Кладем в папку `.git/hooks/` (чтобы получился файл `.git/hooks/pre-commit`, расширения нет)

Или установить одной командой (из корня репы):
- **Windows 10+ (PowerShell):** `Invoke-WebRequest https://gist.githubusercontent.com/1ffycat/54729c77c026a774e431a59c305f5565/raw/eb85b3e11fa4a6ff78993d98d9ac96716ce20940/pre-commit -O .git/hooks/pre-commit`
- **Windows 10+ (cmd):** `curl.exe https://gist.githubusercontent.com/1ffycat/54729c77c026a774e431a59c305f5565/raw/eb85b3e11fa4a6ff78993d98d9ac96716ce20940/pre-commit -o .git/hooks/pre-commit`
- **Linux:** `wget https://gist.githubusercontent.com/1ffycat/54729c77c026a774e431a59c305f5565/raw/eb85b3e11fa4a6ff78993d98d9ac96716ce20940/pre-commit -O .git/hooks/pre-commit`

![Гиперопека](https://media1.tenor.com/m/EjSgVuIVEM8AAAAd/monjjunirawr-cat-mom.gif)

_Гиперопека - теперь в Git!_

## Как добавлять фичи
_Исправления очепяток и мелких багов - тоже важный вклад_
1. Создаем форк репозитория
2. Создаем ветку для фичи / фикса / т.п.
3. Делаем изменения. Не забываем делать детальные коммиты и грамотно их называть
4. Не забываем прогонять dotnet format если этого не делает IDE
5. Создаем юнит-тесты для всего добавленного функционала
6. Прогоняем юнит-тесты (через dotnet test или IDE), убеждаемся что все проходит
7. Тестируем работу бота на своем токене (можно использовать appsettings.Development.json)
8. Создаем Pull Request из своего форка в эту репу
9. В пулл реквесте подробно описываем что изменили, как оно будет работать, и, если нужно, для чего оно вообще надо
10. Ждем мержа пулл реквеста, делаем новые коммиты с правками по надобности
11. Радуемся своему уникальному вкладу

![Радуемся](https://media.tenor.com/Nx5Dg2lKTtQAAAAi/cat-jump-happy-happy-happy.gif)

_^^^ Хочешь быть как он? Создавай PR_

## Как писать код
1. Стараемся следовать SOLID принципам
2. Стараемся не ломать работающий код
3. Комментируем и документируем код, используем XML-комментарии (`///`) в C#
4. Внимательно следим за содержимым коммитов. Не коммитим лишнего (локальные, временные файлы, т.п.)
5. Если IDE не делает этого за нас - прогоняем dotnet format перед коммитами!!!
6. Прогоняем dotnet test (через терминал или в IDE), чтобы прошли юнит тесты. С непрошедшими тестами стараемся не коммитить
7. Пишем юнит-тесты для всего добавленного функционала
8. Не срём в мастер

![НЕ СРЁМ В МАСТЕР Я СКАЗАЛ](https://media.tenor.com/QUSMUwP4DX4AAAAi/plink-cat-blink.gif)

_Когда случайно запушил в мастер_

## Как ловить баги
1. Используем точки останова и прочие инструменты дебага в .NET. Если не хватает инструментов терминала и vscode - используем полную Visual Studio Community
2. По надобности повышаем "разговорчивость" логов - ставим настройку `Logging.LogLevel.Default` на `Debug` через любой возможный способ передачи настроек (по умолчанию в профиле Development)

![Ловим баги](https://media1.tenor.com/m/0suZIgRC6IQAAAAd/cat-scared.gif)

_Попытка поймать баг полностью провалилась. Полностью_

## Как разобраться в происходящем
Список того, что погуглить, если что-то не понятно:
1. C#/.NET, по надобности Docker (`dotnet run`, `docker build`, `dockerfile`)
2. Инъекция зависимостей (DI) в .NET (`FbkiBot.Services`, `Program.cs`, `builder.Add...`)
3. Конфигурация в .NET (appsettings, переменные среды, `FbkiBot.Configuration`)
4. Entity Framework (БД)

![Сложна](https://media.tenor.com/Mow3BwJQLc8AAAAi/cat-cat-meme.gif)

_Слишком много информации_

## Как добавить команду
Как референс можно использовать код команды `/start`
1. Создаем файл `...Command.cs` в папке `Commands`
2. В созданном файле создаем класс `...Command`, наследуем от интерфейса `IChatCommand`, не забываем пространство имен: `namespace FbkiBot.Commands;`
3. Реализуем описанные в интерфейсе методы для этого класса, читаем комментарии если непонятно что они должны делать
4. Добавляем атрибут `[BotCommand()]`, в скобках задаем имя, описание и по надобности пример использования команды. Это будет автоматически добавлено в `/help`
5. Регистрируем команду в `Program.cs` через `builder.AddCommand<...Command>();`
6. Пробуем, радуемся

![Радуемся](https://media.tenor.com/Gz408T11T8gAAAAi/wiggle-cat-wiggle.gif)

_^^^ Он добавил команду в бота_

## Как добавить таблицу в БД
1. Создаем модель хранимого в таблице объекта в папке `Models`
2. Добавляем новый `DbSet` с нужным типом объекта в `Data/BotDbContext.cs` по аналогии с имеющимися
3. Создаём миграцию через `dotnet ef migrations add <название>` - они нужны, чтобы при обновлении бота на сервере схема БД автоматически обновлялась
4. Готово

![Таблицы](https://media1.tenor.com/m/oTeBa4EVepMAAAAd/business-cat-working.gif)

_Таблицы? Как в Excel?_

## Как запустить у себя (для тестов или личного пользования)

_Для тестирования рекомендуется запускать бота с аргументом `--environment=Development`, либо с переменной среды `DOTNET_ENVIRONMENT="Development"`_
_Не забываем про сервер PostgreSQL, проще и удобнее всего запускать через Docker. (см. "После миграции с SQLite на PostgreSQL")_

### Запуск
#### Docker 
Релиз из репы: `docker run ghcr.io/1ffycat/fbki-bot`

Собрать из кода: `docker build . -t fbki-bot && docker run fbki-bot`

#### Без докера
`dotnet run`

### Как задать токен бота
#### Docker
1. Через -e: `docker run -e Telegram__BotToken=<токен> ghcr.io/1ffycat/fbki-bot`
2. Через appsettings: пишем токен в `appsettings.json` или `appsettings.Development.json`. Билдим контейнер и запускаем
3. Через docker volume и appsettings: повторяем пункт 2, но пробрасываем файл из контейнера наружу. Так можно не пересобирать контейнер при изменении настроек

#### Без докера
1. `dotnet run /Telegram:BotToken=<токен>`
2. Через appsettings. Как с докером, но запускаем через `dotnet run`
3. Через переменную среды. Добавляем в системе переменную среды `Telegram__BotToken`, запускаем через `dotnet run`


![Докер-контейнер, фото в цвете](https://media1.tenor.com/m/0kT25AKiz3cAAAAd/fast-cat.gif)

_Докер-контейнер, фото в цвете_
