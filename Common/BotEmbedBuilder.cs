namespace DiscordBotTest.Common
{
    using Discord;

    /// <summary>
    /// Custom embed builder class for the bot.
    /// </summary>
    internal class BotEmbedBuilder : EmbedBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BotEmbedBuilder"/> class.
        /// </summary>
        public BotEmbedBuilder()
        {
            // Setting a default color
            this.WithColor(new Color(134, 14, 220));
        }
    }
}
