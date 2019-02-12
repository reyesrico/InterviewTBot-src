using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;

namespace BasicBot.Dialogs.Greeting
{
    public class AddCalendarDialog : ComponentDialog
    {
        // Dialog IDs
        private const string ProfileDialog = "profileDialog";

        public AddCalendarDialog(IStatePropertyAccessor<AddCalendarState> userProfileStateAccessor, ILoggerFactory loggerFactory)
            : base(nameof(AddCalendarDialog))
        {
            UserProfileAccessor = userProfileStateAccessor ?? throw new ArgumentNullException(nameof(userProfileStateAccessor));

            // Add control flow dialogs
            var waterfallSteps = new WaterfallStep[]
            {
                PromptForCalendarStepAsync,
            };
            AddDialog(new WaterfallDialog(ProfileDialog, waterfallSteps));
        }

        public IStatePropertyAccessor<AddCalendarState> UserProfileAccessor { get; }

        private async Task<DialogTurnResult> PromptForCalendarStepAsync(
                                                WaterfallStepContext stepContext,
                                                CancellationToken cancellationToken)
        {
            var greetingState = await UserProfileAccessor.GetAsync(stepContext.Context);

            // if we have everything we need, greet user and return.
            if (greetingState != null)
            {
                var opts = new PromptOptions
                {
                    Prompt = new Activity
                    {
                        Type = ActivityTypes.Message,
                        Text = "Add calendar?",
                    },
                };

                return await stepContext.PromptAsync("add", opts);
            }
            else
            {
                return await stepContext.NextAsync();
            }
        }
    }
}
