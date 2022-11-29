using Bot.Commons;
using Bot.DTos;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bot.Dialogs
{
    public class RootDialog : ComponentDialog
    {
        private IOptions<AppSettingsModel> _settings;
        public RootDialog(IOptions<AppSettingsModel> settings)
        {
            _settings = settings;

            var waterfallStep = new WaterfallStep[]
            {                
                //SetName, SetAge, ShowData,

                //ShowOptions, ShowDocumentation,                

                //ShowHeroCardOption, ResponseOption

                //ShowOptions2, Confirmations,

                ShowMenuPais,
                ShowMenuPrincipal,
                ShowDespedida
            };            

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog) + ".main", waterfallStep));
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new NumberPrompt<int>(nameof(NumberPrompt<int>), ValidateAge));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt), ConfirmValidate));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));

            AddDialog(new ElSalvadorDialog(_settings));
            AddDialog(new GuatemalaDialog(_settings));            

            InitialDialogId = nameof(WaterfallDialog) + ".main";
        }

        

        private async Task<DialogTurnResult> ShowMenuPais(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {            
            return await PrompMultiChoiceDialog.ShowOptionPais(stepContext, cancellationToken);
        }

        private async Task<DialogTurnResult> ShowMenuPrincipal(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var pais = stepContext.Context.Activity.Text;

            switch (pais)
            {
                case "El Salvador":

                    return await stepContext.BeginDialogAsync(
                            nameof(ElSalvadorDialog),
                            null,
                            cancellationToken
                        );

                case "Guatemala":
                    return await stepContext.BeginDialogAsync(
                            nameof(GuatemalaDialog),
                            null,
                            cancellationToken
                        );

                default:
                    return await stepContext.ContinueDialogAsync( cancellationToken: cancellationToken );
            }           
            
        }

        private async Task<DialogTurnResult> ShowDespedida(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync("Excelente, fue un gusto ayudarte, no olvides darle Like a nuestra fan page en Facebook o Instagram.❤️", cancellationToken: cancellationToken);
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }



        private async Task<DialogTurnResult> SetName(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync("Para iniciar una convesación necesito algunos datos.", cancellationToken: cancellationToken);
            await Task.Delay(100);
            return await stepContext.PromptAsync(
                    nameof(TextPrompt),
                    new PromptOptions { Prompt = MessageFactory.Text("Por favor ingrese su nombre")},
                    cancellationToken
                );
        }

        private async Task<DialogTurnResult> SetAge(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var name = stepContext.Context.Activity.Text;
            return await stepContext.PromptAsync(
                    nameof(NumberPrompt<int>),
                    new PromptOptions
                    {
                        Prompt = MessageFactory.Text($"Bien {name}, ahora ingresa tu edad"),
                        RetryPrompt = MessageFactory.Text($"{name}, por favor ingresa una edad valida")
                    },
                    cancellationToken
                );
        }

        private async Task<bool> ValidateAge(PromptValidatorContext<int> promptContext, CancellationToken cancellationToken)
        {
            return await Task.FromResult( 
                    promptContext.Recognized.Succeeded &&
                    promptContext.Recognized.Value > 0 &&
                    promptContext.Recognized.Value < 150
                );
        }

        private async Task<DialogTurnResult> ShowData(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync("Gracias por registrar sus datos  ", cancellationToken: cancellationToken);
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> ShowOptions(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //MOSTAR BOTONES
            return await OptionsDocumentationDialog.ShowOptions(stepContext, cancellationToken);
        }

        private async Task<DialogTurnResult> ShowDocumentation(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var option = stepContext.Context.Activity.Text;

            switch (option)
            {
                case ".NET":
                    await stepContext.Context.SendActivityAsync("Documentación .NET: https://learn.microsoft.com/es-es/dotnet/", cancellationToken: cancellationToken);
                    break;
                case "JavaScript":
                    await stepContext.Context.SendActivityAsync("Documentación JavaScript: https://developer.mozilla.org/en-US/docs/Web/JavaScript", cancellationToken: cancellationToken);
                    break;
                case "Python":
                    await stepContext.Context.SendActivityAsync("Documentación Python: https://docs.python.org/3/", cancellationToken: cancellationToken);
                    break;
                default:
                    break;
            }

            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> ShowOptions2(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {            
            //MOSTRAR OPCIONES DE CONFIRMACIÓN
            return await ConfirmButtomDialog.ShowOption(stepContext, cancellationToken);
        }

        private async Task<DialogTurnResult> Confirmations(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {            
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }

        private async Task<bool> ConfirmValidate(PromptValidatorContext<bool> promptContext, CancellationToken cancellationToken)
        {
            var option = promptContext.Recognized.Value;

            if (option) 
                await promptContext.Context.SendActivityAsync("Tus datos han sido registrados", cancellationToken: cancellationToken);
            else 
                await promptContext.Context.SendActivityAsync("Lo sentimos", cancellationToken: cancellationToken);

            return true;
        }

        private async Task<DialogTurnResult> ShowHeroCardOption(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await HeroCardDialog.ShowOptions(stepContext, cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> ResponseOption(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var option = stepContext.Context.Activity.Text;
            await stepContext.Context.SendActivityAsync($"Seleccionaste: {option}", cancellationToken: cancellationToken);
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }
        
    }
}
