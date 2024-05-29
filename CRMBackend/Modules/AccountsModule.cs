using CRMBackend.Contracts;
using CRMBackend.Models;

namespace CRMBackend.Modules;

public static class AccountsModule
{
    public static void AccountsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/accounts", async (
            AccountRequests.CreateAccount request,
            Supabase.Client client) =>
        {
            var account = new Accounts
            {
                company_name = request.company_name,
                revenue = request.revenue,
                employee_count = request.employee_count,
                founded_date = request.founded_date,
                phone_number = request.phone_number
            };
            var response = await client.From<Accounts>().Insert(account);
            var newDeal = response.Models.First();
            return Results.Ok(newDeal.account_id);
        });
    }
}