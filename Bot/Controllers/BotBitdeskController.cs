using Bitworks.Bot.Adapters;
using Bitworks.Bot.Adapters.Bitdesk;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bot.Controllers
{
    [Route("api/bitdesk/messages")]
    public class BotBitdeskController : Controller
    {
        private readonly IBitworksBotAdapter Adapter;
        private readonly IBot Bot;

        public BotBitdeskController(IEnumerable<IBitworksBotAdapter> adapters, IBot bot)
        {
            //Se utiliza una lista de IBitworksBotAdapter por si hay mas d eun adapter que se desea implementar
            Adapter = adapters.Single(x => x.GetType() == typeof(BitdeskAdapter));
            Bot = bot;
        }

        [HttpPost, HttpGet]
        public async Task PostAsync()
        {
            // Delegate the processing of the HTTP POST to the adapter.
            // The adapter will invoke the bot.
            await Adapter.ProcessAsync(Request, Response, Bot);
        }
    }
}
