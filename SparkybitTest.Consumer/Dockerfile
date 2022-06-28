FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SparkybitTest.Consumer/SparkybitTest.Consumer.csproj", "SparkybitTest.Consumer/"]
RUN dotnet restore "SparkybitTest.Consumer/SparkybitTest.Consumer.csproj"
COPY . .
WORKDIR "/src/SparkybitTest.Consumer"
RUN dotnet build "SparkybitTest.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SparkybitTest.Consumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SparkybitTest.Consumer.dll"]
