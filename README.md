# Backend-Portafolio
# Backend para Portafolio

Este proyecto es un **backend API** para un **portafolio personal**. Está construido usando **ASP.NET Core** con soporte para autenticación JWT y operaciones CRUD para manejar las tablas de **Proyectos**, **Usuarios**, **Skills**, **Experiencia** y **Mensaje**.

## Tecnologías Utilizadas
- **ASP.NET Core** para crear la API RESTful.
- **JWT (JSON Web Token)** para la autenticación y autorización.
- **Entity Framework Core** para interactuar con la base de datos.
- **SQL Server** como base de datos (puedes usar otro proveedor de base de datos si lo prefieres).
- **CORS (Cross-Origin Resource Sharing)** configurado para permitir solicitudes desde diferentes orígenes (útil para el frontend).

## Tablas del Proyecto
- **Proyectos**: Información sobre los proyectos que has desarrollado.
- **Usuarios**: Datos de los usuarios, con roles y permisos.
- **Skills**: Lista de habilidades y tecnologías que conoces.
- **Experiencia**: Información sobre tu experiencia laboral o académica.
- **Mensaje**: Mensajes enviados por los visitantes de tu portafolio.

## Endpoints Disponibles
### 1. **Proyectos**
- **GET** `/api/Proyectos`: Obtener todos los proyectos.
- **GET** `/api/Proyectos/{id}`: Obtener un proyecto específico por ID.
- **POST** `/api/Proyectos`: Crear un nuevo proyecto.
- **PUT** `/api/Proyectos/{id}`: Actualizar un proyecto existente.
- **DELETE** `/api/Proyectos/{id}`: Eliminar un proyecto.

### 2. **Usuarios**
- **GET** `/api/Usuarios`: Obtener todos los usuarios.
- **GET** `/api/Usuarios/{id}`: Obtener un usuario específico por ID.
- **POST** `/api/Usuarios`: Crear un nuevo usuario (registro).
- **PUT** `/api/Usuarios/{id}`: Actualizar un usuario existente.
- **DELETE** `/api/Usuarios/{id}`: Eliminar un usuario.

### 3. **Skills**
- **GET** `/api/Skills`: Obtener todas las habilidades.
- **GET** `/api/Skills?id={id}`: Obtener una habilidad por ID.
- **POST** `/api/Skills`: Crear una nueva habilidad.
- **PUT** `/api/Skills/{id}`: Actualizar una habilidad existente.
- **DELETE** `/api/Skills/{id}`: Eliminar una habilidad.

### 4. **Experiencia**
- **GET** `/api/Experiencia`: Obtener toda la experiencia.
- **GET** `/api/Experiencia/{id}`: Obtener experiencia por ID.
- **POST** `/api/Experiencia`: Crear nueva experiencia.
- **PUT** `/api/Experiencia/{id}`: Actualizar experiencia existente.
- **DELETE** `/api/Experiencia/{id}`: Eliminar experiencia.

### 5. **Mensaje**
- **POST** `/api/Mensaje`: Enviar un mensaje de contacto.

## Autenticación con JWT
El backend utiliza **JSON Web Tokens (JWT)** para autenticar las solicitudes. Los pasos para obtener un token son los siguientes:

1. **Registrar un usuario**: Utiliza el endpoint `POST /api/Usuarios` para crear un nuevo usuario.
2. **Iniciar sesión**: Usa `POST /api/Usuarios/Login` para obtener un token JWT proporcionando tus credenciales.
3. **Enviar solicitudes protegidas**: En las solicitudes que requieran autenticación, agrega el token JWT en los encabezados de la solicitud como sigue:
    ```bash
    Authorization: Bearer <tu-token-jwt>
    ```

## Levantar el servidor
El servidor está configurado para ejecutarse en el puerto **7600**. Para levantar el servidor, sigue estos pasos:

1. **Clona el repositorio**:
    ```bash
    git clone https://github.com/tu_usuario/Backend-Portafolio.git
    ```

2. **Navega a la carpeta del proyecto**:
    ```bash
    cd Backend-Portafolio
    ```

3. **Restaura las dependencias**:
    Si es la primera vez que ejecutas el proyecto, necesitarás restaurar las dependencias:
    ```bash
    dotnet restore
    ```

4. **Configura la base de datos**:
    Asegúrate de tener la base de datos configurada correctamente en el archivo `appsettings.json`:
    ```json
    "ConnectionStrings": {
        "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PortfolioDb;Trusted_Connection=True;"
    }
    ```

5. **Ejecuta las migraciones (si es necesario)**:
    Si has realizado cambios en la base de datos, ejecuta las migraciones para aplicarlas:
    ```bash
    dotnet ef database update
    ```

6. **Levanta el servidor**:
    Una vez que hayas configurado todo, ejecuta el siguiente comando para levantar el servidor:
    ```bash
    dotnet run
    ```

7. **Accede a la API**:
    El backend estará disponible en `http://localhost:7600`. Puedes usar herramientas como **Postman** o **Insomnia** para hacer pruebas con los endpoints.

## Entorno de Desarrollo
- **Visual Studio Code** o **Visual Studio** para editar el código.
- **Postman** o cualquier otra herramienta similar para probar los endpoints.
- **SQL Server Management Studio (SSMS)** o cualquier cliente SQL para gestionar la base de datos.

## Autor
Juan Cazas

## Licencia
Este proyecto está bajo la licencia MIT - ver el archivo [LICENSE](LICENSE) para más detalles.
