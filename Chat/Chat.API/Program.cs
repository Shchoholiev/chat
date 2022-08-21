using Chat.API;
using Chat.Infrastructure;
using Chat.Infrastructure.Services.SignalR;

var builder = WebApplication.CreateBuilder(args);

//var context = new ApplicationContext();
//await DbInitializer.Initialize(context, builder.Configuration);

builder.Services.AddJWTTokenServices(builder.Configuration);
builder.Services.ConfigureControllers();
builder.Services.ConfigureCORS();
builder.Services.ConfigureSignalR();
builder.Services.AddInfrastructure();
builder.Services.AddServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
