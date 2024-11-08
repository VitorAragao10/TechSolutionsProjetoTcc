using System.Data;
using Npgsql;
using TechSolutions.Web.Data;
using TechSolutions.Web.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Registrar o IDbConnection para PostgreSQL
builder.Services.AddScoped<IDbConnection>(sp =>
    new NpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar o TransacaoRepository
builder.Services.AddScoped<TransacaoRepository>();
builder.Services.AddScoped<ClienteRepository>();
builder.Services.AddScoped<MachineLearningService>();


// Adicionar suporte para controllers e views
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

