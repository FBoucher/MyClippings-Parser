FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["MyClipping-Parser.API/MyClippings-Parser.csproj", "MyClipping-Parser.API/"]
RUN dotnet restore "MyClipping-Parser.API/MyClippings-Parser.csproj"
COPY . .
WORKDIR "/src/MyClipping-Parser.API"
RUN dotnet build "MyClippings-Parser.csproj" -c Release -o /app

FROM build AS tester
WORKDIR /src/MyClipping-Parser.Tests
COPY ["MyClipping-Parser.Tests/MyClipping-Parser.Tests.csproj", "MyClipping-Parser.Tests/"]
ENTRYPOINT ["dotnet", "test", "--logger:trx"]

FROM build AS publish
RUN dotnet publish "MyClippings-Parser.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MyClippings-Parser.dll"]
