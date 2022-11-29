using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bot.Commons
{
    public class HeroCardDialog
    {
        public static async Task<DialogTurnResult> ShowOptions(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var option = await stepContext.PromptAsync(
                    nameof(TextPrompt),
                    new PromptOptions
                    {
                        Prompt = CreateHeroCard()
                    },
                    cancellationToken
                );

            return option;
        }

        private static Activity CreateHeroCard()
        {
            var HeroCard = new HeroCard()
            {
                Title = "Bot Framework",
                Subtitle = "Microsoft Bot Framework v4",
                Buttons = new List<CardAction>()
                {
                    new CardAction() {Title = "Documentation", Value = "Documentation", Type = ActionTypes.ImBack},
                    new CardAction() {Title = "Report", Value = "Report", Type = ActionTypes.ImBack},
                    new CardAction() {Title = "Ir a la web", Value = "https://learn.microsoft.com/es-es/dotnet/", Type = ActionTypes.OpenUrl},
                } 
            };

            return MessageFactory.Attachment(HeroCard.ToAttachment()) as Activity;
        }
    }
}
