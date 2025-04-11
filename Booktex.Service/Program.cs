using Booktex.Application.GitHub;
using Booktex.Application.Subscription;
using Booktex.Application.Zip;
using Booktex.Domain.Book.Specials;
using Booktex.Domain.GitHub;
using Booktex.Domain.Parsing;
using Booktex.Domain.Subscription;
using Booktex.Service;


var inputFile = "C:\\git\\jen-and-will\\psych\\abel1.psych";
var input = await File.ReadAllTextAsync(inputFile);
var parseResult = PsychLogParser.Parse(input);

var builder = WebApplication.CreateBuilder(args);
builder.AddConfiguration();
builder.AddServices();

var app = builder.Build();

app.UseServices();
app.Run();
