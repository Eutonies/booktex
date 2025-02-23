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

app.UseServices();
app.Run();
