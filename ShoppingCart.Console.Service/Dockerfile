FROM microsoft/dotnet:2.0-runtime-nanoserver-sac2016 AS base
WORKDIR /app

FROM microsoft/dotnet:2.0-sdk-nanoserver-sac2016 AS build
WORKDIR /src
COPY Taxology.sln ./
COPY ShoppingCart.Console.Service/ShoppingCart.Console.Service.csproj ShoppingCart.Console.Service/
COPY ShoppingCart.DTO/ShoppingCart.DTO.csproj ShoppingCart.DTO/
COPY Shared.DTO/Shared.DTO.csproj Shared.DTO/
COPY ShoppingCart.Persistency/ShoppingCart.Persistency.csproj ShoppingCart.Persistency/
COPY Shared.Persistency/Shared.Persistency.csproj Shared.Persistency/
COPY Shared.Domain/Shared.Domain.csproj Shared.Domain/
COPY ShoppingCart.Domain/ShoppingCart.Domain.csproj ShoppingCart.Domain/
COPY Shared.Service/Shared.Service.csproj Shared.Service/
COPY Customer.DTO/Customer.DTO.csproj Customer.DTO/
COPY Product.DTO/Product.DTO.csproj Product.DTO/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/ShoppingCart.Console.Service
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ShoppingCart.Console.Service.dll"]
