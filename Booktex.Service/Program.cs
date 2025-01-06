using Booktex.Application.GitHub;
using Booktex.Domain.GitHub;
using Booktex.Infrastructure.GitHub;
using Booktex.Service;
using GitHub;




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
var githubService = scope.ServiceProvider.GetRequiredService<IGitHubService>();
var githubRef = GitHubRef.From("sune-roenne", "haw-story", "main");
await githubService.DownloadFiles(githubRef);


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
