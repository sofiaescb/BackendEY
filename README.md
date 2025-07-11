# BackendEY
Este proyecto consiste en una API REST desarrollada con .NET 8 que permite:

- Realizar **screening automático** mediante web scraping en listas de alto riesgo (como OFAC y World Bank).
- Administrar un **CRUD completo de proveedores**.

## 🧩 Tecnologías utilizadas

- ASP.NET Core 8 Web API
- C#
- HtmlAgilityPack (para scraping)
- Entity Framework Core (EF Core)
- SQL Server
- Middleware de autenticación por token (solo para `/api/screening`)

## 🚀 Instrucciones para desplegar la solución
Esta API ha sido desplegada exitosamente en un entorno accesible desde:
```
http://13.222.89.27:8080
```

Se pueden probar los endpoints directamente en esa dirección, sin necesidad de ejecutarlo localmente.

Sin embargo, también se puede ejecutar localmente siguiendo los pasos a continuación:

### 1. Clonar el repositorio

```bash
git clone https://github.com/sofiaescb/BackendEY.git
cd BackendEY
```

### 2. Configurar la base de datos

Usar el script SQL disponible en la raíz del proyecto llamado:

```
CreacionTablaProveedores.sql
```

Ejecuta el script en SQL Server para crear la base y tablas necesarias.

### 3. Configurar `appsettings.json`

El archivo appsettings.json ya está configurado en el proyecto con la cadena de conexión y el ApiToken necesario para el endpoint /api/screening. Es modificable por si se desea cambiar el token o los datos de la base de datos.

### 4. Ejecutar el servidor

```bash
dotnet run
```

Por defecto, la API estará disponible en:

```
https://localhost:7267
```

## 📮 Endpoints disponibles

### 🔎 1. Web Scraping `/api/screening` (requiere token)

Realiza una búsqueda web en una de las fuentes de listas de riesgo.

```
GET /api/screening?Fuente=ofac&Proveedor=NombreEmpresa
```

- **Headers:**
  - `Authorization: Bearer mi-token-secreto`

- **Parámetros:**
  - `Fuente`: ofac o worldbank
  - `Proveedor`: Nombre de la entidad a buscar

#### 📌 Respuesta esperada:

```json
{
  "fuente": ofac,
  "totalResultados":2,
  "resultados": [
    {
      "Name": DISTRIBUIDORA NICARAGUENSE DE PETROLEO, S.A.",
      "Address": "Ofiplaza El Retiro Edificio 8; Segundo Piso",
      "Type": "Entity",
      "Programas": "NICARAGUA",
      "List":"SDN",
      "Score":100
    },
    ...
  ],
  "error":null
}
```

#### Postman:
<img width="1359" height="870" alt="image" src="https://github.com/user-attachments/assets/c2713b32-6d26-4e5d-8f6c-75d67d4ba8be" />

---

### 👥 2. CRUD de Proveedores (sin autenticación)

#### Crear proveedor
```
POST /api/proveedores
```
<img width="1356" height="885" alt="image" src="https://github.com/user-attachments/assets/be9dc6b5-1b82-4fe4-82ef-4f5282eff36c" />
<img width="1365" height="840" alt="image" src="https://github.com/user-attachments/assets/0c0efdd4-176c-43ce-a8d4-63033c61e454" />

#### Obtener todos
```
GET /api/proveedores
```
<img width="1355" height="862" alt="image" src="https://github.com/user-attachments/assets/0c685c09-ece3-49bc-b6b1-34244a02b36d" />
<img width="1368" height="950" alt="image" src="https://github.com/user-attachments/assets/0735a864-000c-4307-a521-82f965c4ebb8" />


#### Obtener por ID
```
GET /api/proveedores/{id}
```
<img width="1369" height="557" alt="image" src="https://github.com/user-attachments/assets/bd65b2b1-2ac1-4b48-b501-09206949fdfa" />
<img width="1361" height="650" alt="image" src="https://github.com/user-attachments/assets/16c90d28-2a7b-49cd-9b70-873b08db24d3" />


#### Editar proveedor
```
PUT /api/proveedores/{id}
```
<img width="1371" height="674" alt="image" src="https://github.com/user-attachments/assets/96720f0c-ce0d-40a1-b529-15d4c6660b06" />


#### Eliminar proveedor
```
DELETE /api/proveedores/{id}
```
<img width="1359" height="378" alt="image" src="https://github.com/user-attachments/assets/18d1589d-b823-4537-a3cc-2848d5dc4ab5" />


---

## 📁 Estructura del proyecto

```
BackendEY/
│
├── Controllers/
├── Services/
├── DTOs/
├── Data/
├── Models/
├── appsettings.json
└── Program.cs
```

## 🧪 Recomendaciones de prueba

- Usa [Postman](https://www.postman.com/) para probar los endpoints.
- No olvides agregar el token en el header para `/api/screening`.
- Para el CRUD no se necesita autenticación.
---
## 📥 Colección de Postman

Puedes probar todos los endpoints usando la siguiente colección:

➡️ [Ver colección Postman](https://web.postman.co/workspace/27c81036-e4a4-45a1-813d-0294effd4410/collection/36962241-b980cc26-ff7e-487f-882c-bebb73e2312b?action=share&source=copy-link&creator=36962241)


## 📜 Licencia

Repositorio creado como parte de una prueba técnica para EY – FY26.
