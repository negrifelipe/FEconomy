using F.Economy.Database;
using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;
using System.Collections.Generic;

namespace F.Economy.Commands
{
    public class WipeBalanceCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "wipebalance";

        public string Help => string.Empty;

        public string Syntax => "/wipebal";

        public List<string> Aliases => new List<string>()
        {
            "wipebal"
        };

        public List<string> Permissions => new List<string>()
        {
            "f.wipebalance"
        };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            EconomyDB.WipeAccounts();
            if (caller is ConsolePlayer)
            {
                Logger.Log(Economy.Instance.Translate("wipe_success"));
            }
            else
            {
                UnturnedChat.Say(caller, Economy.Instance.Translate("wipe_success"));
                Logger.Log(Economy.Instance.Translate("wipe_success"));
            }
        }
    }
}