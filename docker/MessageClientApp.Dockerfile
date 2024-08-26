# Используем образ .NET SDK для сборки и выполнения приложения
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Устанавливаем рабочую директорию
WORKDIR /app

# Копируем файлы проекта и восстанавливаем зависимости
COPY ["src/frontends/MessageClientApp/MessageClientApp.csproj", "src/frontends/MessageClientApp/"]
COPY ["src/shared/shared.csproj", "src/shared/"]
RUN dotnet restore "src/frontends/MessageClientApp/MessageClientApp.csproj"

# Копируем оставшийся исходный код и собираем приложение
COPY . .
WORKDIR "/app/src/frontends/MessageClientApp"
RUN dotnet build "MessageClientApp.csproj" -c Release

# Запускаем приложение
CMD ["dotnet", "run", "--no-launch-profile", "--urls", "http://0.0.0.0:5240"]
