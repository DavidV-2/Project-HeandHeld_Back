using handheld_beta_api.Model;
using handheld_beta_api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.WebHost.ConfigureKestrel(options =>
{
    options.Configure(builder.Configuration.GetSection("Kestrel"));
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

builder.Services.AddControllers();

builder.Services.AddScoped<PermisosTrasladoService>();

builder.Services.AddDbContext<PermisosTrasladoContext>(
    opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DBConnectionJJVPRGPRODUCCION"))
);

builder.Services.AddScoped<ObtenerPedidoService>();

builder.Services.AddDbContext<ObtenerPedidoContext>(
    opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DBConnectionJJVPRGPRODUCCION"))
);

builder.Services.AddScoped<UsuarioService>();

builder.Services.AddDbContext<UsuarioContext>(
    opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DBConnectionCorsan"))
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 1433;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
