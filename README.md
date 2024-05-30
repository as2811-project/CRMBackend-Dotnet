This is an API for the CRM application repo built using .NET minimal APIs and Supabase. Functionalities include: CRUD operations on Contacts, Deals, Accounts, Tasks etc., Role Based Access Control and authentication. 

WIP: Extend RBAC to other modules, Custom Claims

Notes:

* CookieHelper.cs: Receives JWT and Refresh Token from the client to be passed on to the modules (Contacts, Deals etc). This is used for RBAC along with RLS policies on Supabase. 
