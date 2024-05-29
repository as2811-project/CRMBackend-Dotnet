using CRMBackend.Contracts;
using CRMBackend.Models;

namespace CRMBackend.Modules;
using Microsoft.AspNetCore.Routing;


public static class DealsModule
{
    public static void DealsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/deals", async (
            DealRequests.CreateDealRequest request,
            Supabase.Client client) =>
        {
            var deal = new Deals
            {
                deal_name = request.deal_name,
                value = request.value,
                deal_stage = request.deal_stage,
                product = request.product
            };
            var response = await client.From<Deals>().Insert(deal);
            var newDeal = response.Models.First();
            return Results.Ok(newDeal.deal_id);
        });

        app.MapGet("/api/deals/{id}", async (int id, Supabase.Client client) =>
        {
            var response = await client
                .From<Deals>()
                .Where(n => n.deal_id == id)
                .Get();
            var deal = response.Models.FirstOrDefault();
            if (deal is null)
            {
                return Results.NotFound();
            }

            var dealResponse = new DealRequests.NewDealResponse()
            {
                deal_id = deal.deal_id,
                deal_name = deal.deal_name,
                deal_stage = deal.deal_stage,
                value = deal.value
            };
            return Results.Ok(dealResponse);
        });
    }
}