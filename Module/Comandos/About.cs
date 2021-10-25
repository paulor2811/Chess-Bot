using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_Bot.Module.Comandos
{
    public class About : ModuleBase<SocketCommandContext>
    {
        [Command("about")]
        public async Task AboutMe()
        {
            await ReplyAsync("Nome: Chess Bot\nVersão: 1.0v\nDesenvolvedor: PauloR2811");
        }
    }
}
