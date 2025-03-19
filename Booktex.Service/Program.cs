using Booktex.Application.GitHub;
using Booktex.Application.Subscription;
using Booktex.Application.Zip;
using Booktex.Domain.GitHub;
using Booktex.Domain.Parsing;
using Booktex.Domain.Subscription;
using Booktex.Service;


var inputFile = "C:/git/jen-and-will/story/UNDECIDED-Will and Laura - Emotionally Responsible.story";
var input = await File.ReadAllTextAsync(inputFile);
var parseResult = WritingParser.ParseFileContents(input);

var builder = WebApplication.CreateBuilder(args);
builder.AddConfiguration();
builder.AddServices();

var app = builder.Build();

app.UseServices();
app.Run();
