using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Bot.Functions;

namespace Bot.Commons
{
    public class PrompMultiChoiceDialog
    {
        public static async Task<DialogTurnResult> ShowOptionPais(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var options = await stepContext.PromptAsync(
                    nameof(ChoicePrompt),
                    new PromptOptions
                    {
                        Prompt = MessageFactory.Text("Primero cuéntame el país del evento de tu interés❓(bot 2.0 )"),
                        Choices = ChoiceFactory.ToChoices(new List<string> { "El Salvador", "Guatemala" }),
                        Style = Microsoft.Bot.Builder.Dialogs.Choices.ListStyle.HeroCard
                    },
                    cancellationToken);

            return options;
        }

        public static async Task<DialogTurnResult> ShowOptionElSalvador(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var options = await stepContext.PromptAsync(
                    nameof(ChoicePrompt),
                    new PromptOptions
                    {
                        Prompt = MessageFactory.Text("Elige una de estas opciones 👇"),
                        Choices = ChoiceFactory.ToChoices(new List<string> {
                                        "Info de Eventos",
                                        "¿Cómo puedo comprar?",
                                        "Ubicación y horarios",
                                        "¿Imprimo mis tickets?",
                                        "¿Cargo de boletería?",
                                        "Hablar con un agente"
                                    }),
                        Style = Microsoft.Bot.Builder.Dialogs.Choices.ListStyle.HeroCard
                    },
                    cancellationToken);

            //var token = await FuncionesBot.GetTokenSmartAsync("https://apismartticketcomsv.azurewebsites.net");
            //var prueba = await FuncionesBot.GetListaPreciosBotAsync("https://apismartticketcomsv.azurewebsites.net", token, "Aladdin");

            return options;
        }

        public static async Task<DialogTurnResult> ShowOptionGuatemala(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var options = await stepContext.PromptAsync(
                    nameof(ChoicePrompt),
                    new PromptOptions
                    {
                        Prompt = MessageFactory.Text("Elige una de estas opciones 👇"),
                        Choices = ChoiceFactory.ToChoices(new List<string> {
                                        "Info de Eventos",
                                        "¿Cómo puedo comprar?",
                                        "Ubicación y horarios",
                                        "¿Imprimo mis tickets?",
                                        "¿Cargo de boletería?",
                                        "Hablar con un agente"
                                    }),
                        Style = Microsoft.Bot.Builder.Dialogs.Choices.ListStyle.HeroCard
                    },
                    cancellationToken);

            return options;
        }

        public static async Task<DialogTurnResult> ShowOptionEvento(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var options = await stepContext.PromptAsync(
                    nameof(ChoicePrompt),
                    new PromptOptions
                    {
                        Prompt = MessageFactory.Text("¿Qué información necesitas? 🤔"),
                        Choices = ChoiceFactory.ToChoices(new List<string> {
                                        "Lugar y fechas",
                                        "Quiero saber precios",
                                        "¿Cómo puedo comprar?",
                                        "¿Taquilla día evento?",
                                        "¿Cartilla de vacunas?",
                                        "Información de edades"
                                    }),
                        Style = Microsoft.Bot.Builder.Dialogs.Choices.ListStyle.HeroCard
                    },
                    cancellationToken);

            return options;
        }

        public static async Task<DialogTurnResult> ShowOptionsEndEventoDialog(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var options = await stepContext.PromptAsync(
                    nameof(ChoicePrompt),
                    new PromptOptions
                    {
                        Prompt = MessageFactory.Text("¿Necesitas más Información de este Evento?"),
                        Choices = ChoiceFactory.ToChoices(new List<string> { "Si", "No" }),
                        Style = Microsoft.Bot.Builder.Dialogs.Choices.ListStyle.HeroCard
                    },
                    cancellationToken);

            return options;
        }

        public static async Task<DialogTurnResult> ShowOptionsEndElSalvadorDialog(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var options = await stepContext.PromptAsync(
                    nameof(ChoicePrompt),
                    new PromptOptions
                    {
                        Prompt = MessageFactory.Text("¿Necesitas más ayuda en algo más?"),
                        Choices = ChoiceFactory.ToChoices(new List<string> { "Si", "No" }),
                        Style = Microsoft.Bot.Builder.Dialogs.Choices.ListStyle.HeroCard
                    },
                    cancellationToken);

            return options;
        }

        public static async Task<DialogTurnResult> ShowListaEventos(WaterfallStepContext stepContext, CancellationToken cancellationToken, string dominio, string pais)
        {
            var token = await FuncionesBot.GetTokenSmartAsync(dominio);
            var eventos = await FuncionesBot.GetListaEventosBotAsync(dominio, token, pais);
            var lista = new List<string>();

            eventos.ForEach(evento => lista.Add(evento.nombreEvento));

            var options = await stepContext.PromptAsync(
                    nameof(ChoicePrompt),
                    new PromptOptions
                    {
                        Prompt = MessageFactory.Text("Elige unos de estos eventos👇"),
                        Choices = ChoiceFactory.ToChoices(lista),
                        Style = Microsoft.Bot.Builder.Dialogs.Choices.ListStyle.HeroCard
                    },
                    cancellationToken);

            return options;
        }

        public static async Task<DialogTurnResult> ShowOptionYaRealizoCompra(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var options = await stepContext.PromptAsync(
                    nameof(ChoicePrompt),
                    new PromptOptions
                    {
                        Prompt = MessageFactory.Text("Elige una opción 👇"),
                        Choices = ChoiceFactory.ToChoices(new List<string> { "Ya realicé mi compra", "Tengo una consulta" }),
                        Style = Microsoft.Bot.Builder.Dialogs.Choices.ListStyle.HeroCard
                    },
                    cancellationToken);

            return options;
        }
    }
}
