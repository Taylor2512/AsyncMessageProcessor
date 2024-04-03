# Establecer la imagen base para la construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Establecer el directorio de trabajo
WORKDIR /src

# Instalar libicu-dev (si es necesario)
RUN apt-get update && apt-get install -y --no-install-recommends \
    libicu-dev \
 && rm -rf /var/lib/apt/lists/*

# Copiar los archivos del proyecto y restaurar las dependencias
COPY ./AsyncMessageProcessor ./
RUN dotnet restore "AsyncMessageProcessor.WorkerService/AsyncMessageProcessor.WorkerService.csproj"

# Compilar y publicar la aplicación
RUN dotnet publish "AsyncMessageProcessor.WorkerService/AsyncMessageProcessor.WorkerService.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Establecer la imagen base final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

# Establecer el directorio de trabajo
WORKDIR /app

# Copiar los archivos publicados desde la etapa de construcción
COPY --from=build /app/publish .

# Establecer el punto de entrada de la aplicación
ENTRYPOINT ["dotnet", "AsyncMessageProcessor.WorkerService.dll"]
