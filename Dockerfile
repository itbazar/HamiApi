# مرحله پایه
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# مرحله بیلد
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILDCONFIGURATION=Release
WORKDIR /src
COPY ["Api/Api.csproj", "Api/"]
RUN dotnet restore "./Api/./Api.csproj"
COPY . .
WORKDIR "/src/Api"
RUN dotnet build "./Api.csproj" -c $BUILDCONFIGURATION -o /app/build

# مرحله پابلیش
FROM build AS publish
ARG BUILDCONFIGURATION=Release
RUN dotnet publish "./Api.csproj" -c $BUILDCONFIGURATION -o /app/publish /p:UseAppHost=false

# مرحله نهایی
FROM base AS final
WORKDIR /app

# نصب ابزارها در حالت root
USER root
RUN apt-get update && \
    apt-get install -y curl iputils-ping net-tools && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# تغییر وضعیت به کاربر غیر ریشه‌ای
USER app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]
