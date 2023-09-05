using FluentValidation;
using MCS.HomeSite.Common;
using MCS.HomeSite.Data;
using MCS.HomeSite.Data.CustomValidators;
using MCS.HomeSite.Data.Models.Users;
using MCS.HomeSite.Handlers;
using MCS.HomeSite.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<IMigrationHandler, MigrationHandler>();
builder.Services.AddDbContext<McsHomeSiteContext>();
builder.Services.AddDbContext<McsHomeSiteDestContext>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IUrlGenerator, UrlGenerator>();

//Validators
builder.Services.AddScoped<IValidator<UserDto>, UserValidator>();

builder.Services.AddMvc();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<McsHomeSiteContext>();
var dbContextDest = scope.ServiceProvider.GetRequiredService<McsHomeSiteDestContext>();
dbContext.Database.EnsureCreated();
dbContextDest.Database.EnsureCreated();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapGroup(EndpointPrefixes.Users).MapUserServiceGroup();
app.MapGroup(EndpointPrefixes.Migrations).MapMigrationServiceGroup();


app.Run();
