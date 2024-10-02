using CleanArchitecture.Application.Common.Abstracts.Account;
using CleanArchitecture.Application.Common.Abstracts.ClinetInfo;
using CleanArchitecture.Persistence.EF;
using CleanArchitecture.WebAPI.Common;
using CleanArchitecture.WebAPI.Configuration;
using CleanArchitecture.WebAPI.Services;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

#region Service Collection
// Add services to the container.
builder.Services.AddSingleton<ICurrentUser, CurrentUser>();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ICurrentRequestInfoService, CurrentRequestInfoService>();
builder.Services.InstallServices(config, typeof(IServiceInstaller).Assembly);
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Presentation.WebAPI", Version = "v1" });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
#endregion

#region Application middleware 
var app = builder.Build();
app.UseExceptionHandler(opt => { });
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
#endregion
