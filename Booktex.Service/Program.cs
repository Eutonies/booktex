using Booktex.Application.GitHub;
using Booktex.Application.Subscription;
using Booktex.Application.Zip;
using Booktex.Domain.GitHub;
using Booktex.Domain.Subscription;
using Booktex.Service;




var builder = WebApplication.CreateBuilder(args);
builder.AddConfiguration();
builder.AddServices();

var app = builder.Build();
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using var scope = scopeFactory.CreateScope();
/*var githubService = scope.ServiceProvider.GetRequiredService<IGitHubService>();
var githubRef = GitHubRef.From("sune-roenne", "haw-story", "main");
var fileBytes = await githubService.DownloadFiles(githubRef);
var outputFileName = $"C:/git/booktex/sample/GitHubZips/{githubRef.Repo.Owner}.{githubRef.Repo.Name}.zip";
await File.WriteAllBytesAsync(outputFileName, fileBytes);*/

var zipFileName = "C:\\git\\booktex\\sample\\GitHubZips\\sune-roenne.haw-story.zip";
var zipFileParser = scope.ServiceProvider.GetRequiredService<IZipFileParser>();
var parsed = (await zipFileParser.Parse(zipFileName));
var subscription = new BooktexGitHubSubscription(0L, "sune-roenne", "haw-story", @"(/|\\)Book(/|\\).*\.((story)|(ata)|(json))");
var execution = subscription.ExecutionFrom(parsed!);
var repo = scope.ServiceProvider.GetRequiredService<ISubscriptionRepo>();
subscription = await repo.Create(subscription);
execution = await repo.Create(subscription.SubscriptionId, execution);


app.UseServices();
app.Run();
