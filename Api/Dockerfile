# استفاده از ایمیج ASP.NET Core 8.0 به عنوان Base Image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# نصب ابزارهای اضافی در Base Image
USER root
RUN apt-get update && apt-get install -y \
    curl \
    net-tools && \
    rm -rf /var/lib/apt/lists/*

# تغییر کاربر به یک کاربر غیر root برای امنیت بیشتر
USER app

# استفاده از SDK برای ساخت پروژه
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILDCONFIGURATION=Release
WORKDIR /src

# کپی فایل‌های پروژه‌های وابسته و اصلی
COPY ./Api/Api.csproj ./Api/
COPY ./Infrastructure/Infrastructure.csproj ./Infrastructure/

# بازیابی وابستگی‌ها
WORKDIR /src/Api
RUN dotnet restore

# کپی کل فایل‌های پروژه
COPY . .

# ساخت پروژه
RUN dotnet build -c $BUILDCONFIGURATION -o /app/build

# مرحله انتشار (Publish)
FROM build AS publish
ARG BUILDCONFIGURATION=Release
RUN dotnet publish -c $BUILDCONFIGURATION -o /app/publish /p:UseAppHost=false

# ایمیج نهایی بر پایه Base Image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]
