using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microservicios.Atracciones.Catalog.DataAccess;
using Microservicios.Atracciones.Catalog.DataManagement;
using Microservicios.Atracciones.Catalog.Business;
using Asp.Versioning;
using Microservicios.Atracciones.Catalog.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// IHttpContextAccessor: requerido por el factory de conexiones por rol
builder.Services.AddHttpContextAccessor();

// ======================================================
// 1. CONFIGURACIÃ“N DE CAPAS (CLEAN ARCHITECTURE)
// ======================================================
builder.Services.AddDataAccessServices(builder.Configuration);
builder.Services.AddDataManagementServices();
builder.Services.AddBusinessServices();

// ======================================================
// 2. CONFIGURACIÃ“N API & CORS
// ======================================================
builder.Services.AddControllers(options =>
{
    options.Filters.Add<Microservicios.Atracciones.Catalog.API.Filters.ApiResponseWrapperFilter>();
    options.Conventions.Add(new RoutePrefixConvention("catalog"));
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});

builder.Services.AddEndpointsApiExplorer();

// ======================================================
// 3. SWAGGER CON SEGURIDAD JWT
// ======================================================
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Catalog Microservice API", Version = "v1" });

    c.MapType<DateOnly>(() => new Microsoft.OpenApi.Models.OpenApiSchema { Type = "string", Format = "date" });
    c.MapType<TimeOnly>(() => new Microsoft.OpenApi.Models.OpenApiSchema { Type = "string", Format = "time" });
    c.MapType<TimeSpan>(() => new Microsoft.OpenApi.Models.OpenApiSchema { Type = "string", Format = "duration" });

    c.CustomSchemaIds(type => type.FullName);

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Ingresa tu token JWT en el formato: Bearer {tu_token_aqui}",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new Microsoft.OpenApi.Models.OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme
        }
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// ======================================================
// 4. JWT AUTHENTICATION (Solo ValidaciÃ³n)
// ======================================================
var jwtKey = builder.Configuration["Jwt:Key"] ?? "MicroserviciosAtraccionesCatalog_Super_Secret_Key_2026_Minimum_Length_Requirement_Long_String";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "MicroserviciosAtraccionesCatalog";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "MicroserviciosAtraccionesCatalogUsers";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

var app = builder.Build();

// ======================================================
// 5. CONFIGURACIÃ“N DEL PIPELINE HTTP
// ======================================================
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog Microservice API v1");
    c.RoutePrefix = string.Empty;
});

app.UseMiddleware<ExceptionMiddleware>();
app.UseStaticFiles();
// UseHttpsRedirection omitido: servicio interno detrás de gateway nginx que maneja TLS
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ======================================================
// ACTUALIZADOR DE BASE DE DATOS AUTOMÁTICO EN EL INICIO
// ======================================================
try
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<Microservicios.Atracciones.Catalog.DataAccess.Context.AtraccionDbContext>();
        Console.WriteLine("Ejecutando script de actualización de base de datos...");
        
        Microsoft.EntityFrameworkCore.RelationalDatabaseFacadeExtensions.ExecuteSqlRaw(context.Database, @"
            ALTER TABLE price_tier ADD COLUMN IF NOT EXISTS is_active BOOLEAN DEFAULT TRUE;
            ALTER TABLE price_tier ADD COLUMN IF NOT EXISTS created_at TIMESTAMPTZ DEFAULT NOW();
            ALTER TABLE price_tier ADD COLUMN IF NOT EXISTS updated_at TIMESTAMPTZ DEFAULT NOW();
            
            ALTER TABLE product_option ADD COLUMN IF NOT EXISTS description TEXT;
            ALTER TABLE product_option ADD COLUMN IF NOT EXISTS duration_minutes INTEGER;
            ALTER TABLE product_option ADD COLUMN IF NOT EXISTS duration_description VARCHAR(100);
            ALTER TABLE product_option ADD COLUMN IF NOT EXISTS cancel_policy_hours INTEGER DEFAULT 24;
            ALTER TABLE product_option ADD COLUMN IF NOT EXISTS cancel_policy_text TEXT;
            ALTER TABLE product_option ADD COLUMN IF NOT EXISTS max_group_size SMALLINT;
            ALTER TABLE product_option ADD COLUMN IF NOT EXISTS min_participants SMALLINT DEFAULT 1;
            ALTER TABLE product_option ADD COLUMN IF NOT EXISTS is_active BOOLEAN DEFAULT TRUE;
            ALTER TABLE product_option ADD COLUMN IF NOT EXISTS is_private BOOLEAN DEFAULT FALSE;
            ALTER TABLE product_option ADD COLUMN IF NOT EXISTS sort_order SMALLINT DEFAULT 0;
            ALTER TABLE product_option ADD COLUMN IF NOT EXISTS created_at TIMESTAMPTZ DEFAULT NOW();
            ALTER TABLE product_option ADD COLUMN IF NOT EXISTS updated_at TIMESTAMPTZ DEFAULT NOW();
        ");
        
        Console.WriteLine("¡Base de datos actualizada con éxito de forma automática!");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error en actualización automática de base de datos: {ex.Message}");
}

// Diagnostic endpoints - respond to different path formats to determine gateway forwarding behavior
app.MapGet("/ping", (HttpContext ctx) => Results.Ok(new { match = "absolute /ping", path = ctx.Request.Path.Value })).AllowAnonymous();
app.MapGet("/catalog/ping", (HttpContext ctx) => Results.Ok(new { match = "/catalog/ping", path = ctx.Request.Path.Value })).AllowAnonymous();
app.MapGet("/api/v1/catalog/ping", (HttpContext ctx) => Results.Ok(new { match = "/api/v1/catalog/ping", path = ctx.Request.Path.Value })).AllowAnonymous();
app.MapGet("/api/v1/coronel_paula/catalog/ping", (HttpContext ctx) => Results.Ok(new { match = "/api/v1/coronel_paula/catalog/ping", path = ctx.Request.Path.Value })).AllowAnonymous();

app.Run();


public class RoutePrefixConvention : Microsoft.AspNetCore.Mvc.ApplicationModels.IApplicationModelConvention
{
    private readonly string _prefix;
    public RoutePrefixConvention(string prefix) => _prefix = prefix;

    public void Apply(Microsoft.AspNetCore.Mvc.ApplicationModels.ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        {
            foreach (var selector in controller.Selectors)
            {
                if (selector.AttributeRouteModel != null)
                {
                    var currentTemplate = selector.AttributeRouteModel.Template;
                    if (currentTemplate != null && currentTemplate.StartsWith("api/v1/"))
                    {
                        selector.AttributeRouteModel.Template = _prefix + "/" + currentTemplate["api/v1/".Length..];
                    }
                }
            }
        }
    }
}

