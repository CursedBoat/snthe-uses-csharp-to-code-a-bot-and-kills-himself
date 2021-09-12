namespace DiscordBotTest.Modules
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Channels;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using DiscordBotTest.Common;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Start of the "General" module, where some general bot commands are stored such as kill, ping etc.
    /// </summary>
    public class General : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// A command that replies with the latency of the bot.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Command("Ping")]
        [Alias("latency")]
        public async Task PingPong()
        {
            // Creating the embed
            var embed = new EmbedBuilder()
                .WithTitle($"{this.Context.Client.CurrentUser.Username}#{this.Context.Client.CurrentUser.Discriminator}")
                .WithDescription($"The bot latency is: **{this.Context.Client.Latency}ms**")
                .Build();

            // Sending the embed
            await this.Context.Channel.SendMessageAsync(embed: embed);
        }

        /// <summary>
        /// A command that gets the information about the author or a user mentioned.
        /// </summary>
        /// <param name="discordMember">(Optional) A discord user to get information from.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Command("Info")]
        [Alias("UserInfo", "GetUserInfo")]
        public async Task GetUserInfoAsync(SocketGuildUser discordMember = null)
        {
            // If no users are mentioned, the author will be used instead
            if (discordMember == null)
            {
                discordMember = this.Context.Message.Author as SocketGuildUser;
            }

            // Getting the user's profile picture
            var thumbNail = discordMember.GetAvatarUrl() ?? discordMember.GetDefaultAvatarUrl();

            // Creating the embed
            var embedUserInfo = new BotEmbedBuilder()
                .WithTitle($"User Info for {discordMember.Username}#{discordMember.Discriminator}")
                .WithCurrentTimestamp()
                .WithThumbnailUrl(thumbNail)
                .AddField("ID", discordMember.Id, false)
                .AddField("Created at", discordMember.CreatedAt, false)
                .Build();

            // Sending the embed
            await this.ReplyAsync(embed: embedUserInfo);
        }

        /// <summary>
        /// A fun command where users "kill" each other.
        /// </summary>
        /// <param name="discordMember">(Optional) a discord member to kill</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Command("Kill")]
        public async Task KillUserAsync(SocketGuildUser discordMember = null)
        {
            if (discordMember == null)
            {
                discordMember = this.Context.Message.Author as SocketGuildUser;
            }

            // Array of things the user can "die" from
            string[] killUserText = {"from dancing too much", "from smoking carrots", "while trying to speak to ur mom", "while having sex :thumbsup:"};

            // Picking a random string from the array
            Random random = new Random();
            int value = random.Next(0, killUserText.Length);

            // The embed
            var embedKillUser = new BotEmbedBuilder()
                .WithTitle($"{discordMember.Username} died ;-;")
                .WithDescription($"{discordMember.Username} died {killUserText[value]}")
                .Build();

            // Sending the embed
            await this.ReplyAsync(embed: embedKillUser);
        }

        /// <summary>
        /// A command for users to report if the bot is not functioning properly.
        /// </summary>
        /// <param name="reason">The reason that the user inputs.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Command("Report")]
        public async Task ReportAsync(params string[] reason)
        {
            // Joining the strings
            var reasonUser = string.Join(" ", reason);

            // Getting the channel ID to send the message
            var client = this.Context.Client;
            ulong id = 886504481930510376;
            var reportMessageChannel = client.GetChannel(id) as IMessageChannel;

            // Creating the embed
            var reportSuccessEmbed = new BotEmbedBuilder()
                .WithTitle("New report!")
                .WithDescription($"The report was sent by {this.Context.Message.Author.Username}#{this.Context.Message.Author.Discriminator}")
                .AddField("Report:", reasonUser)
                .Build();

            // Sending the message
            await reportMessageChannel.SendMessageAsync(embed: reportSuccessEmbed);
        }
    }
}
