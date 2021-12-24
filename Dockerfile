FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

#Install dependencies
COPY *.csproj ./
RUN dotnet restore

#Build application
COPY . ./
RUN dotnet publish -c Release -o out


# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "BLMS.dll"]
