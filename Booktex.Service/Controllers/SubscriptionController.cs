using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Booktex.Domain.Api.Subscription;
using Booktex.Application.Book;
using Booktex.Application.Subscription;
using Booktex.Domain.Util;
using System.Text.RegularExpressions;
using Booktex.Domain.Subscription;

namespace Booktex.Service.Controllers;

public class SubscriptionController : BooktexController
{
    public const string Subscriptions = "subscriptions";
    public const string Subscription = "subscription";

    private static readonly Regex GithubUrlRegex = new Regex("https://github.com/([^/]+)/(.+)", RegexOptions.IgnoreCase);

    private readonly ISubscriptionRepo _subscriptionRepo;

    public SubscriptionController(ISubscriptionRepo subscriptionRepo)
    {
        _subscriptionRepo = subscriptionRepo;
    }

    [HttpGet(Subscriptions)]
    public async Task<Ok<IReadOnlyCollection<BooktexSubscriptionDto>>> LoadSubscriptions()
    {
        var subs = await _subscriptionRepo.LoadSubscriptions();
        var returnee = subs
            .Select(_ => _.ToDto())
            .ToReadonlyCollection();
        return TypedResults.Ok(returnee);
    }

    [HttpPost(Subscription)]
    public async Task<Results<Ok<BooktexSubscriptionDto>, BadRequest<string>, BadRequest<BooktexSubscriptionDto>>> CreateGithubSubscription(
        [FromForm] BooktexSubscriptionDto sub
        )
    {
        if (sub.SubscriptionId > 0)
            return TypedResults.BadRequest("Thou cannotst bring ya own Subscription ID when creating a new subscription");
        if (string.IsNullOrWhiteSpace(sub.GitHubOwner) || string.IsNullOrWhiteSpace(sub.GitHubRepo))
            return TypedResults.BadRequest("As a bare minimum, ya have ta specify Github Owner & Repo");
        var existing = await _subscriptionRepo.Exists(sub.GitHubOwner, sub.GitHubRepo, sub.FileRegex);
        if (existing != null)
            return TypedResults.BadRequest(existing.ToDto());
        var domainSub = sub.ToDomain();
        domainSub = await _subscriptionRepo.Create(domainSub);
        var returnee = domainSub.ToDto();
        return TypedResults.Ok(returnee);
    }

    [HttpPost($"{Subscription}-from-url")]
    public async Task<Results<Ok<BooktexSubscriptionDto>, BadRequest<string>, BadRequest<BooktexSubscriptionDto>>> CreateGithubSubscriptionFromUrl(
        [FromForm] string url,
        [FromForm] string? fileRegex
        )
    {
        var matches = GithubUrlRegex.Matches(url);
        if (matches.Count == 0 || matches.First().Groups.Count < 2)
            return TypedResults.BadRequest($"Could not entire match URL: {url} to regex: {GithubUrlRegex.ToString()}... Matched: {matches.FirstOrDefault()?.Value}");
        var match = matches.First();
        var (owner, repo) = (match.Groups[1].Value, match.Groups[2].Value);
        if (repo.ToLower().EndsWith(".git"))
            repo = repo.Substring(0, repo.Length - 4);
        var sub = new BooktexSubscriptionDto(0L, BooktexSubscriptionTypeDto.GitHub, owner, repo, fileRegex);
        var returnee = await CreateGithubSubscription(sub);
        return returnee;
    }
}
