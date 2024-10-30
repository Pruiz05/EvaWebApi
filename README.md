# User Api Restfull

Api Restfull para el proyecto de Evaluación.

## Requerimientos

- Visual Studio 2022
- SQL Server
- .Net 6+

## Instalación

1. Clonar el repositorio de Github
2. Abrir la carpeta del proyecto en Visual Studio
3. Ejecutar en el administrador de base de datos el script `Scripts/db_script.sql`
4. Configurar las variables de entorno en el archivo `appsettings.json`. Cambiar la conexión de la base de datos y la clave de encriptación `Servidor_SQL` por el servidor de SQL Server.
5. Ejecutar el proyecto con `dotnet run`
6. Abrir Swagger UI en `http://localhost:5022/swagger`
7. Ejecutar el endpoint `POST /api/User` para registrar un usuario.
8. Ejecutar el endpoint `GET /api/User` para consultar los usuarios - Se debe enviar el token JWT que se genera al registrar un usuario ya que esta api es privada.

## Uso

1. Limpia la carpeta de compilación anterior

```bash
dotnet clean
```

2. Compila el proyecto

```bash
dotnet build
```

3. Ejecutar el proyecto en Visual Studio Code:

```bash
dotnet run --project API
```

Ejecutar los tests unitarios:

```bash
dotnet test EvaUnitTest
```

## Paquetes utilizados

- [Entity Framework Core](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools/9.0.0-rc.2.24474.1)
- [Entity Framework Core Sql Server](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/9.0.0-rc.2.24474.1)
- [Fluent Validation](https://www.nuget.org/packages/FluentValidation/)
- [BCrypt.Net-Next](https://www.nuget.org/packages/BCrypt.Net-Next/)
- [Swashbuckle](https://www.nuget.org/packages/Swashbuckle.AspNetCore/)

### Registro

Para registrar un usuario se debe enviar un POST con los siguientes campos:

```
http://localhost:5022/api/User
```

```json
{
  "name": "pedro rodriguez",
  "email": "prodriguez@gmail.com",
  "password": "Prueba123@*_2024",
  "phones": [
    {
      "number": "1234567",
      "cityCode": "1",
      "countryCode": "57"
    }
  ]
}
```

### Consulta de usuarios

Para consultar los usuarios se debe enviar un GET, con el token JWT que se crea al registrar un usuario.

```
http://localhost:5022/api/User
```

```curl
curl -X 'GET' \
  'http://localhost:5022/api/User' \
  -H 'accept: application/json' \
  -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6Ik1hZHlzb245NkBnbWFpbC5jb20iLCJ1bmlxdWVfbmFtZSI6IkthcmxhIFBhZ2FjIiwiaWQiOiIyNjYzNjZiOC1lOTQ2LTRjYWEtYjM5Yy1lZTEyYWYwMDkzZGYiLCJuYW1lIjoiS2FybGEgUGFnYWMiLCJuYmYiOjE3MzAyNDQ4NTQsImV4cCI6MTczMDI0NjY1NCwiaWF0IjoxNzMwMjQ0ODU0fQ.R1Tul_QMeASHtAdsdmlDsVD30kgvzbjIuZCYvP65a5Y'
```

## Screenshots

### Diagrama de solución

![Diagrama de solución](image-2.png)

![Swagger UI](image.png)

![Postman UI](image-1.png)

## Estructura de directorios

- **API**: Proyecto de la API Restfull
- **Scripts**: Scripts para crear la base de datos y las tablas necesarias.
- **UnitTest**: Pruebas unitarias

## Licencia

Este proyecto está licenciado bajo licencia **MIT**, si desea saber más, visite el fichero [LICENSE](./LICENSE) para su uso docente y educativo.
