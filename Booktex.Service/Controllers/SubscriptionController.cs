using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Booktex.Domain.Api.Subscription;
using Booktex.Application.Book;
using Booktex.Application.Subscription;
using Booktex.Domain.Util;

namespace Booktex.Service.Controllers;

public class SubscriptionController : BooktexController
{
    public const string Subscriptions = "subscriptions";

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

    [HttpPost(Subscriptions)]
    public async Task<Results<Ok<BooktexSubscriptionDto>, BadRequest<string>>> CreateSubscription(
        [FromBody] BooktexSubscriptionDto sub
        )
    {
        if (sub.SubscriptionId > 0)
            return TypedResults.BadRequest("Thou cannotst bring ya own Subscription ID when creating a new subscription");

        _subscriptionRepo.Create()
        var subs = await _subscriptionRepo.LoadSubscriptions();
        var returnee = subs
            .Select(_ => _.ToDto())
            .ToReadonlyCollection();
        return TypedResults.Ok(returnee);
    }


}
