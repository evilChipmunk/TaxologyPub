FROM microsoft/aspnetcore:2.0-nanoserver-sac2016 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-sac2016 AS build
WORKDIR /src
COPY Taxology.sln ./
COPY Taxology.Site/Taxology.Site.csproj Taxology.Site/
COPY Order.DTO/Order.DTO.csproj Order.DTO/
COPY Shared.DTO/Shared.DTO.csproj Shared.DTO/
COPY Shared.Service/Shared.Service.csproj Shared.Service/
COPY Customer.DTO/Customer.DTO.csproj Customer.DTO/
COPY Product.DTO/Product.DTO.csproj Product.DTO/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/Taxology.Site
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Taxology.Site.dll"]
