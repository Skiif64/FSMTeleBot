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
        if(update is null)
        {
            HttpContext.Response.StatusCode = 400;

            return;
        }
        
    }

    private static async Task<Update> DeserializeUpdateAsync(IHttpContext context) 
        => JsonConvert.DeserializeObject<Update>(await context.GetRequestBodyAsStringAsync());        
    
}
