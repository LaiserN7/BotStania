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

    public UpdateService(IBotService botService, ILogger<UpdateService> logger)
    {
        Logger = logger;
        BotService = botService;
    }

    public Task EchoAsync(Update update, CancellationToken cancellationToken)
        =>
            update.Type switch
            {
                UpdateType.Message => HandleMessage(update.Message, cancellationToken),
                // UpdateType.CallbackQuery => HandlingCallback(update.CallbackQuery),
                // _ => throw new ApplicationException($"Type '{update.Type}' not support")
            };

    /*private async Task HandlingCallback(CallbackQuery callback)
    {
        var (commandId, chatId, value) = GetInfoFromCallBack(callback.Data);
        if (commandId == (byte) Commands.Repeat)
        {
            ChatOptions["repeat"] = value;
            // await _botService.Client.SendTextMessageAsync(chatId, $"success update state of repeat to {value}");
        }
    }*/
    
    private async Task HandleMessage(Message message, CancellationToken cancellationToken)
    {
        if (!IsValid(message)) return;

        if (Regex.IsMatch(message.Text, @"туп.*бот|бот.*туп|бот.*глуп", RegexOptions.IgnoreCase))
        {
            await BotService.SendTextMessageAsync(message.Chat.Id, "Kiss my shiny metal arse!!!", cancellationToken: cancellationToken);
            return;
        }

        if (Regex.IsMatch(message.Text,
                @"руслан.*знаешь|знаешь.*руслан|руслан.*ведь|дим.*ведь|андрей.*ведь|антон.*ведь",
                RegexOptions.IgnoreCase))
        {
            await BotService.SendStickerAsync(message.Chat.Id, Stickers.StickerKeys["Sticker_Ti_pidor"], cancellationToken: cancellationToken);
            return;
        }

        if (Regex.IsMatch(message.Text, @"красава.*бот|бот.*красава|cтаня.*красава", RegexOptions.IgnoreCase))
        {
            await BotService.SendTextMessageAsync(message.Chat.Id, "Спасибо, бро)))");
            await BotService.SendStickerAsync(message.Chat.Id, Stickers.StickerKeys["krasava"]);
            return;
        }

        if (Regex.IsMatch(message.Text, @"ping", RegexOptions.IgnoreCase))
        {
            await BotService.SendTextMessageAsync(message.Chat.Id, "pong");
            return;
        }

        if (Regex.IsMatch(message.Text, @"^нет", RegexOptions.IgnoreCase))
        {
            await BotService.SendTextMessageAsync(message.Chat.Id, "Пидора ответ");
            return;
        }

        if (Regex.IsMatch(message.Text, @"^Всем привет|^Привет Станя|^Здорова Станя", RegexOptions.IgnoreCase))
        {
            await BotService.SendStickerAsync(message.Chat.Id, Stickers.StickerKeys["privet"]);
            return;
        }

        if (Regex.IsMatch(message.Text, @"^Станя го бухать", RegexOptions.IgnoreCase))
        {
            await BotService.SendStickerAsync(message.Chat.Id, Stickers.StickerKeys["go_buxat"]);
            return;
        }

        if (Regex.IsMatch(message.Text, @"^го бухать ребята", RegexOptions.IgnoreCase))
        {
            await BotService.SendStickerAsync(message.Chat.Id, Stickers.StickerKeys["buxat_student"]);
            return;
        }

        if (Regex.IsMatch(message.Text, @"так точно", RegexOptions.IgnoreCase))
        {
            await BotService.SendStickerAsync(message.Chat.Id, Stickers.StickerKeys["capitan"]);
            return;
        }

        if (Regex.IsMatch(message.Text, @"кофе", RegexOptions.IgnoreCase))
        {
            await BotService.SendStickerAsync(message.Chat.Id, Stickers.StickerKeys["pikachu_coffe"]);
            return;
        }

        if (Regex.IsMatch(message.Text, @"где\sты|где\sвы", RegexOptions.IgnoreCase))
        {
            await BotService.SendStickerAsync(message.Chat.Id, Stickers.StickerKeys["gde_vse"]);
            return;
        }
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