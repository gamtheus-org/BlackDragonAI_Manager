FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["BlackDragonAI_Manager/BlackDragonAI_Manager.csproj", "BlackDragonAI_Manager/"]
RUN dotnet restore "BlackDragonAI_Manager/BlackDragonAI_Manager.csproj"
COPY . .
RUN dotnet build "BlackDragonAI_Manager/BlackDragonAI_Manager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlackDragonAI_Manager/BlackDragonAI_Manager.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM nginx:latest AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf

EXPOSE 80