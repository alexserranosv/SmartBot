using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder;
using System;

namespace Bot.Services
{
    public class BotStateService
    {
        public ConversationState ConversationState { get; }
        public UserState UserState { get; }

        public BotStateService(UserState userState, ConversationState conversationState)
        {
            UserState = userState ?? throw new ArgumentNullException(nameof(userState));
            ConversationState = conversationState ?? throw new ArgumentNullException(nameof(conversationState));



            //Inicializamos los accessors
            InitializeAccessors();
        }

        #region de Ids
        /// <summary>
        /// Identificador unico para el objeto de datos de la conversaion
        /// </summary>
        public string ConversationDataId { get; } = $"{nameof(BotStateService)}.ConversationData";

        /// <summary>
        /// Identificador unico para este state service para todos los dialogos que se generen
        /// </summary>
        public string DialogStateId { get; } = $"{nameof(BotStateService)}.DialogState";
        #endregion

      

        public IStatePropertyAccessor<DialogState> DialogStateAccessor { get; set; }


        /// <summary>
        /// Metodo que me crea los accessors con las variables definidas por mi para cada estado
        /// </summary>
        public void InitializeAccessors()
        {
            //Creamos el accessor del dialogo usando el conversation state y su id de dialogo
            DialogStateAccessor = ConversationState.CreateProperty<DialogState>(DialogStateId);
            
        }
    }
}
