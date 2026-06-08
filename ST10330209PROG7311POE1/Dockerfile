FROM mcr.microsoft.com/dotnet/nightly/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/nightly/sdk:10.0-preview AS build
WORKDIR /src
COPY ["ST10330209PROG7311POE1/ST10330209PROG7311POE1.csproj", "ST10330209PROG7311POE1/"]
RUN dotnet restore "ST10330209PROG7311POE1/ST10330209PROG7311POE1.csproj"
COPY . .
WORKDIR "/src/ST10330209PROG7311POE1"
RUN dotnet build "ST10330209PROG7311POE1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ST10330209PROG7311POE1.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ST10330209PROG7311POE1.dll"]