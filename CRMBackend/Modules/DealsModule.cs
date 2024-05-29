using CRMBackend.Contracts;
using CRMBackend.CookieService;
using CRMBackend.Models;
using Microsoft.AspNetCore.Routing;
using Supabase;

namespace CRMBackend.Modules
{
    public static class DealsModule
    {
        public static void DealsEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/deals", async (
                HttpContext httpContext,
                DealRequests.CreateDealRequest request,
                Client client) =>
            {
                var (isAuthenticated, errorMessage) = await CookieHelper.Authenticate(httpContext, client);
                if (!isAuthenticated)
                {
                    return Results.Unauthorized();
                }

                var deal = new Deals
                {
                    deal_id = request.deal_id,
                    deal_name = request.deal_name,
                    deal_stage = request.deal_stage,
                    value = request.value,
                    product = request.product
                };
                var response = await client.From<Deals>().Insert(deal);
                var newDeal = response.Models.First();
                return Results.Ok(newDeal.deal_id);
            });

            app.MapGet("/api/deals", async (HttpContext httpContext, Client client) =>
            {
                var (isAuthenticated, errorMessage) = await CookieHelper.Authenticate(httpContext, client);
                if (!isAuthenticated)
                {
                    return Results.Unauthorized();
                }

                var results = await client.From<Deals>().Get();
                var deals = results.Models;
                var responseDeals = deals.Select(deal => new DealRequests.NewDealResponse
                {
                    deal_id = deal.deal_id,
                    deal_name = deal.deal_name,
                    deal_stage = deal.deal_stage,
                    value = deal.value
                }).ToList();

                return Results.Ok(responseDeals);
            });

            app.MapGet("/api/deals/{id}", async (HttpContext httpContext, int id, Client client) =>
            {
                var (isAuthenticated, errorMessage) = await CookieHelper.Authenticate(httpContext, client);
                if (!isAuthenticated)
                {
                    return Results.Unauthorized();
                }

                var response = await client
                    .From<Deals>()
                    .Where(n => n.deal_id == id)
                    .Get();
                var deal = response.Models.FirstOrDefault();
                if (deal is null)
                {
                    return Results.NotFound();
                }

                var dealResponse = new DealRequests.NewDealResponse
                {
                    deal_id = deal.deal_id,
                    deal_name = deal.deal_name,
                    deal_stage = deal.deal_stage,
                    value = deal.value
                };
                return Results.Ok(dealResponse);
            });

            app.MapDelete("/api/deals/{id}", async (HttpContext httpContext, int id, Client client) =>
            {
                var (isAuthenticated, errorMessage) = await CookieHelper.Authenticate(httpContext, client);
                if (!isAuthenticated)
                {
                    return Results.Unauthorized();
                }

                await client
                    .From<Deals>()
                    .Where(n => n.deal_id == id)
                    .Delete();
                return Results.NoContent();
            });
        }
    }
}
