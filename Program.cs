using Microsoft.OpenApi.Models;
using PizzaStore.DB;

// Se declara el constructor de la apliacion
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer(); // se agrega el explorador de Endpoints de Swagger
builder.Services.AddSwaggerGen(c =>
   {
       c.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo API", Description = "Keep track of your tasks", Version = "v1" });
   });

var app = builder.Build();

/*
 Estas dos líneas de código le indican al proyecto de API que use Swagger y
  también dónde encontrar el archivo de especificación swagger.json
*/
app.UseSwagger();
app.UseSwaggerUI(c =>
   {
     c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
   });

app.MapGet("/", () => "Hello World!");

app.MapGet("/pizzas/{id}", (int id) => PizzaDB.GetPizza(id));
app.MapGet("/pizzas", () => PizzaDB.GetPizzas());
app.MapPost("/pizzas", (Pizza pizza) => PizzaDB.CreatePizza(pizza));
app.MapPut("/pizzas", (Pizza pizza) => PizzaDB.UpdatePizza(pizza));
app.MapDelete("/pizzas/{id}", (int id) => PizzaDB.RemovePizza(id));

app.Run();
