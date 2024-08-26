# Используем образ .NET SDK для сборки и выполнения приложения
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Устанавливаем рабочую директорию
WORKDIR /app

# Копируем файлы проекта и восстанавливаем зависимости
COPY ["src/backends/RESTApiService/RESTApiService.csproj", "src/backends/RESTApiService/"]
COPY ["src/shared/shared.csproj", "src/shared/"]
RUN dotnet restore "src/backends/RESTApiService/RESTApiService.csproj"

# Копируем оставшийся исходный код и собираем приложение
COPY . .
WORKDIR "/app/src/backends/RESTApiService"
RUN dotnet build "RESTApiService.csproj" -c Release

# Запускаем приложение
CMD ["dotnet", "run", "--no-launch-profile", "--urls", "http://0.0.0.0:5290"]
