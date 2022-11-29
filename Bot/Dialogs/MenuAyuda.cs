using Bot.DTos;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bot.Dialogs
{
    public class MenuAyuda : ComponentDialog
    {
        private IOptions<AppSettingsModel> _settings;
    
        public MenuAyuda(IOptions<AppSettingsModel> settings)
        {
            _settings = settings;

            var waterfallStep = new WaterfallStep[]
            {
                ShowMenuOptions,
                ShowOpcionSolicitada
            };

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog) + ".ayuda", waterfallStep));
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));

            InitialDialogId = nameof(WaterfallDialog) + ".ayuda";
        }        

        private async Task<DialogTurnResult> ShowMenuOptions(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync("HOLA", cancellationToken: cancellationToken);
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }

        private Task<DialogTurnResult> ShowOpcionSolicitada(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
