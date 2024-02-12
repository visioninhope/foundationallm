// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.22.0

using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Core.Interfaces;
using FoundationaLLM.Core.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;

namespace FoundationaLLM.Core.Dialogs
{
    public class MainDialog : ComponentDialog
    {
        private readonly ILogger _logger;
        private readonly ICoreService _coreService;
        private string _agent;

        // Dependency injection uses this constructor to instantiate MainDialog
        public MainDialog(
            ICoreService coreService,
            ILogger<MainDialog> logger)
            : base(nameof(MainDialog))
        {
            _logger = logger;
            _coreService = coreService;
            _agent = "FLLMDocumentation";

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            
            var waterfallSteps = new WaterfallStep[]
            {
                IntroStepAsync,
                ActStepAsync
            };

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> IntroStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Use the text provided in FinalStepAsync or the default if it is the first time.
            var messageText = stepContext.Options?.ToString() ?? "What can I help you with today?";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> ActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            DirectCompletionRequest directCompletionRequest = new DirectCompletionRequest();
            directCompletionRequest.UserPrompt = stepContext.Context.Activity.Text;

            //send to FLLM...
            var completionResponse = await _coreService.GetChatBotCompletionAsync(stepContext.Context.Activity.From.Id, stepContext.Context.Activity.From.Id, stepContext.Context.Activity.Text, _agent);

            //var completionResponse = await _coreService.GetCompletionAsync(directCompletionRequest);

            return await stepContext.ReplaceDialogAsync(InitialDialogId, completionResponse.Text, cancellationToken);
        }
    }
}
