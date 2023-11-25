using System.Reflection.Metadata;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Уведомлялка;

internal class Program
{
    private readonly static HttpClient _client = new();
    private static TelegramBotClient _botClient;
    private static List<string> _commandsAvailable = new()
    {
        "/start",
        "/getText",
        "/getVideo",
        "/getVoice"
    };

    private static List<string> _voiceMessages = new()
    {
        @"C:\Users\Евгений\Desktop\Всякое непотребство\Теория информационных процессов и систем\Телеграмм уведомления ПР 3\Voices\BITE.mp3",
        @"C:\Users\Евгений\Desktop\Всякое непотребство\Теория информационных процессов и систем\Телеграмм уведомления ПР 3\Voices\breath.mp3",
        @"C:\Users\Евгений\Desktop\Всякое непотребство\Теория информационных процессов и систем\Телеграмм уведомления ПР 3\Voices\bump.mp3",
        @"C:\Users\Евгений\Desktop\Всякое непотребство\Теория информационных процессов и систем\Телеграмм уведомления ПР 3\Voices\canBounce.mp3",
        @"C:\Users\Евгений\Desktop\Всякое непотребство\Теория информационных процессов и систем\Телеграмм уведомления ПР 3\Voices\canFall.mp3",
        @"C:\Users\Евгений\Desktop\Всякое непотребство\Теория информационных процессов и систем\Телеграмм уведомления ПР 3\Voices\canRoll.mp3",
        @"C:\Users\Евгений\Desktop\Всякое непотребство\Теория информационных процессов и систем\Телеграмм уведомления ПР 3\Voices\Crazy_Realistic_Knocking_Sound_Troll_Twitch_Streamers_256_kbps.mp3",
        @"C:\Users\Евгений\Desktop\Всякое непотребство\Теория информационных процессов и систем\Телеграмм уведомления ПР 3\Voices\crickets.mp3",
        @"C:\Users\Евгений\Desktop\Всякое непотребство\Теория информационных процессов и систем\Телеграмм уведомления ПР 3\Voices\fbi-open-up-meme-sound-effect.mp3",
        @"C:\Users\Евгений\Desktop\Всякое непотребство\Теория информационных процессов и систем\Телеграмм уведомления ПР 3\Voices\hey.mp3",
        @"C:\Users\Евгений\Desktop\Всякое непотребство\Теория информационных процессов и систем\Телеграмм уведомления ПР 3\Voices\Honk1.mp3",
        @"C:\Users\Евгений\Desktop\Всякое непотребство\Теория информационных процессов и систем\Телеграмм уведомления ПР 3\Voices\jog.mp3",
        @"C:\Users\Евгений\Desktop\Всякое непотребство\Теория информационных процессов и систем\Телеграмм уведомления ПР 3\Voices\jump.mp3",
        @"C:\Users\Евгений\Desktop\Всякое непотребство\Теория информационных процессов и систем\Телеграмм уведомления ПР 3\Voices\neon.mp3",
        @"C:\Users\Евгений\Desktop\Всякое непотребство\Теория информационных процессов и систем\Телеграмм уведомления ПР 3\Voices\table.mp3"
    };

    private static void Main(string[] args)
    {
        MainAsync().GetAwaiter().GetResult();
    }

    private static async Task MainAsync()
    {
        _botClient = new TelegramBotClient("6916526647:AAGvftDPQIogoh1Eltrx5YYsNC-ZsMwSojI", _client);

        await Logger.Info($"Logged in as {(await _botClient.GetMeAsync()).Username}");

        _botClient.StartReceiving(UpdateHandler, ErrorHandler);

        Console.ReadLine();
    }

    private static async Task ErrorHandler(ITelegramBotClient client, Exception exception, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    private static async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken token)
    {
        await (update.Type switch
        {
            UpdateType.Message => ProcedureMessageAsync(update),
            UpdateType.EditedMessage => ProcedureMessageAsync(update, true)
        });
    }

    private static async Task ProcedureMessageAsync(Update update, bool isEdited = false)
    {
        var message = isEdited ? update.EditedMessage : update.Message;

        await Logger.Info($"new message from {message.Chat.Username}: '{message.Text}'");

        if (message.Entities != null && message.Entities.Any())
            foreach (var entity in message.Entities)
                await ProcedureCommand(message, entity);
    }

    private static async Task ProcedureCommand(Message msg, MessageEntity entity)
    {
        if (entity.Type != MessageEntityType.BotCommand)
            return;

        var command = string.Join("", msg.Text.Skip(entity.Offset).Take(entity.Length));

        if (!_commandsAvailable.Contains(command))
        {
            await _botClient.SendTextMessageAsync(msg.Chat, "Incorrect command");
            return;
        }

        await (command switch
        {
            "/start" => _botClient.SendTextMessageAsync(msg.Chat, $"""
            Available commands:
            {string.Join(",\n", _commandsAvailable)}
            """),
            "/getText" => _botClient.SendTextMessageAsync(msg.Chat, "Какая-то сверх мега важная ифнормация"),
            "/getVideo" => SendVideoAsync(msg.Chat),
            "/getVoice" => SendVoiceAsync(msg.Chat)
        });
    }

    private static async Task SendVoiceAsync(Chat chat)
    {
        var rndNum = new Random().Next(0, _voiceMessages.Count - 1);
        var randomVoicePath = _voiceMessages[rndNum];
        using var fs = new FileStream(randomVoicePath, FileMode.Open);

        var inputFile = new InputFileStream(fs);
        await _botClient.SendVoiceAsync(chat, inputFile);
    }

    private static async Task SendVideoAsync(Chat chat)
    {
        using var fs = new FileStream(@"C:\Users\Евгений\Desktop\Всякое непотребство\Теория информационных процессов и систем\Телеграмм уведомления ПР 3\Videos\rick-roll-different-link-no-ads_(VIDEOMIN.NET).mp4", FileMode.Open);

        var inputFile = new InputFileStream(fs);
        await _botClient.SendVideoAsync(chat, inputFile);
    }
}