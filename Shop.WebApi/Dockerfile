# Используйте официальный .NET Core SDK в качестве родительского образа
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build 
WORKDIR /app 

# Скопируйте файл проекта и восстановите все зависимости (используйте .csproj для имени проекта)
COPY *.csproj ./ 
RUN dotnet restore 

# Скопируйте остальной код приложения
COPY . . 

# Опубликуйте приложение
RUN dotnet publish -c Release -o out 

# Соберите образ среды выполнения
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime 
WORKDIR /app 
COPY --from=build /app/out ./ 

# Предоставьте порт, на котором будет работать ваше приложение
EXPOSE 80
EXPOSE 5432

# Запустите приложение
ENTRYPOINT [ "dotnet" , "Shop.WebAPI.dll" ]