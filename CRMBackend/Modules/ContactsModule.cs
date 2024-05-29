using CRMBackend.Contracts;
using CRMBackend.CookieService;
using CRMBackend.Models;
using Microsoft.AspNetCore.Routing;
using Supabase;

namespace CRMBackend.Modules
{
    public static class ContactsModule
    {
        public static void ContactsEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/contacts", async (
                HttpContext httpContext,
                CreateContactRequest request,
                Client client) =>
            {
                var (isAuthenticated, errorMessage) = await CookieHelper.Authenticate(httpContext, client);
                if (!isAuthenticated)
                {
                    return Results.Unauthorized();
                }

                var contact = new Contacts
                {
                    contact_id = request.contact_id,
                    first_name = request.first_name,
                    last_name = request.last_name,
                    email = request.email,
                    phone_number = request.phone_number,
                    address = request.address
                };
                var response = await client.From<Contacts>().Insert(contact);
                var newContact = response.Models.First();
                return Results.Ok(newContact.contact_id);
            });

            app.MapGet("/api/contacts", async (HttpContext httpContext, Client client) =>
            {
                var (isAuthenticated, errorMessage) = await CookieHelper.Authenticate(httpContext, client);
                if (!isAuthenticated)
                {
                    return Results.Unauthorized();
                }

                var results = await client.From<Contacts>().Get();
                var contacts = results.Models;

                var responseContacts = contacts.Select(contact => new NewContactResponse
                {
                    contact_id = contact.contact_id,
                    first_name = contact.first_name,
                    last_name = contact.last_name,
                    email = contact.email
                }).ToList();

                return Results.Ok(responseContacts);
            });

            app.MapGet("/api/contacts/{id}", async (HttpContext httpContext, int id, Client client) =>
            {
                var (isAuthenticated, errorMessage) = await CookieHelper.Authenticate(httpContext, client);
                if (!isAuthenticated)
                {
                    return Results.Unauthorized();
                }

                var response = await client
                    .From<Contacts>()
                    .Where(n => n.contact_id == id)
                    .Get();
                var contact = response.Models.FirstOrDefault();
                if (contact is null)
                {
                    return Results.NotFound();
                }

                var contactResponse = new NewContactResponse
                {
                    contact_id = contact.contact_id,
                    first_name = contact.first_name,
                    last_name = contact.last_name,
                    email = contact.email
                };
                return Results.Ok(contactResponse);
            });

            app.MapDelete("/api/contacts/{id}", async (HttpContext httpContext, int id, Client client) =>
            {
                var (isAuthenticated, errorMessage) = await CookieHelper.Authenticate(httpContext, client);
                if (!isAuthenticated)
                {
                    return Results.Unauthorized();
                }

                await client
                    .From<Contacts>()
                    .Where(n => n.contact_id == id)
                    .Delete();
                return Results.NoContent();
            });
        }
    }
}
