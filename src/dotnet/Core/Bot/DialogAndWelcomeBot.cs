// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.22.0

using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Chat;
using FoundationaLLM.Core.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FoundationaLLM.Core.Bot
{
    public class DialogAndWelcomeBot<T> : DialogBot<T>
        where T : Dialog
    {
        public DialogAndWelcomeBot(ConversationState conversationState, UserState userState, T dialog, ILogger<DialogBot<T>> logger, ICoreService coreService, ICallContext callContext)
            : base(conversationState, userState, dialog, logger, coreService, callContext)
        {
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                // Greet anyone that was not the target (recipient) of this message.
                // To learn more about Adaptive Cards, see https://aka.ms/msbot-adaptivecards for more details.
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    //create a new session
                    CallContext.CurrentUserIdentity = new Common.Models.Authentication.UnifiedUserIdentity { Name = member.Id, UPN = member.Id, Username = member.Id };
                    
                    Session session = await CoreService.CreateNewChatBotSessionAsync(member.Id, member.Id);

                    var errorMessageText = "Connected to FLLM!";
                    var errorMessage = MessageFactory.Text(errorMessageText, errorMessageText, InputHints.ExpectingInput);
                    await turnContext.SendActivityAsync(errorMessage, cancellationToken);
                    await Dialog.RunAsync(turnContext, ConversationState.CreateProperty<DialogState>("DialogState"), cancellationToken);
                }
            }
        }
    }
}
