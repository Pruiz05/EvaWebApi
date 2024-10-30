# Limpia la carpeta de compilación anterior
dotnet clean

# Compila el proyecto
dotnet build

# Corre pruebas (si existen)
dotnet test

# Publica (si es necesario)
dotnet publish -c Release
