namespace DiscordBotTest.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;

// Disabling warnings because it's a test file.
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1101 // Prefix local calls with this
#pragma warning disable SA1507 // Whitespace
    public class Test : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        [RequireOwner]
        public async Task PingPong()
        {
            await ReplyAsync("Pong");
        }


        [Command("loop")]
        [RequireOwner]
        public async Task Looper()
        {
            while (true)
            {
                await ReplyAsync("Pooper!");
            }
        }

        [Command("dm")]
        [RequireOwner]
        public async Task Dmhaha(SocketUser user)
        {
            await user.SendMessageAsync("Dumbass");
        }

        [Command("dumbassery")]
        [RequireOwner]
        public async Task Dumbass(SocketUser user)
        {
            await ReplyAsync("I will now spam dm the user :thumbsup:");

            // Infinite loop
            while (true)
            {
                await user.SendMessageAsync("haha child");
            }
        }
    }
}
