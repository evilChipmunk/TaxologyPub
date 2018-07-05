FROM microsoft/aspnetcore:2.0-nanoserver-sac2016 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-sac2016 AS build
WORKDIR /src
COPY Taxology.sln ./
COPY Customer.Service/Customer.Service.csproj Customer.Service/
COPY Customer.Domain/Customer.Domain.csproj Customer.Domain/
COPY Shared.Domain/Shared.Domain.csproj Shared.Domain/
COPY Shared.Service/Shared.Service.csproj Shared.Service/
COPY Shared.DTO/Shared.DTO.csproj Shared.DTO/
COPY Customer.DTO/Customer.DTO.csproj Customer.DTO/
COPY Customer.Persistency/Customer.Persistency.csproj Customer.Persistency/
COPY Shared.Persistency/Shared.Persistency.csproj Shared.Persistency/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/Customer.Service
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Customer.Service.dll"]
