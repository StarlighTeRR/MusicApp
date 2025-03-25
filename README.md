
# MusicApp API

Веб-API для музыкального приложения, разработанное с использованием ASP.NET Core 8.0 и PostgreSQL.

## Технологии

- **ASP.NET Core 8.0** - Фреймворк для создания веб-приложений
- **Entity Framework Core 8.0** - ORM для работы с базой данных
- **PostgreSQL** - Система управления базами данных


## Требования

- .NET 8.0 SDK
- PostgreSQL
- IDE (Visual Studio, VS Code)

## Установка и запуск

### 1. Клонирование репозитория

```bash
git clone https://github.com/StarlighTeRR/MusicApp.git
cd MusicApp/MusicAppApi
```

### 2. Настройка базы данных

Создайте базу данных PostgreSQL и обновите строку подключения в `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=musicapp;Username=your_username;Password=your_password"
  }
}
```
### 3. Применение миграций

```bash
dotnet ef database update
```

### 4. Запуск приложения

```bash
dotnet run
```

После запуска API будет доступен по адресу: `http://localhost:5298`

## API Endpoints

- `GET /api/music` - Получить список музыкальных треков
- `GET /api/music/{id}` - Получить трек по ID
- `POST /api/music` - Добавить новый трек
- `PUT /api/music/{id}` - Обновить существующий трек
- `DELETE /api/music/{id}` - Удалить трек


```

# Музыкальное приложение - Клиентская часть

Клиентская часть приложения для просмотра информации о музыкантах, их альбомах и треках.


## Технологии

- React.js
- JavaScript (ES6+)
- CSS
- Fetch API для работы с бэкендом

## Установка

1. Убедитесь, что у вас установлен Node.js и npm

   ```
2. Перейдите в директорию проекта:
   ```bash
   cd music_app_react
   ```
3. Установите зависимости:
   ```bash
   npm install
   ```

## Запуск

Для запуска приложения в режиме разработки:

```bash
npm start
```
Приложение будет доступно по адресу [http://localhost:3000](http://localhost:3000).









