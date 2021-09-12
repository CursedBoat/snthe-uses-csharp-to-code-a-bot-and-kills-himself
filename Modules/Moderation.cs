namespace DiscordBotTest.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using DiscordBotTest.Common;

    /// <summary>
    /// A class/module that contains commands only Moderators or Admins can access.
    /// </summary>
    public class Moderation : ModuleBase
    {
        /// <summary>
        /// A command that bans the user mentioned.
        /// </summary>
        /// <param name="banUser">The user to ban.</param>
        /// <param name="reason">Reason for banning the user.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Command("Ban")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task BanAsync(SocketGuildUser banUser, params string[] reason)
        {
            try
            {
                // Banning the user
                await banUser.BanAsync(0, string.Join(" ", reason), null);

                // Building DM embed
                var banUserDMEmbed = new BotEmbedBuilder()
                    .WithTitle("Hey, there")
                    .WithDescription($"You have been banned from **{this.Context.Guild.Name}** for: {string.Join(" ", reason)}")
                    .Build();

                // DMing the user
                string dmInfo = "and I sent them a DM notifying that they have been banned.";
                try
                {
                    await banUser.SendMessageAsync(embed: banUserDMEmbed);
                }
                catch
                {
                    dmInfo = "but I could not DM them";
                }

                // Building the embed
                var banSuccessEmbed = new BotEmbedBuilder()
                    .WithTitle($"Successfully banned {banUser.Username}#{banUser.Discriminator} {dmInfo}")
                    .Build();

                // Sending the embed
                await this.ReplyAsync(embed: banSuccessEmbed);
            }
            catch
            {
                // Building the embed
                var banFailEmbed = new BotEmbedBuilder()
                    .WithTitle($"Could not ban {banUser.Username}#{banUser.Discriminator}")
                    .AddField("Common reasons:", "Bot doesn't have required permissions", false)
                    .Build();

                // Sending the embed
                await this.ReplyAsync(embed: banFailEmbed);
            }
        }

        /// <summary>
        /// A command that kicks the user mentioned.
        /// </summary>
        /// <param name="kickUser">The user that is going to be kicked.</param>
        /// <param name="reason">The reason that the user is kicked.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Command("Kick")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task KickAsync(SocketGuildUser kickUser, params string[] reason)
        {
            try
            {
                // Kicking the user
                await kickUser.BanAsync(0, string.Join(" ", reason), null);

                // Building DM embed
                var kickUserDMEmbed = new BotEmbedBuilder()
                    .WithTitle("Hey, there")
                    .WithDescription($"You have been kicked from **{this.Context.Guild.Name}** for: {string.Join(" ", reason)}")
                    .Build();

                // DMing the user
                string dmInfo = "and I sent them a DM notifying that they have been kicked.";
                try
                {
                    await kickUser.SendMessageAsync(embed: kickUserDMEmbed);
                }
                catch
                {
                    dmInfo = "but I could not DM them";
                }

                // Building the embed
                var kickSuccessEmbed = new BotEmbedBuilder()
                    .WithTitle($"Successfully kicked {kickUser.Username}#{kickUser.Discriminator} {dmInfo}")
                    .Build();

                // Sending the embed
                await this.ReplyAsync(embed: kickSuccessEmbed);
            }
            catch
            {
                // Building the embed
                var kickFailEmbed = new BotEmbedBuilder()
                    .WithTitle($"Could not kick {kickUser.Username}#{kickUser.Discriminator}")
                    .AddField("Common reasons:", "Bot doesn't have required permissions", false)
                    .Build();

                // Sending the embed
                await this.ReplyAsync(embed: kickFailEmbed);
            }
        }
    }
}
