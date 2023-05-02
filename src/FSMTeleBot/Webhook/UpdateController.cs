using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Telegram.Bot.Types;

namespace FSMTeleBot.Webhook;
public class UpdateController : WebApiController
{
    [Route(HttpVerbs.Post, "update")]
    public async Task UpdateAsync()
    {
        var update = await HttpContext.GetRequestDataAsync<Update>();        
    }
}
