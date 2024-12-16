using handheld_beta_api.Conexion;
using handheld_beta_api.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuración de Kestrel
builder.WebHost.ConfigureKestrel(options =>
{
    options.Configure(builder.Configuration.GetSection("Kestrel"));
});

// Configuración de CORS
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

// Agregar controladores
builder.Services.AddControllers();

// Registrar servicios
builder.Services.AddScoped<ValidarTiketService>();
builder.Services.AddScoped<PermisosTrasladoService>();
builder.Services.AddScoped<ObtenerPedidoService>();
builder.Services.AddScoped<UsuarioService>();

// Configurar las conexiones a las bases de datos
var conexionBD = new ConexionBD(builder.Configuration);
conexionBD.ConfigurarConexionBD(builder.Services);

// Configuración de Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de redirección HTTPS
builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 1443; // Asumiendo que el puerto HTTPS es 443
});

var app = builder.Build();

// Configuración de la tubería de solicitud HTTP
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
