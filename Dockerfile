# publish project
FROM mcr.microsoft.com/dotnet/sdk:9.0@sha256:087fc98e5c6ffcea6c3e276c135c4a6717c589d9509a09cc22e7c634830a4db8 AS build
WORKDIR /opt/todo-api

COPY . .

RUN dotnet restore

WORKDIR ToDo.API

RUN dotnet build
RUN dotnet publish -r linux-x64 -o publish

# build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0@sha256:76b7df121eacae21642e94d70d7211b3865bab808ce51a5bbac5b85e5f196a2f
WORKDIR /opt/todo-api

COPY --from=build /opt/todo-api/ToDo.API/publish .

EXPOSE 8080/tcp

ENTRYPOINT ["dotnet", "ToDo.API.dll"]