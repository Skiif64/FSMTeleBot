using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using FSMTeleBot.Services;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace FSMTeleBot.Webhook;
internal class UpdateController : WebApiController //TODO: make public?
{
    //private readonly InternalUpdateHandler _updateHandler; //TODO: use interface

    //public UpdateController(InternalUpdateHandler updateHandler)
    //{
        //_updateHandler = updateHandler;
    //}

    [Route(HttpVerbs.Post, "/update")]
    public async Task UpdateAsync()
    {       
        var update = await HttpContext.GetRequestDataAsync<Update>(DeserializeUpdateAsync);  
        if(update is null)
        {
            HttpContext.Response.StatusCode = 400;
            return;
        }
        //await _updateHandler.HandleUpdateAsync(update, CancellationToken);
    }

    private static async Task<Update> DeserializeUpdateAsync(IHttpContext context) 
        => JsonConvert.DeserializeObject<Update>(await context.GetRequestBodyAsStringAsync());        
    
}
