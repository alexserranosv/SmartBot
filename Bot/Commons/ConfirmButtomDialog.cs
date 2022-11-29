using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bot.Commons
{
    public class ConfirmButtomDialog
    {
        public static async Task<DialogTurnResult> ShowOption(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var options = await stepContext.PromptAsync(
                    nameof(ChoicePrompt),
                    new PromptOptions
                    {
                        Prompt = MessageFactory.Text("¿Aceptas los términos y condiciones?"),
                        Choices = ChoiceFactory.ToChoices(new List<string> { "Car", "Bus", "Bicycle" }),
                        Style = Microsoft.Bot.Builder.Dialogs.Choices.ListStyle.HeroCard
                    },
                    cancellationToken);

            return options;
        }

        public static async Task<DialogTurnResult> ShowOptionsEndEventoDialog(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var options = await stepContext.PromptAsync(
                    nameof(ConfirmPrompt),
                    new PromptOptions
                    {
                        Prompt = MessageFactory.Text("¿Necesitas más Información de este Evento?"),
                        Style = Microsoft.Bot.Builder.Dialogs.Choices.ListStyle.HeroCard
                    },
                    cancellationToken);

            return options;
        }
    }
}
