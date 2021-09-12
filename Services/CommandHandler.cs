namespace DiscordBotTest.Services
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Addons.Hosting;
    using Discord.Commands;
    using Discord.WebSocket;
using DiscordBotTest.Common;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// The class that handles any commands.
    /// </summary>
    [Obsolete]
    public class CommandHandler : InitializedService
    {
        private readonly IServiceProvider provider;
        private readonly DiscordSocketClient client;
        private readonly CommandService service;
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandler"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="IServiceProvider"/> that should be injected.</param>
        /// <param name="client">The <see cref="DiscordSocketClient"/> that should be injected.</param>
        /// <param name="service">The <see cref="CommandService"/> that sould be injected.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> that should be injected.</param>
        public CommandHandler(IServiceProvider provider, DiscordSocketClient client, CommandService service, IConfiguration configuration)
        {
            this.provider = provider;
            this.client = client;
            this.service = service;
            this.configuration = configuration;
        }

        /// <inheritdoc/>
        public override async Task InitializeAsync(CancellationToken cancellationToken)
        {
            this.client.MessageReceived += this.OnMessageRecieved;
            this.service.CommandExecuted += this.OnCommandExecuted;
            this.client.Connected += this.OnBotReady;
            await this.service.AddModulesAsync(Assembly.GetEntryAssembly(), this.provider);
        }

        /// <summary>
        /// Events triggered when the bot is successfully connected to the Discord gateway.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task OnBotReady()
        {
            // Setting "do not disturb" status
            await this.client.SetStatusAsync(Discord.UserStatus.DoNotDisturb);

            // Setting "Watching Anime" status
            await this.client.SetGameAsync("Anime", null, ActivityType.Watching);
        }

        private async Task OnCommandExecuted(Optional<CommandInfo> info, ICommandContext context, IResult result)
        {
            if (result.IsSuccess)
            {
                return;
            }

            string errorResult = result.ErrorReason;

            if (errorResult == "The server responded with error 50007: Cannot send messages to this user")
            {
                errorResult = "Error recognized: The user has DMs turned off";
            }
            else if (errorResult == "Field value must not be null or empty. (Parameter 'Value')")
            {
                errorResult = "Error recognized: You must specify something. E.g t!userinfo @example";
            }

            var embedError = new EmbedBuilder()
                .WithTitle("Oops! An error occured")
                .WithDescription($"{errorResult}")
                .WithFooter("Please use the report command to notify the developer if the bot responds with an error code.")
                .WithColor(new Color(216, 22, 22))
                .WithCurrentTimestamp()
                .Build();

            await context.Channel.SendMessageAsync(embed: embedError);
        }

        private async Task OnMessageRecieved(SocketMessage socketMessage)
        {
            if (!(socketMessage is SocketUserMessage message))
            {
                return;
            }

            if (message.Source != Discord.MessageSource.User)
            {
                return;
            }

            var argPos = 0;
            if (!message.HasStringPrefix(this.configuration["Prefix"], ref argPos) && !message.HasMentionPrefix(this.client.CurrentUser, ref argPos))
            {
                return;
            }

            var ctx = new SocketCommandContext(this.client, message);
            await this.service.ExecuteAsync(ctx, argPos, this.provider);
        }
    }
}
