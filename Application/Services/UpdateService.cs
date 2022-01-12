using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Application.Abstractions;
using Domain.Constants;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Application.Services;

public class UpdateService : IUpdateService
{
    private IBotService BotService { get; }
    private ILogger<UpdateService> Logger { get; }
    private IInlineCommandService InlineCommandService { get; }
    private static int NeedReplyCounter { get; set; } = new Random().Next(1, 20); 
    

    public UpdateService(IBotService botService, ILogger<UpdateService> logger,
        IInlineCommandService inlineCommandService)
    {
        Logger = logger;
        InlineCommandService = inlineCommandService;
        BotService = botService;
    }

    public Task EchoAsync(Update update)
    {
        return
#pragma warning disable CS8509
            update.Type switch
            {
                UpdateType.Message => HandleMessage(update.Message),
                // UpdateType.CallbackQuery => HandlingCallback(update.CallbackQuery),
                // _ => throw new ApplicationException($"Type '{update.Type}' not support")
            };
    }
    /*private async Task HandlingCallback(CallbackQuery callback)
    {
        var (commandId, chatId, value) = GetInfoFromCallBack(callback.Data);
        if (commandId == (byte) Commands.Repeat)
        {
            ChatOptions["repeat"] = value;
            // await _botService.Client.SendTextMessageAsync(chatId, $"success update state of repeat to {value}");
        }
    }*/

    private async Task HandleMessage(Message message)
    {
        if (!IsValid(message)) return;

        if (message.Text.StartsWith("/"))
            await HandleCommand(message);

        if (await HandleSimpleRegular(message))
            return;
        await HandleCountTriggers(message);
    }

    //todo: move to service
    private async Task HandleCountTriggers(Message message)
    {
        if (NeedReplyCounter == default)
        {
            await BotService.SendReplyMessageAsync(message.Chat.Id, message.MessageId, GetFunnyMessage());
            NeedReplyCounter = new Random().Next(1, 20);
            return;
        }

        NeedReplyCounter--;
    }

    private static string GetFunnyMessage()
    {
        var rnd = new Random().Next(0, FunnyMessages.Messages.Length);
        return FunnyMessages.Messages[rnd];
    }

    private async Task HandleCommand(Message message)
    {
        if (await InlineCommandService.IsCommand(message.Text.Split(' ').First()))
            await InlineCommandService.HandleCommand(message.Text, message.From.Id, message.Chat.Id);
    }

    //todo: move to service
    private async ValueTask<bool> HandleSimpleRegular(Message message)
    {
        //todo: move to constants
        if (Regex.IsMatch(message.Text, @"туп.*бот|бот.*туп|бот.*глуп|станя.*туп", RegexOptions.IgnoreCase))
        {
            await BotService.SendTextMessageAsync(message.Chat.Id, "Kiss my shiny metal arse!!!");
            return true;
        }

        if (Regex.IsMatch(message.Text,
                @"руслан.*знаешь|знаешь.*руслан|руслан.*ведь|дим.*ведь|андрей.*ведь|антон.*ведь",
                RegexOptions.IgnoreCase))
        {
            await BotService.SendStickerAsync(message.Chat.Id, Stickers.StickerKeys["Sticker_Ti_pidor"]);
            return true;
        }

        if (Regex.IsMatch(message.Text, @"красава.*бот|бот.*красава|cтаня.*красава", RegexOptions.IgnoreCase))
        {
            await BotService.SendTextMessageAsync(message.Chat.Id, "Спасибо, бро)))");
            await BotService.SendStickerAsync(message.Chat.Id, Stickers.StickerKeys["krasava"]);
            return true;
        }

        if (Regex.IsMatch(message.Text, @"ping", RegexOptions.IgnoreCase))
        {
            await BotService.SendTextMessageAsync(message.Chat.Id, "pong");
            return true;
        }

        if (Regex.IsMatch(message.Text, @"^нет", RegexOptions.IgnoreCase))
        {
            await BotService.SendTextMessageAsync(message.Chat.Id, "Пидора ответ");
            return true;
        }

        if (Regex.IsMatch(message.Text, @"^Всем привет|^Привет Станя|^Здорова Станя", RegexOptions.IgnoreCase))
        {
            await BotService.SendStickerAsync(message.Chat.Id, Stickers.StickerKeys["privet"]);
            return true;
        }

        if (Regex.IsMatch(message.Text, @"^Станя го бухать", RegexOptions.IgnoreCase))
        {
            await BotService.SendStickerAsync(message.Chat.Id, Stickers.StickerKeys["go_buxat"]);
            return true;
        }

        if (Regex.IsMatch(message.Text, @"^го бухать ребята", RegexOptions.IgnoreCase))
        {
            await BotService.SendStickerAsync(message.Chat.Id, Stickers.StickerKeys["buxat_student"]);
            return true;
        }

        if (Regex.IsMatch(message.Text, @"так точно", RegexOptions.IgnoreCase))
        {
            await BotService.SendStickerAsync(message.Chat.Id, Stickers.StickerKeys["capitan"]);
            return true;
        }

        if (Regex.IsMatch(message.Text, @"кофе", RegexOptions.IgnoreCase))
        {
            await BotService.SendStickerAsync(message.Chat.Id, Stickers.StickerKeys["pikachu_coffe"]);
            return true;
        }

        if (Regex.IsMatch(message.Text, @"где\sты|где\sвы", RegexOptions.IgnoreCase))
        {
            await BotService.SendStickerAsync(message.Chat.Id, Stickers.StickerKeys["gde_vse"]);
            return true;
        }

        return false;
    }

    private static bool IsValid(Message message)
    {
        if (message is not {Type: MessageType.Text}) return false;
        return message.Text is not null;
    }

    private InlineKeyboardButton[] GetInlineKeyboard(string taskName, string callBackData) =>
        new InlineKeyboardButton[]
        {
            InlineKeyboardButton.WithCallbackData(text: taskName, callbackData: callBackData)
        };

    private (byte command, long chatId, string value) GetInfoFromCallBack(string callBack)
    {
        const string pattern = @"^commandId=(?<commandId>-?\d+)&chatId=(?<chatId>-?\d+)&value=(?<value>-?.*)";

        var m = Regex.Match(callBack, pattern);

        if (m.Length > 0)
            if (byte.TryParse(m.Groups["commandId"].Value, out byte command)
                && long.TryParse(m.Groups["chatId"].Value, out long chatId)
                && m.Groups["value"].Value != null)
            {
                return (command, chatId, m.Groups["value"].Value);
            }

        throw new ApplicationException($"Wrong query callBack = {callBack}");
    }
}