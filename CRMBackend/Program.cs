using System.Text;
using CRMBackend.Contracts;
using CRMBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Supabase;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddScoped<Supabase.Client>(_ =>
    new Supabase.Client(
        builder.Configuration["SupabaseUrl"],
        builder.Configuration["SupabaseKey"],
        new SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true
        }));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

string jwtToken = string.Empty;
string refreshToken = string.Empty;

app.MapPost("/auth/login", async (
    AuthService.Login request, Client client) =>
{
    var session = await client.Auth.SignIn(request.email, request.password);

    // Assuming the session object contains the JWT in a property called AccessToken
    jwtToken = session.AccessToken;
    refreshToken = session.RefreshToken;
    return Results.Ok(session);
});


app.MapPost("/auth/reg", async (
    AuthService.CreateUser request, Client client) =>
{
    var response = await client.Auth.SignUp(request.email, request.password);
    return Results.Ok(response);
});

app.MapPost("/api/contacts", async (
    HttpContext httpContext,
    CreateContactRequest request,
    Supabase.Client client) =>
{
    if (string.IsNullOrEmpty(jwtToken))
    {
        return Results.Unauthorized();
    }
    httpContext.Request.Headers["Authorization"] = $"`Bearer {jwtToken}`";
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

app.MapGet("/api/contacts", async (HttpContext httpContext, Supabase.Client client) =>
{
    if (string.IsNullOrEmpty(jwtToken))
    {
        return Results.Unauthorized();
    }

    await client.Auth.SetSession(jwtToken, refreshToken, false);
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

app.MapGet("/api/contacts/{id}", async (HttpContext httpContext, int id, Supabase.Client client) =>
{
    if (string.IsNullOrEmpty(jwtToken))
    {
        return Results.Unauthorized();
    }
    await client.Auth.SetSession(jwtToken, refreshToken, false);
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

app.MapDelete("/api/contacts/{id}", async (int id, Supabase.Client client) =>
{
    await client
        .From<Contacts>()
        .Where(n => n.contact_id == id)
        .Delete();
    return Results.NoContent();
});

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



app.UseHttpsRedirection();
app.Run();