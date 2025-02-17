using ControlStoreAPI.Data;
using ControlStoreAPI.Data.Interface;
using ControlStoreAPI.Data.Model;
using ControlStoreAPI.Extensions;
using ControlStoreAPI.Middleware;
using ControlStoreAPI.Models;
using ControlStoreAPI.Service;
using ControlStoreAPI.Service.Interface;
using ControlStoreAPI.Services;
using ControlStoreAPI.Services.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionStringMySQL = builder.Configuration.GetConnectionString("ConnectionMysql");
builder.Services.AddDbContext<APIDbContext>(x => x.UseMySql(
    connectionStringMySQL,
    Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.5.62"),
    mysqlOptions => mysqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null) // Customize retries as needed
    )
);

// Add services to the container.

#region Permitir CORS Vue
builder.Services.AddControllersWithViews();

// Configuração do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
        builder.WithOrigins("http://localhost:3000",
                        "http://www.memconsultoria.kinghost.net",
                        "http://memconsultoria.kinghost.net",
                        "www.memconsultoria.kinghost.net")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .WithExposedHeaders("Content-Disposition"));
    //options.AddPolicy("AllowAll", policy =>
    //{
    //    policy.AllowAnyOrigin()
    //          .AllowAnyMethod()
    //          .AllowAnyHeader()
    //          .WithExposedHeaders("Content-Disposition");
    //});
});
#endregion

builder.Services.AddControllers();

//Registro de serviços
builder.Services.AddScoped<IRepository<Cliente>, Repository<Cliente>>();
builder.Services.AddScoped<IRepository<Composicao>, Repository<Composicao>>();
builder.Services.AddScoped<IRepository<ComposicaoToProduto>, Repository<ComposicaoToProduto>>();
builder.Services.AddScoped<IRepository<GrupoProduto>, Repository<GrupoProduto>>();
builder.Services.AddScoped<IRepository<GrupoUsuario>, Repository<GrupoUsuario>>();
builder.Services.AddScoped<IRepository<ListaPrecoCabecalho>, Repository<ListaPrecoCabecalho>>();
builder.Services.AddScoped<IRepository<ListaPrecoDetalhe>, Repository<ListaPrecoDetalhe>>();
builder.Services.AddScoped<IRepository<Modulo>, Repository<Modulo>>();
builder.Services.AddScoped<IRepository<PedidoCabecalho>, Repository<PedidoCabecalho>>();
builder.Services.AddScoped<IRepository<PedidoDetalhe>, Repository<PedidoDetalhe>>();
builder.Services.AddScoped<IRepository<Permissao>, Repository<Permissao>>();
builder.Services.AddScoped<IRepository<Produto>, Repository<Produto>>();
builder.Services.AddScoped<IRepository<StatusCliente>, Repository<StatusCliente>>();
builder.Services.AddScoped<IRepository<TipoPermissao>, Repository<TipoPermissao>>();
builder.Services.AddScoped<IRepository<Usuario>, Repository<Usuario>>();


builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IComposicaoService, ComposicaoService>();
builder.Services.AddScoped<IComposicaoToProdutoService, ComposicaoToProdutoService>();
builder.Services.AddScoped<IGrupoProdutoService, GrupoProdutoService>();
builder.Services.AddScoped<IListaPrecoCabecalhoService, ListaPrecoCabecalhoService>();
builder.Services.AddScoped<IListaPrecoDetalheService, ListaPrecoDetalheService>();
builder.Services.AddScoped<IPedidoCabecalhoService, PedidoCabecalhoService>();
builder.Services.AddScoped<IPedidoDetalheService, PedidoDetalheService>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IStatusClientService, StatusClientService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do serviço de logger
builder.Services.AddScoped<ILoggerService, LoggerService>();


// Registrar AutoMapper
builder.Services.AddAutoMapper(typeof(Program)); // Scaneia automaticamente os perfis de mapeamento


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region Permitir CORS
app.UseDeveloperExceptionPage();

var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(uploadsPath),
    RequestPath = "/uploads",
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Methods", "GET, HEAD, OPTIONS");
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type");
    }
});


app.UseRouting();

// Aplica o CORS usando a política configurada
app.UseCors("AllowSpecificOrigin");
//app.UseCors("AllowAll");



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
#endregion

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//Permitindo servir arquivos estáticos de um diretório específico
app.UseStaticFiles(); // Para wwwroot

// Adiciona o middleware de tratamento de erros padrão do ASP.NET Core
app.UseExceptionHandler("/error");

// Adiciona o middleware de logging de erro personalizado
app.UseCustomExceptionHandler();

app.UseMiddleware<ErrorLoggingMiddleware>();


app.Run();
