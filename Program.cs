using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Web;

using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputMessageContents;
using Telegram.Bot.Types.ReplyMarkups;

using Newtonsoft.Json;
using Telegram_Bot.Classes;


namespace Telegram_Bot
{
    class Program
    {
        private static readonly TelegramBotClient Bot = new TelegramBotClient(ConfigurationManager.AppSettings["APIKey"]);
        private static CurrencyWrapper currencyWrapper = new CurrencyWrapper();

        static void Main(string[] args)
        {
            Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnMessageEdited += BotOnMessageReceived;
            Bot.OnReceiveError += BotOnReceiveError;

            var me = Bot.GetMeAsync().Result;

            Console.Title = me.Username;

            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }

        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Debugger.Break();
        }

        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;
            string currency;
            string priceMessage;

            Console.WriteLine("Received Message from {0}", message.Chat.Id);
            Console.WriteLine(message.Text);

            if (message == null || message.Type != MessageType.TextMessage) return;

            if (message.Text.StartsWith("/insultjohn"))
            {
                await Bot.SendTextMessageAsync(message.Chat.Id, "I wasn't born with enough middle fingers to let you know how I feel about John.");
            }
            else if (message.Text.StartsWith("/insultjason"))
            {
                await Bot.SendTextMessageAsync(message.Chat.Id, "Why would I do that? Jason is a big teddy bear. I fucking love teddy bears.");
            }
            else if (message.Text.StartsWith("/getprice"))
            {
                currency = message.Text.Remove(0, message.Text.IndexOf(' ') + 1);
                priceMessage = currencyWrapper.GetCurrencyPrice(currency.ToUpper());

                await Bot.SendTextMessageAsync(message.Chat.Id, priceMessage);
            }
            else
            {
                string usage = "Commands: \n/getprice <currency accronym> \n/insultjohn \n/insultjason";

                await Bot.SendTextMessageAsync(message.Chat.Id, usage);
            }

        }

        private static async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            await Bot.AnswerCallbackQueryAsync(callbackQueryEventArgs.CallbackQuery.Id,
                $"Received {callbackQueryEventArgs.CallbackQuery.Data}");
        }

    }

}
