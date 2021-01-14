FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY . /src
RUN dotnet build web.csproj -c Release -o /build/
RUN dotnet publish web.csproj -c Release -r alpine-x64 --self-contained true /p:PublishTrimmed=true -o /publish/

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine3.12 AS final
WORKDIR /bin
COPY --from=build /publish/ ./
ENTRYPOINT ["dotnet", "web.dll"]
EXPOSE 80
