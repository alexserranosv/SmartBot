using Bot.DTos;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Extensions.Options;

namespace Bot.Dialogs
{
    public class GuatemalaDialog : ComponentDialog
    {
        private IOptions<AppSettingsModel> _settings;
        public GuatemalaDialog(IOptions<AppSettingsModel> settings)
        {
            _settings = settings;
        }
    }
}
