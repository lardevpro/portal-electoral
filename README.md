# PortalElectoral API

**PortalElectoral** es una API REST desarrollada en **.NET 8 / .NET Core** diseñada para gestionar y centralizar la información de procesos electorales. El sistema permite administrar partidos políticos, candidaturas, propuestas, provincias y realizar el seguimiento estadístico de los resultados de las elecciones.

## 🚀 Características principales

* **Gestión de Elecciones:** Creación, actualización y consulta de diferentes procesos electorales.
* **Partidos y Candidaturas:** Registro de partidos políticos junto a sus respectivos candidatos y listas de candidaturas.
* **Propuestas Electorales:** Vinculación de propuestas programáticas a partidos o candidatos.
* **División Territorial:** Control de datos orientados a provincias para segmentar los escrutinios.
* **Estadísticas y Resultados:** Endpoints especializados para el cálculo y visualización del avance de votos y resultados.
* **Sembrado de Datos:** Incluye un seeder automático (`DemoDataSeeder`) con datos de prueba preconfigurados para entornos de desarrollo.

## 🛠️ Tecnologías utilizadas

* **Backend:** .NET 8 / Web API (C#)
* **Persistencia:** Entity Framework Core con enfoque Code-First
* **Base de Datos:** SQL Server / PostgreSQL (configurable en el DbContext)
* **Arquitectura:** Controladores (Controllers) con transferencia de datos optimizada mediante DTOs (Data Transfer Objects).

## 📂 Estructura del Proyecto

A grandes rasgos, la arquitectura del directorio `PortalElectoral` se organiza de la siguiente manera:

```text
PortalElectoral/
│
├── Controllers/          # Controladores de la API (Candidaturas, Elecciones, Estadísticas, etc.)
├── Data/                 # Contexto de Base de Datos (ElectoralDbContext) y Sembrador de datos de demo
├── DTOs/                 # Objetos de Transferencia de Datos para peticiones y respuestas estructuradas
├── Migrations/           # Historial de migraciones de Entity Framework Core
├── Models/               # Entidades de dominio del modelo de base de datos
├── appsettings.json      # Archivo de configuración (Cadenas de conexión, variables)
└── Program.cs            # Punto de entrada de la aplicación y configuración de servicios
```

## ⚙️ Requisitos Previos

* **.NET 8 SDK** instalado en tu equipo.
* Motor de base de datos compatible (como SQL Server) en ejecución.
* IDE recomendado: **Visual Studio 2022** o **VS Code** con la extensión de C#.

## 🛠️ Instalación y Configuración

1. **Clonar el repositorio:**
   ```bash
   git clone https://github.com
   cd PortalElectoral
   ```

2. **Configurar la base de datos:**
   Abre el archivo `appsettings.json` y edita la cadena de conexión (`ConnectionStrings`) con las credenciales de tu servidor local.

3. **Aplicar las migraciones a la base de datos:**
   Si utilizas la Consola de Administrador de Paquetes en Visual Studio:
   ```powershell
   Update-Database
   ```
   O mediante la CLI de .NET en tu terminal:
   ```bash
   dotnet ef database update
   ```

4. **Ejecutar la aplicación:**
   ```bash
   dotnet run
   ```
   La API levantará por defecto y podrás consultar los endpoints disponibles (o acceder a la interfaz de Swagger si está configurada en `Program.cs`) ingresando a la URL local indicada en la consola.
