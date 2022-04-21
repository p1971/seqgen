FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY / ./

RUN dotnet build "Sequence.Generator.sln" -c Release

FROM build AS publish
RUN dotnet publish "Sequence.Generator.WebApi/Sequence.Generator.WebApi.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Sequence.Generator.WebApi.dll"]