using Chat.API;
using Chat.API.Controllers;
using Chat.API.SignalR;
using Chat.Infrastructure;
using Chat.Infrastructure.DataInitializer;
using Chat.Infrastructure.EF;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

var context = new ApplicationContext();
await DbInitializer.Initialize(context, builder.Configuration);

builder.Services.AddJWTTokenServices(builder.Configuration);

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
builder.Services.AddCors(options =>
{
    options.AddPolicy("allowMyOrigin",
    builder =>
    {
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .WithExposedHeaders("X-Pagination");
    });
});
builder.Services.AddInfrastructure();
builder.Services.AddServices();

builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, EmailBasedUserIdProvider>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseCors("allowMyOrigin");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ChatHub>("/chat");
});

app.Run();
