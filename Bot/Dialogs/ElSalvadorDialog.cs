using Bot.Commons;
using Bot.DTos;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bot.Dialogs
{
    public class ElSalvadorDialog : ComponentDialog
    {
        private IOptions<AppSettingsModel> _settings;
        public ElSalvadorDialog(IOptions<AppSettingsModel> settings)
        {
            _settings = settings;

            var waterfallStep = new WaterfallStep[]
            {
                ShowMenuPrincipal,
                ShowOpcionSolicitada,
                ShowMenuEvento,
                AskEndDialog,
                Confirmations
            };

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog) + ".sv", waterfallStep));
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));

            AddDialog(new CustomerSupportDialog(settings));

            InitialDialogId = nameof(WaterfallDialog) + ".sv";
        }        

        private async Task<DialogTurnResult> ShowMenuPrincipal(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await PrompMultiChoiceDialog.ShowOptionElSalvador(stepContext, cancellationToken);
        }

        private async Task<DialogTurnResult> ShowOpcionSolicitada(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var option = stepContext.Context.Activity.Text;

            switch (option)
            {
                case "Info de Eventos":
                        return await PrompMultiChoiceDialog.ShowListaEventos(stepContext, cancellationToken, _settings.Value.Domain, "SV");

                case "¿Cómo puedo comprar?":
                        await stepContext.Context.SendActivityAsync("Puedes comprar en línea en www.smartticket.fun con tarjeta o Tigo Money. " +
                            "Si quieres pagar en efectivo puedes llegar a nuestro Kiosco en centro comercial Galerías Escalón, 3er Nivel frente a la entrada de Siman. " +
                            "Abrimos Lunes a Domingo de 10AM a 6PM cerramos de 2PM a 3PM.\r\n " +
                            "También puedes reservar tus tickets en www.smartticket.fun y pagar en efectivo en " +
                            "cualquier punto express del país.", cancellationToken: cancellationToken);
                        return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);

                case "Ubicación y horarios":
                        await stepContext.Context.SendActivityAsync("Estamos ubicados en centro comercial Galerías Escalón, 3er Nivel frente a la entrada de Siman. " +
                            "Abrimos Lunes a Domingo de 10AM a 6PM cerramos de 2PM a 3PM. Puedes comprar en línea en nuestro sitio web www.smartticket.fun\r\n\r\n" +
                            "También puedes reservar tus tickets en www.smartticket.fun y pagar en efectivo en cualquier punto express del país.", cancellationToken: cancellationToken);
                        return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);

                case "¿Imprimo mis tickets?":
                        await stepContext.Context.SendActivityAsync("Para ingresar al evento solo debes descargar el E-Ticket el cual es un PDF que te llegó adjunto" +
                            " en un correo al momento de realizar tu compra. También puedes descargar el Ticket desde la sección de \"mis tickets\" de nuestro sitio " +
                            "web https://www.smartticket.fun Puedes presentar tu Ticket desde tu celular o puedes imprimirlo.", cancellationToken: cancellationToken);
                        return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);

                case "¿Cargo de boletería?":
                        await stepContext.Context.SendActivityAsync("El cargo de boletería es el valor de nuestro servicio para que tu compra de tus eventos favoritos " +
                            "sea fácil y desde cualquier lugar.", cancellationToken: cancellationToken);
                        return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);

                case "Hablar con un agente":
                    return await stepContext.BeginDialogAsync(
                        nameof(CustomerSupportDialog),
                        null,
                        cancellationToken
                    );

                default:
                    return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);                    
            }
            
        }

        private async Task<DialogTurnResult> ShowMenuEvento(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var evento = stepContext.Context.Activity.Text;

            AddDialog(new MenuEventoDialog(_settings.Value.Domain, evento, "SV"));
            return await stepContext.BeginDialogAsync(
                            nameof(MenuEventoDialog),
                            null,
                            cancellationToken
                        );
        }

        private async Task<DialogTurnResult> AskEndDialog(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await PrompMultiChoiceDialog.ShowOptionsEndElSalvadorDialog(stepContext, cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> Confirmations(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var respuesta = stepContext.Context.Activity.Text;

            if (respuesta == "Si")
                return await stepContext.BeginDialogAsync(
                            nameof(ElSalvadorDialog),
                            null,
                            cancellationToken
                        );
            else
                return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }
    }
}
