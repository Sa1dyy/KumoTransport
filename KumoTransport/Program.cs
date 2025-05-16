using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Piranha;
using Piranha.AspNetCore.Identity.SQLite;
using Piranha.AttributeBuilder;
using Piranha.Data.EF.SQLite;
using Piranha.Manager.Editor;

var builder = WebApplication.CreateBuilder(args);

// Dynamické nastavení base path z prostředí nebo konfigurace
var basePath = Environment.GetEnvironmentVariable("APP_BASE_PATH") ?? builder.Configuration["AppBasePath"];

if (!string.IsNullOrWhiteSpace(basePath))
{
  builder.WebHost.UseSetting(WebHostDefaults.ApplicationKey, builder.Environment.ApplicationName);
}

builder.AddPiranha(options =>
{
  options.AddRazorRuntimeCompilation = true;
  options.UseCms();
  options.UseManager();
  options.UseFileStorage(naming: Piranha.Local.FileStorageNaming.UniqueFolderNames);
  options.UseImageSharp();
  options.UseTinyMCE();
  options.UseMemoryCache();

  var connectionString = builder.Configuration.GetConnectionString("piranha");
  options.UseEF<SQLiteDb>(db => db.UseSqlite(connectionString));
  options.UseIdentityWithSeed<IdentitySQLiteDb>(db => db.UseSqlite(connectionString));
});

var app = builder.Build();

if (!string.IsNullOrWhiteSpace(basePath))
{
  app.UsePathBase(basePath);
}

// Piranha CMS initialization
using (var scope = app.Services.CreateScope())
{
  var api = scope.ServiceProvider.GetRequiredService<Piranha.IApi>();

  new ContentTypeBuilder(api)
      .AddAssembly(typeof(Program).Assembly)
      .Build()
      .DeleteOrphans();
}

if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
}

// ✅ statické soubory před UseRouting
StaticFileOptions staticOptions = new StaticFileOptions
{
  ContentTypeProvider = new FileExtensionContentTypeProvider(),
  ServeUnknownFileTypes = true
};
app.UseStaticFiles(staticOptions);

app.UseRouting();

// ✅ Piranha middleware po routing
app.UsePiranha(options =>
{
  App.Init(options.Api);

  EditorConfig.FromFile("editorconfig.json");

  options.UseManager();
  options.UseTinyMCE();
  options.UseIdentity();
});

app.Run();
