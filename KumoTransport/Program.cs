//using KumoTransport.Models;
//using Microsoft.AspNetCore.StaticFiles;
//using Microsoft.EntityFrameworkCore;
//using Piranha;
//using Piranha.AspNetCore.Identity.SQLite;
//using Piranha.AspNetCore.Models;
//using Piranha.AttributeBuilder;
//using Piranha.Data.EF.SQLite;
//using Piranha.Manager.Editor;

//var builder = WebApplication.CreateBuilder(args);

//builder.AddPiranha(options =>
//{
//  /**
//   * This will enable automatic reload of .cshtml
//   * without restarting the application. However since
//   * this adds a slight overhead it should not be
//   * enabled in production.
//   */
//  options.AddRazorRuntimeCompilation = true;

//  options.UseCms();
//  options.UseManager();

//  options.UseFileStorage(naming: Piranha.Local.FileStorageNaming.UniqueFolderNames);
//  options.UseImageSharp();
//  options.UseTinyMCE();
//  options.UseMemoryCache();


//  var connectionString = builder.Configuration.GetConnectionString("piranha");
//  options.UseEF<SQLiteDb>(db => db.UseSqlite(connectionString));
//  options.UseIdentityWithSeed<IdentitySQLiteDb>(db => db.UseSqlite(connectionString));

//  /**
//   * Here you can configure the different permissions
//   * that you want to use for securing content in the
//   * application.
//  options.UseSecurity(o =>
//  {
//      o.UsePermission("WebUser", "Web User");
//  });
//   */

//  /**
//   * Here you can specify the login url for the front end
//   * application. This does not affect the login url of
//   * the manager interface.
//  options.LoginUrl = "login";
//   */
//});

//var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//  app.UseDeveloperExceptionPage();
//}
//app.UseRouting();


//StaticFileOptions options = new StaticFileOptions { ContentTypeProvider = new FileExtensionContentTypeProvider() };
//options.ServeUnknownFileTypes = true;
//app.UseStaticFiles(options);


//app.UsePiranha(options =>
//{
//  // Initialize Piranha
//  App.Init(options.Api);

//  // Build content types
//  new ContentTypeBuilder(options.Api)
//      .AddAssembly(typeof(Program).Assembly)
//      .AddType(typeof(TestPage))
//      .Build()
//      .DeleteOrphans();


//  // Configure Tiny MCE
//  EditorConfig.FromFile("editorconfig.json");

//  options.UseManager();
//  options.UseTinyMCE();
//  options.UseIdentity();
//});



//app.Run();

using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Piranha;
using Piranha.AspNetCore.Identity.SQLite;
using Piranha.AttributeBuilder;
using Piranha.Data.EF.SQLite;
using Piranha.Manager.Editor;

var builder = WebApplication.CreateBuilder(args);

// 🟢 Nastavení Piranha
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

using (var scope = app.Services.CreateScope())
{
  var api = scope.ServiceProvider.GetRequiredService<Piranha.IApi>();

  new ContentTypeBuilder(api)
      .AddAssembly(typeof(Program).Assembly)
      .Build()
      .DeleteOrphans();

}

// 🟢 Piranha middleware
app.UsePiranha(options =>
{
  App.Init(options.Api);

  // Editor config (volitelně)
  EditorConfig.FromFile("editorconfig.json");

  options.UseManager();
  options.UseTinyMCE();
  options.UseIdentity();
});

if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
}

app.UseRouting();

StaticFileOptions staticOptions = new StaticFileOptions
{
  ContentTypeProvider = new FileExtensionContentTypeProvider(),
  ServeUnknownFileTypes = true
};
app.UseStaticFiles(staticOptions);

app.Run();
