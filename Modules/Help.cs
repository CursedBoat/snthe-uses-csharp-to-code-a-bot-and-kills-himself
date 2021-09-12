namespace DiscordBotTest.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Discord.Commands;
    using DiscordBotTest.Common;

    /// <summary>
    /// The class/module which contains the help command.
    /// </summary>
    public class Help : ModuleBase
    {
        /// <summary>
        /// A command which contains the description of other.
        /// </summary>
        /// <param name="topic">Page number or command.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Command("Help")]
        [Alias("BotHelp")]
        public async Task HelpAsync(string topic = null)
        {
            if (topic == null)
            {
                var embedDefault = new BotEmbedBuilder()
                    .WithTitle("Help command")
                    .AddField("Page 1: General Commands", "``ping, info/userinfo, kill, report``", false)
                    .AddField("Page 2: Moderation Commands", "``ban, kick``", false)
                    .WithFooter("do t!help <page number>")
                    .Build();

                await this.ReplyAsync(embed: embedDefault);
            }
            else if (topic == "1")
            {
                var embedPage1 = new BotEmbedBuilder()
                    .WithTitle("Page 1")
                    .AddField("Ping", "Shows the latency of the bot ``usage: t!ping``", false)
                    .AddField("Info", "Shows the user info. Defaults to whoever called the command if user isn't provided ``usage: t!info t!info @example``", false)
                    .AddField("Kill", "'Kills' the user mentioned. ``usage: t!kill @example``", false)
                    .AddField("Report", "Ding ding di- I mean uhh use this if you find a problem with the bot. ``usage: t!report red's sus``", false)
                    .WithFooter("do t!help <page number>")
                    .Build();

                await this.ReplyAsync(embed: embedPage1);
            }
            else if (topic == "2")
            {
                var embedPage2 = new BotEmbedBuilder()
                    .WithTitle("Page 2")
                    .AddField("Ban", "Bans the user mentioned. ``usage: t!ban @example farting too much``", false)
                    .AddField("Kick", "Kicks the user mentioned. ``usage: t!kick @example dissed my favorite anime``", false)
                    .WithFooter("do t!help <page number>")
                    .Build();

                await this.ReplyAsync(embed: embedPage2);
            }
        }
    }
}