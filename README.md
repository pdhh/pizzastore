# Introducción

La creación de una API web es una tarea común. Es recomendable poder ofrecer algunos datos y saber cómo los consume una aplicación o servicio. La forma de compilar la API puede diferir enormemente entre las pilas tecnológicas. A la hora de compilar una API, sabe que son muchas las partes que hay que tener en cuenta, como el almacenamiento de datos, la seguridad, el control de versiones y la documentación. Conseguir que todas estas partes funcionen puede ser una tarea compleja.

Escenario: Creación de un prototipo
Usted es desarrollador en un equipo. Como parte de su trabajo, compila y mantiene muchas API. También experimenta un poco con nuevas tecnologías para determinar si son una buena opción para las necesidades de la empresa. Le gustan los marcos que le permiten crear una API con solo unas pocas líneas de código, ya que un resultado rápido facilita una buena conversación con compañeros y otras partes interesadas. Se supone que puede agregar más características más adelante a medida que la API crece en complejidad.

¿Qué aprenderá?
Aprenderá a compilar lo que se conoce como una API mínima mediante ASP.NET Core y .NET 6. Como parte de la creación de la API, agregará varias construcciones de ruta para controlar la lectura y escritura de datos. También agregará Swagger para asegurarse de que tiene una manera de documentar la API.

¿Cuál es el objetivo principal?
Cree una API eficaz que admita la lectura y la escritura con solo unas pocas líneas de código.

## ¿Qué es una API mínima?

Si ha desarrollado una API web de .NET, ha estado usando un enfoque con controladores. La idea es tener un método de clase de controlador, que representa varios verbos HTTP, y realizar una operación para completar una tarea específica. Por ejemplo,**_GetProducts()_** devolvería productos mediante GET como verbo HTTP.

¿Cuál es la diferencia entre este enfoque basado en controlador y la API mínima?

Program.cs simplificado: la plantilla de la API web basada en controlador conecta los controladores mediante el método **_ AddControllers_**. Además, conecta Swagger para proporcionar compatibilidad con OpenAPI. Las API mínimas no tienen esta conexión de forma predeterminada, aunque puede agregar Swagger si lo desea.

El enrutamiento tiene un aspecto algo diferente: el enrutamiento tiene un aspecto ligeramente diferente en comparación con una API web basada en controlador. En una API web, para el enrutamiento, se debe escribir el código como se muestra a continuación:

## ¿Cómo se empieza?

### Creación de una API con una API mínima

Con .NET 6 instalado, en este ejemplo de comando se crea un proyecto de API mínima:

```bash
dotnet new web -o PizzaStore -f net6.0
```

#### La carpeta PizzaStore recién creada contiene el proyecto de API

Inspección de los archivos
Los archivos generados son muy parecidos a los que se obtendrían con una API basada en controlador:

```bash
bin/
obj/
appsettings.Development.json
appsettings.json
PizzaStore.csproj
Program.cs
```

Dentro de PizzaStore.csproj, hay una entrada como esta:

```xml
<PropertyGroup>
  <TargetFramework>net6.0</TargetFramework>
  <Nullable>enable</Nullable>
</PropertyGroup>
```

Este código le indica que está usando .NET 6.

#### Comprendiendo el código

***Program.cs*** contiene el código de la API. Echemos un vistazo más detallado a un ejemplo de programa:

```C#
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.MapGet("/", () => "Hello World!");
app.Run();
```

Si ha usado versiones anteriores de .NET, observará que no hay instrucciones `using`. Con .NET 6, el compilador determina las instrucciones `using` automáticamente. No es algo de lo que deba preocuparse.

##### Nota

> A medida que agregue más características, como Entity Framework, por ejemplo, tendrá que agregar instrucciones using. Pero para una API sencilla como la del ejemplo anterior, aún no las necesita.

En las dos primeras líneas de código, se crea un generador. A partir de `builder`, se construye una instancia de aplicación app:

```c#
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
```

El generador tiene una propiedad `Services`. Mediante el uso de la propiedad `Services`, puede agregar características como CORS, Entity Framework o Swagger, por ejemplo. Veamos un ejemplo:

```c#
builder.Services.AddCors(options => {});
```

En la propiedad `Services`, le indica a la API que esta es una funcionalidad para usar. Por el contrario, la instancia app se usa con la finalidad de realmente utilizarla. Por lo tanto, puede usar la instancia `app` para configurar el enrutamiento:

```c#
app.MapGet("/", () => "Hello World!");
```

También puede usar la instancia `app` para agregar middleware. A continuación se incluye un ejemplo de cómo usaría una funcionalidad como CORS:

```c#
app.UseCors("some unique string");
```

##### Nota 2

> El middleware suele ser código que intercepta la solicitud y lleva a cabo algunas comprobaciones, como verificar la autenticación, o asegurarse de que el cliente puede realizar esta operación según CORS. Una vez que el middleware ha terminado de ejecutarse, se lleva a cabo la solicitud real. Los datos se leen o escriben en el almacén y se envía una respuesta al cliente que realiza la llamada.

Por último, app.Run() inicia la API y la hace escuchar las solicitudes del cliente.

Para ejecutar el código, inicie el proyecto, como cualquier proyecto de .NET con dotnet run. De forma predeterminada, esto significa que tiene un proyecto que se ejecuta en [http://localhost:{PORT}](https://localhost:{PORT}), donde PORT es un valor entre 5000 y 5300.

### Incorporación de documentación con Swagger

La documentación es algo que quiere para la API. Lo quiere para usted, sus compañeros y cualquier desarrollador de terceros que quiera usar la API. Es clave mantener la documentación sincronizada con la API a medida que cambia. Un buen enfoque es describir la API de forma estandarizada y asegurarse de que se documenta por sí misma. Mediante la autodocumentación, queremos decir que, si el código cambia, la documentación cambia con él.

Swagger implementa la especificación OpenAPI. En este formato se describen las rutas, pero también los datos que aceptan y generan. La interfaz de usuario de Swagger es una colección de herramientas que representa la especificación de OpenAPI como un sitio web y le permite interactuar con la API mediante dicho sitio.

Para usar Swagger y la interfaz de usuario de Swagger en la API, debe hacer dos cosas:

* Instalar un paquete. Para instalar Swagger, especifique que se instale un paquete llamado Swashbuckle:

```bash
dotnet add package Swashbuckle.AspNetCore --version 6.1.4
```

* Configurarlo. Una vez instalado el paquete, configúrelo mediante el código. Agregue algunas entradas diferentes:

  * Agregue un espacio de nombres. Necesitará este espacio de nombres cuando llame más adelante a `SwaggerDoc()` y proporcione la información de encabezado para la API.

  ```c#
  using Microsoft.OpenApi.Models;
  ```

  * Agregue `AddSwaggerGen()`. Este método configura la información del encabezado en la API; p. ej., a qué se llama y el número de versión.

  ```c#
  builder.Services.AddEndpointsApiExplorer();
  builder.Services.AddSwaggerGen(c =>
   {
       c.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo API", Description = "Keep track of your tasks", Version = "v1" });
   }); 
  ```

¡Eso es todo lo que implica la creación de una API mínima! El inicio del proyecto y la navegación a [http://localhost:{PORT}/swagger](http://localhost:5000/swagger) muestran algo parecido a esto:

![alt](https://learn.microsoft.com/es-es/training/aspnetcore/build-web-api-minimal-api/media/swagger-todo-api.png)

## Para mas informacion seguir el enlace [Creación de aplicaciones web y servicios con ASP.NET Core, API mínima y .NET](https://learn.microsoft.com/es-es/training/paths/aspnet-core-minimal-api/)
