using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace Chess_Bot
{
    public class Program
    {
        private
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        public DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        public async Task RunBotAsync()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _services = new ServiceCollection().AddSingleton(_client).AddSingleton(_commands).BuildServiceProvider();

            string tokenBot = "OTAxODY3MDM2MDM1MTI1MjY4.YXWHIQ.olzDYuAlJXZTw1QnYO-qGI8KB4c";

            //assinatura de evento

            _client.Ready += Client_Ready;
            _client.Log += ClientLog;
            _client.UserJoined += userJoined;

            await Client_Ready();
            await BotCommands();

            _client.LoginAsync(TokenType.Bot, tokenBot);

            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task userJoined(SocketGuildUser user)
        {
            var usuario = user.Guild;
        }

        private Task ClientLog(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        private async Task Client_Ready() //Responsável pelo status do bot
        {
            await _client.SetGameAsync(/*ação?*/"Xadrez", /*link da ação*/"https://www.chess.com/", ActivityType.Playing);
        }

        private async Task BotCommands() //Responsável pelos comandos do bot
        {
            _client.MessageReceived += IniciandoComandos;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task IniciandoComandos(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;

            if (message is null || message.Author.IsBot) return; //Tratando caso a mensagem seja fazia ou de um Bot

            var Context = new SocketCommandContext(_client, message);

            int argPost = 0;
            if(message.HasStringPrefix("/", ref argPost)) //Checando se a mensagem contém o prefixo de comando
            {
                var result = await _commands.ExecuteAsync(Context, argPost, _services);

                if(!result.IsSuccess)
                {
                    Console.WriteLine(result.ErrorReason);
                }
            }
        }
    }
}
