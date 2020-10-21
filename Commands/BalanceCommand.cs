using F.Economy.Database;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F.Economy.Commands
{
    public class BalanceCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "balance";

        public string Help => string.Empty;

        public string Syntax => "/balance";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>()
        {
            "f.balance"
        };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromName(caller.DisplayName);

            if (Economy.Instance.Configuration.Instance.XpMode == false)
            {
                UnturnedChat.Say(caller, string.Format(Economy.Instance.Translate("balance"), EconomyDB.GetBalance(unturnedPlayer)));
            }
            else
            {
                UnturnedChat.Say(caller, string.Format(Economy.Instance.Translate("balance"), unturnedPlayer.Experience.ToString()));
            }

        }
    }
}
