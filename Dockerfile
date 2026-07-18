FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["TaskTracker/TaskTracker.API.csproj", "TaskTracker/"]
COPY ["TaskTracker.Tests/TaskTracker.Tests.csproj", "TaskTracker.Tests/"]

RUN dotnet restore "TaskTracker.API/TaskTracker.API.csproj"

COPY . .

WORKDIR "/src/TaskTracker.API"
RUN dotnet build "TaskTracker.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaskTracker.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskTracker.API.dll"]