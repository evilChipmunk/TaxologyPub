FROM microsoft/aspnetcore:2.0-nanoserver-sac2016 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-sac2016 AS build
WORKDIR /src
COPY Taxology.sln ./
COPY Order.Service/Order.Service.csproj Order.Service/
COPY Shared.Service/Shared.Service.csproj Shared.Service/
COPY Shared.DTO/Shared.DTO.csproj Shared.DTO/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/Order.Service
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Order.Service.dll"]
