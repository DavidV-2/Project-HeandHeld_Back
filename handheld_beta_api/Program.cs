using handheld_beta_api.Model;
using handheld_beta_api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

builder.Services.AddControllers();
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
    options.HttpsPort = 5000;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
