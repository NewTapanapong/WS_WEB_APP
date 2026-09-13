using Microsoft.EntityFrameworkCore;

using TodoApi.Dtos;
using TodoApi.Models;
using TodoApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")
));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var todoGroup = app.MapGroup("/api/todos").WithTags("Todos");

#region in-Memory Endpoints

// var todos = new List<TodoGetDto>
//  {
//     new(1,"Learn C#", true),
//      new(2,"Learn ASP.NET Core", false),
//      new(3,"Build a web API", false)
//  };

//  todoGroup.MapGet("/api/todos",() =>
//  Results.Ok(todos));

//  todoGroup.MapGet("/api/todos", () => Results.Ok(todos));

//  todoGroup.MapGet("/api/todos/{id}", (int id) =>
//  {
//      var todo = todos.FirstOrDefault(t => t.id == id);

//      return todo is not null ? Results.Ok(todo) : Results.NotFound();
//  });

//  todoGroup.MapPost("/api/todos",(TodopostDto dto) =>
//  {
//      var nextId = todos.Count == 0 ? 1 : todos.Max(t => t.id) + 1;

//      var todo = new TodoGetDto(nextId, dto.title, false);
//      todos.Add(todo);

//      return Results.Created($"/api/todos/{todo.id}", todo);
//  });

//  todoGroup.MapPut("/api/todos/{id}", (int id, TodoputDTO dto) =>
//  {
//      try
//      {
//          var index =todos.FindIndex(t => t.id ==id);
//          if (index == -1) return Results.NotFound();
//          todos[index] = todos[index] with 
//          { 
//              title = dto.title,
//              isCompleted = dto.isCompleted
//          };

//          return Results.Ok(todos[index]);
//      }
//      catch (Exception ex)
//      {
//          return Results.Problem(ex.Message);
//      }
//  });

//  todoGroup.MapDelete("/api/todos/{id}", (int id) =>
//  {
//      try
//      {
//          var todo = todos.FirstOrDefault(t => t.id == id);
//          if (todo is null) return Results.NotFound();

//          todos.Remove(todo);
//          return Results.NoContent();
//      }
//      catch (Exception ex)
//      {
//          return Results.Problem(ex.Message);
//      }
//  });

#endregion

#region Database Endpoints

todoGroup.MapGet("/", async (AppDbContext db) =>
{
    var todos = await db.Todos.ToListAsync();

    return todos.Count ==0 ? Results.NotFound() : Results.Ok(todos);
});

todoGroup.MapGet("/", async (AppDbContext db, TodopostDto dto) =>
{
    var lastTodo = await db.Todos.OrderByDescending(t => t.id).FirstOrDefaultAsync();
    var nextid = lastTodo is null ? 1 : lastTodo.id + 1;

    var todo = new TodoItem
    {
        id = nextid,
        title = dto.title,
        isCompleted = false,
        CreatedAt = DateTime.UtcNow
    };

   db.Todos.Add(todo);
    await db.SaveChangesAsync();
    
    var todoGetDto = new TodoGetDto(todo.id, todo.title, todo.isCompleted);

    return Results.Created($"/{todo.id}", todoGetDto);
});

#endregion

app.Run();