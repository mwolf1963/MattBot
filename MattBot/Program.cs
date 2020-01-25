using System;
using System.ComponentModel.Design;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace MattBot
{
    class Program
    {
        private CommandService commands;
        private IServiceProvider services;
        private DiscordSocketClient client;

        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            client = new DiscordSocketClient();
            
            client.MessageReceived += HandleCommand;

            client.Log += Log;
            commands = new CommandService();
            
            //add all these to services

            services = new ServiceCollection().AddSingleton(this).AddSingleton(client).AddSingleton(commands).AddSingleton<ConfigHandler>().BuildServiceProvider();
            await services.GetService<ConfigHandler>().PopulateConfig();
            

            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);

            await client.LoginAsync(TokenType.Bot, services.GetService<ConfigHandler>().GetToken());
            await client.StartAsync();
            //block the thread till we have got a connection
            while (client.ConnectionState != ConnectionState.Connected)
            {
                
            }
            // Block this task until the program is closed.
            ClassReminder reminder = new ClassReminder(client);
            while (true)
            {
                await reminder.CheckReminder();
            }
            
        }


        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        public  async Task HandleCommand(SocketMessage messageParam)
        {
            //Console.WriteLine(messageParam.Channel);
            //Console.WriteLine(messageParam.Application);
            //Console.WriteLine(messageParam.Activity);
            //Console.WriteLine(messageParam.Author);
            //Console.WriteLine(messageParam.EditedTimestamp);


            var message = messageParam as SocketUserMessage;
            if (message == null) return;
            int argPos = 0;
            if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos))) return;
            var context = new SocketCommandContext(client, message);
            
            var result = await commands.ExecuteAsync(context, argPos, services);
            if (!result.IsSuccess)
            {
                await context.Channel.SendFileAsync(result.ErrorReason);
            }
        }

        
    }
    
}
