using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace FSMTeleBot.Webhook;
public class UpdateController : WebApiController
{
    [Route(HttpVerbs.Post, "update")]
    public async Task UpdateAsync()
    {       
        var update = await HttpContext.GetRequestDataAsync<Update>(DeserializeUpdateAsync);    
        
    }

    private static async Task<Update> DeserializeUpdateAsync(IHttpContext context)
    {
        var serializer = JsonSerializer.CreateDefault();
        using var stringReader = new StringReader(await context.GetRequestBodyAsStringAsync());
        using var jsonReader = new JsonTextReader(stringReader);
        return serializer.Deserialize<Update>(jsonReader);
    }
}
