using Rocket.API;
using F.Economy.Database;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F.Economy.Commands
{
    public class PayCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "pay";

        public string Help => null;

        public string Syntax => null;

        public List<string> Aliases => null;

        public List<string> Permissions => new List<string>
        {
            "pay"
        };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var player = (UnturnedPlayer)caller;

            EconomyDB.AddBalance(player, 10);
        }
    }
}
