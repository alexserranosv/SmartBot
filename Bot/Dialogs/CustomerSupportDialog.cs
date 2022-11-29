using Bitworks.Bot.Adapters.Bitdesk.Core.EndConversationCommands;
using Bot.Commons;
using Bot.DTos;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bot.Dialogs
{
    public class CustomerSupportDialog : ComponentDialog
    {
        private IOptions<AppSettingsModel> _settings;
        public CustomerSupportDialog(IOptions<AppSettingsModel> settings)
        {
            _settings = settings;

            var waterfallStep = new WaterfallStep[]
            {
                ShowMenuOptions,
                ShowOpcionSolicitada
            };

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog) + ".support", waterfallStep));
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new MenuAyuda(settings));

            InitialDialogId = nameof(WaterfallDialog) + ".support";
        }
        

        private async Task<DialogTurnResult> ShowMenuOptions(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync("¿Ya realizaste una compra o tienes una consulta antes de comprar?", cancellationToken: cancellationToken);
            return await PrompMultiChoiceDialog.ShowOptionYaRealizoCompra(stepContext, cancellationToken);
        }

        private async Task<DialogTurnResult> ShowOpcionSolicitada(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var option = stepContext.Context.Activity.Text;

            switch (option)
            {
                case "Ya realicé mi compra":
                    return await stepContext.BeginDialogAsync(
                            nameof(MenuAyuda),
                            null,
                            cancellationToken
                        );

                case "Tengo una consulta":
                    await stepContext.Context.SendActivityAsync("Ok, te hemos asignado a un miembro del equipo de servicio al cliente.\r\n\r\n" +
                        "Asistente virtual se despide.  👋🏻", cancellationToken: cancellationToken);

                    ReclasificarCommand action = new ReclasificarCommand
                    {
                        IdArea = 2022,
                        Razon = "Elección del cliente en conversación con el bot"
                    };

                    string json = JsonConvert.SerializeObject(action);

                    Activity activity = new Activity
                    {
                        Type = ActivityTypes.EndOfConversation,
                        ChannelData = json,
                        Code = ReclasificarCommand.CommandName
                    };

                    await stepContext.Context.SendActivityAsync(activity, cancellationToken);
                    return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);

                default:
                    return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
            }
        }
    }   
}
