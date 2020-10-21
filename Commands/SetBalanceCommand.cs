using F.Economy.Database;
using Rocket.API;
using Rocket.Core.Logging;
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
    public class SetBalanceCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "setbalance";

        public string Help => string.Empty;

        public string Syntax => "/setbalance <player> <money>";

        public List<string> Aliases => new List<string>()
        {
            "setbal"
        };

        public List<string> Permissions => new List<string>()
        {
            "f.setbalance"
        };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (caller is ConsolePlayer)
            {
                UnturnedPlayer player2 = UnturnedPlayer.FromName(command[0]);
                int money = Convert.ToInt32(command[1]);

                if (player2 == null)
                {
                    Logger.Log(string.Format(Economy.Instance.Translate("player_find"), command[0]));
                    return;
                }
                if (money >= 0)
                {
                    if (Economy.Instance.Configuration.Instance.XpMode == false)
                    {
                        EconomyDB.SetBalance(player2, money);
                    }
                    else
                    {
                        player2.Experience = (uint)money;
                    }
                    Logger.Log(string.Format(Economy.Instance.Translate("setbalance_success"), player2.DisplayName, money));
                }
                else
                {
                    Logger.Log(Economy.Instance.Translate("err_ammount"));
                }
            }
            else
            {
                UnturnedPlayer player2 = UnturnedPlayer.FromName(command[0]);
                int money = Convert.ToInt32(command[1]);

                if (player2 == null)
                {
                    UnturnedChat.Say(caller, string.Format(Economy.Instance.Translate("player_find"), command[0]));
                    return;
                }
                if (money >= 0)
                {
                    if (Economy.Instance.Configuration.Instance.XpMode == false)
                    {
                        EconomyDB.SetBalance(player2, money);
                    }
                    else
                    {
                        player2.Experience = (uint)money;
                    }
                    UnturnedChat.Say(caller, string.Format(Economy.Instance.Translate("setbalance_success"), player2.DisplayName, money));
                    Logger.Log(string.Format(Economy.Instance.Translate("setbalance_success"), player2.DisplayName, money));
                }
                else
                {
                    UnturnedChat.Say(caller, Economy.Instance.Translate("err_ammount"));
                }
            }
        }
    }
}