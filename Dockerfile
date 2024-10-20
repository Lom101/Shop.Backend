# Используем официальный образ .NET для сборки и публикации
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5432

# Используем образ .NET SDK для сборки приложения
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем файл решения и файлы проектов
COPY ["Shop.WebApi/Shop.WebAPI.csproj", "Shop.WebAPI/"]
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

# Указываем команду запуска
ENTRYPOINT ["dotnet", "Shop.WebAPI.dll"]


## Используйте официальный .NET Core SDK в качестве родительского образа
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build 
#WORKDIR /app 
#
## Скопируйте файл проекта и восстановите все зависимости (используйте .csproj для имени проекта)
#COPY *.csproj ./ 
#RUN dotnet restore 
#
## Скопируйте остальной код приложения
#COPY . . 
#
## Опубликуйте приложение
#RUN dotnet publish -c Release -o out 
#
## Соберите образ среды выполнения
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime 
#WORKDIR /app 
#COPY --from=build /app/out ./ 
#
## Предоставьте порт, на котором будет работать ваше приложение
#EXPOSE 80
#EXPOSE 5432
#
## Запустите приложение
#ENTRYPOINT [ "dotnet" , "Shop.WebAPI.dll" ]