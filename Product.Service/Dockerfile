FROM microsoft/aspnetcore:2.0-nanoserver-sac2016 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-sac2016 AS build
WORKDIR /src
COPY Taxology.sln ./
COPY Product.Service/Product.Service.csproj Product.Service/
COPY Product.Domain/Product.Domain.csproj Product.Domain/
COPY Customer.DTO/Customer.DTO.csproj Customer.DTO/
COPY Shared.DTO/Shared.DTO.csproj Shared.DTO/
COPY Shared.Domain/Shared.Domain.csproj Shared.Domain/
COPY Shared.Service/Shared.Service.csproj Shared.Service/
COPY Product.DTO/Product.DTO.csproj Product.DTO/
COPY Product.Persistency/Product.Persistency.csproj Product.Persistency/
COPY Shared.Persistency/Shared.Persistency.csproj Shared.Persistency/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/Product.Service
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Product.Service.dll"]
