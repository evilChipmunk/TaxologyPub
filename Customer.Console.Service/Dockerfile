FROM microsoft/dotnet:2.0-runtime-nanoserver-sac2016 AS base
WORKDIR /app

FROM microsoft/dotnet:2.0-sdk-nanoserver-sac2016 AS build
WORKDIR /src
COPY Taxology.sln ./
COPY Customer.Console.Service/Customer.Console.Service.csproj Customer.Console.Service/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/Customer.Console.Service
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Customer.Console.Service.dll"]
