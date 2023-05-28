FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app


COPY "ecpay.sln" "ecpay.sln"
COPY "ecpay.csproj" "ecpay.csproj"

RUN dotnet restore "ecpay.sln"

COPY . .
WORKDIR /app
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "ecpay.dll"]