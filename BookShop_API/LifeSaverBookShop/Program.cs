using LifeSaverBookShop.Hubs;
using LifeSaverBookShopAPI.Controllers;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));

builder.Services.AddSingleton<IHubBookDispatcher, HubBookDispatcher>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BooksService", Version = "v1" });
});

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.



app.UseCors("MyPolicy");


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BooksService v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.MapHub<BooksHub>("/bookHub");

app.Run();
