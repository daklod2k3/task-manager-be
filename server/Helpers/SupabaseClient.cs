using System.Diagnostics;
using Supabase.Gotrue;
using Supabase.Gotrue.Interfaces;
using static Supabase.Gotrue.StatelessClient;


namespace server.Helpers;

public class SupabaseClient
{
    public static IGotrueClient<User, Session> AuthClient;
    
    public SupabaseClient()
    {
        Console.WriteLine(Environment.GetEnvironmentVariable("SUPABASE_URL") + "/auth/v1");
        AuthClient = new Supabase.Gotrue.Client(new ClientOptions
        {
            Url = Environment.GetEnvironmentVariable("SUPABASE_URL") + "/auth/v1",
            Headers = new Dictionary<string, string>
            {
                { "apikey", Environment.GetEnvironmentVariable("SUPABASE_PUB_KEY") },
            }
        });
    }
}