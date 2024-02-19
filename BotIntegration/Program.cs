using Azure;
using Microsoft.Azure.Cosmos;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.DirectLine;
using Microsoft.Bot.Connector.Authentication;
using System;

namespace BotIntreg
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            string directLineSecret = "DHE17RiKHgA.fr3JBNMqPjL6enHcZ_FXnAtnJCB-T0RATs7eN8mka3E";
            string botEndpoint = "https://XLN-bot-eeda.azurewebsites.net/api/messages";

            var client = new DirectLineClient(directLineSecret);

            var conversation = await client.Conversations.StartConversationAsync();

            Console.WriteLine("Please ask a question: ");

            while (true)
        {
            string userInput = Console.ReadLine();

            if (userInput.ToLower() == "exit")
                break;

            await client.Conversations.PostActivityAsync(conversation.ConversationId, new Activity
            {
                From = new ChannelAccount("User"),
                Text = userInput,
                Type = ActivityTypes.Message
            });

            var messages = await client.Conversations.GetActivitiesAsync(conversation.ConversationId);
            foreach (var message in messages.Activities)
            {
                if (message.From.Name == "XLN-bot")
                {
                    Console.WriteLine($"Bot: {message.Text}");
                }
            }
        }
        }
    }
}