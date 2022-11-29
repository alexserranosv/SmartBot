using Bot.Commons;
using Bot.Functions;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bot.Dialogs
{
    public class MenuEventoDialog : ComponentDialog
    {
        private string _evento;
        private string _dominio;
        private string _pais;
        public MenuEventoDialog(string dominio, string evento, string pais)
        {                        
            _evento = evento;
            _dominio = dominio;
            _pais = pais;

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog) + ".evento", new WaterfallStep[]
            {
                ShowMenuEvento,
                ShowOpcionSolicitada,
                AskEndDialog,
                Confirmations
            }));

            InitialDialogId = nameof(WaterfallDialog) + ".evento";
        }
        

        private async Task<DialogTurnResult> ShowMenuEvento(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await PrompMultiChoiceDialog.ShowOptionEvento(stepContext, cancellationToken);
        }

        private async Task<DialogTurnResult> ShowOpcionSolicitada(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var option = stepContext.Context.Activity.Text;
            var token = await FuncionesBot.GetTokenSmartAsync(_dominio);

            switch (option)
            {
                case "Lugar y fechas":
                    
                    var listaLugarFechas = await FuncionesBot.GetListaFechasBotAsync(_dominio, token, _evento);
                    foreach (var fecha in listaLugarFechas)
                    {
                        await stepContext.Context.SendActivityAsync("" +
                            $"*Fecha:* {fecha.fecha}\r\n\r\n" +
                            $"*Horario:* {fecha.hora}\r\n\r\n" +
                            $"*Lugar:* {fecha.lugar}", cancellationToken: cancellationToken);
                    }
                    break;

                case "Quiero saber precios":
                    var listaPrecios = await FuncionesBot.GetListaPreciosBotAsync(_dominio, token, _evento);
                    foreach (var precio in listaPrecios)
                    {
                        await stepContext.Context.SendActivityAsync("" +
                            $"*Localidad:* {precio.nombre}\r\n\r\n" +
                            $"*Precio:* {precio.divisa} {Math.Round(precio.valor, 2)}\r\n\r\n" +
                            $"*Cargo por boletería:* {precio.divisa} {Math.Round(precio.recargoServicio, 2)}\r\n\r\n" +
                            $"*Disponibilidad:* {precio.soldOut}", cancellationToken: cancellationToken);
                    }
                    break;

                case "¿Cómo puedo comprar?":
                        await stepContext.Context.SendActivityAsync("Puedes comprar en línea en www.smartticket.fun con tarjeta o Tigo Money. " +
                            "Si quieres pagar en efectivo puedes llegar a nuestro Kiosco en centro comercial Galerías Escalón, 3er Nivel frente " +
                            "a la entrada de Siman. Abrimos Lunes a Domingo de 10AM a 6PM cerramos de 2PM a 3PM.\r\n\r\nTambién puedes reservar " +
                            "tus tickets en www.smartticket.fun y pagar en efectivo en cualquier punto express del país.", cancellationToken: cancellationToken);
                    break;

                case "¿Taquilla día evento?":
                    var listaLugarFechas2 = await FuncionesBot.GetListaFechasBotAsync(_dominio, token, _evento);
                    foreach (var fecha in listaLugarFechas2)
                    {
                        await stepContext.Context.SendActivityAsync("" +
                            $"*Fecha:* {fecha.fecha}\r\n\r\n" +
                            $"*Horario:* {fecha.hora}\r\n\r\n" +
                            $"*Lugar:* {fecha.lugar}", cancellationToken: cancellationToken);
                    }
                    break;

                case "¿Cartilla de vacunas?":
                    var eventoVacunas = await FuncionesBot.GetEventoBotAsync(_dominio, token, _evento);
                    await stepContext.Context.SendActivityAsync($"*Información Covid:* {eventoVacunas.infoCovid}", cancellationToken: cancellationToken);
                    break;

                case "Información de edades":
                    var eventoEdades = await FuncionesBot.GetEventoBotAsync(_dominio, token, _evento);
                    await stepContext.Context.SendActivityAsync($"*Restrición de edades:* {eventoEdades.restricEdades}", cancellationToken: cancellationToken);
                    break;

                default:

                    break;                
            }

            return await stepContext.ContinueDialogAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> AskEndDialog(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await PrompMultiChoiceDialog.ShowOptionsEndEventoDialog(stepContext, cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> Confirmations(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var respuesta = stepContext.Context.Activity.Text;

            if (respuesta == "Si")
                return await stepContext.BeginDialogAsync(
                            nameof(MenuEventoDialog),
                            null,
                            cancellationToken
                        );
            else
                return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }

    }
}
