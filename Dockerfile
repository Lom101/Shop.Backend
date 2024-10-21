# Используем официальный образ .NET для сборки и публикации
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Используем образ .NET SDK для сборки приложения
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем файл решения и файлы проектов
COPY ["Shop.WebAPI/Shop.WebAPI.csproj", "Shop.WebAPI/"]
COPY ["Shop.Tests/Shop.Tests.csproj", "Shop.Tests/"]

# Восстанавливаем зависимости
RUN dotnet restore "Shop.WebAPI/Shop.WebAPI.csproj"

# Копируем остальные файлы проекта и собираем приложение
COPY . .
WORKDIR "/src/Shop.WebAPI"
RUN dotnet build "Shop.WebAPI.csproj" -c Release -o /app/build

# Публикуем приложение
FROM build AS publish
RUN dotnet publish "Shop.WebAPI.csproj" -c Release -o /app/publish

# Создаем окончательный образ с ASP.NET Core runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 80
EXPOSE 443
EXPOSE 5432

# Указываем команду запуска
ENTRYPOINT ["dotnet", "Shop.WebAPI.dll"]