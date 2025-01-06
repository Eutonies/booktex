using Booktex.Application.GitHub;
using Booktex.Application.Zip;
using Booktex.Domain.GitHub;
using Booktex.Service;




var builder = WebApplication.CreateBuilder(args);
builder.AddConfiguration();
builder.AddServices();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using var scope = scopeFactory.CreateScope();
/*var githubService = scope.ServiceProvider.GetRequiredService<IGitHubService>();
var githubRef = GitHubRef.From("sune-roenne", "haw-story", "main");
var fileBytes = await githubService.DownloadFiles(githubRef);
var outputFileName = $"C:/git/booktex/sample/GitHubZips/{githubRef.Repo.Owner}.{githubRef.Repo.Name}.zip";
await File.WriteAllBytesAsync(outputFileName, fileBytes);*/

var zipFileName = "C:\\git\\booktex\\sample\\GitHubZips\\sune-roenne.haw-story.zip";
var zipFileParser = scope.ServiceProvider.GetRequiredService<IZipFileParser>();
var parsed = await zipFileParser.Parse(zipFileName);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
