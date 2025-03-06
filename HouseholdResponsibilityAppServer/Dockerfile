# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o /out

RUN dotnet tool install --global dotnet-ef

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .
EXPOSE 5000
ENV PATH="${PATH}:/root/.dotnet/tools"
CMD ["dotnet", "HouseholdResponsibilityAppServer.dll"]

