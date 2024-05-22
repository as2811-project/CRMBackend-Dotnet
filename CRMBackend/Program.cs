using CRMBackend.Contracts;
using CRMBackend.Models;
using Supabase;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.MapPost("/api/contacts", async (
    CreateContactRequest request,
    Supabase.Client client) =>
{
    var contact = new Contacts
    {
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

app.MapGet("/api/contacts/{id}", async (int id, Supabase.Client client) =>
{
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

app.UseHttpsRedirection();
app.Run();