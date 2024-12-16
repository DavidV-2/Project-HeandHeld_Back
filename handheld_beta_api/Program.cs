using handheld_beta_api.Conexion;
using handheld_beta_api.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuraci�n de Kestrel
builder.WebHost.ConfigureKestrel(options =>
{
    options.Configure(builder.Configuration.GetSection("Kestrel"));
});

// Configuraci�n de CORS
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

// Configuraci�n de Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuraci�n de redirecci�n HTTPS
builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 1443; // Asumiendo que el puerto HTTPS es 443
});

var app = builder.Build();

// Configuraci�n de la tuber�a de solicitud HTTP
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
