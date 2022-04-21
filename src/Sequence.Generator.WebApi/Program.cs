using StackExchange.Redis;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

WebApplication app = builder.Build();

string redisConnectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING") ?? "localhost:6379";

System.Console.WriteLine($"Using redis connection string: {redisConnectionString}");

ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(redisConnectionString);

app.MapGet(@"/api/seq/{key}/{prefix=}/{suffix=}/{digits:int=10}", async (string key, string prefix, string suffix, int digits) =>
{
    System.Console.WriteLine($"... : {key}");
    try
    {
        IDatabase db = redis.GetDatabase();

        long nextValue = await db.StringIncrementAsync(new RedisKey(key), 1);

        string value = nextValue.ToString($"D{digits}");

        return Results.Ok($"{prefix}{value}{suffix}");
    }
    catch (Exception ex)
    {
        System.Console.WriteLine(ex);

        return Results.BadRequest(ex.Message);
    }
});

app.Run();
