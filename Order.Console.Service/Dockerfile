FROM microsoft/dotnet:2.0-runtime-nanoserver-sac2016 AS base
WORKDIR /app

FROM microsoft/dotnet:2.0-sdk-nanoserver-sac2016 AS build
WORKDIR /src
COPY Taxology.sln ./
COPY Order.Console.Service/Order.Console.Service.csproj Order.Console.Service/
COPY Order.Persistency/Order.Persistency.csproj Order.Persistency/
COPY Shared.Persistency/Shared.Persistency.csproj Shared.Persistency/
COPY Shared.Domain/Shared.Domain.csproj Shared.Domain/
COPY Order.Domain/Order.Domain.csproj Order.Domain/
COPY Order.DTO/Order.DTO.csproj Order.DTO/
COPY Shared.DTO/Shared.DTO.csproj Shared.DTO/
COPY Shared.Service/Shared.Service.csproj Shared.Service/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/Order.Console.Service
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Order.Console.Service.dll"]
