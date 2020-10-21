using F.Economy.Database;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;

namespace F.Economy.Commands
{
    public class MoneyExangeCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "moneyexange";

        public string Help => string.Empty;

        public string Syntax => "/mexange <ammount>";

        public List<string> Aliases => new List<string>()
        {
            "mexange"
        };

        public List<string> Permissions => new List<string>()
        {
            "f.moneyexange"
        };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var player = (UnturnedPlayer)caller;
            int money = Convert.ToInt32(command[0]);
            switch (command.Length)
            {
                case 1:
                    if (money > 0)
                    {
                        if (money < EconomyDB.GetBalance(player) + 1)
                        {
                            EconomyDB.RemoveBalance(player, money);
                            player.Experience = player.Experience + (uint)money;
                            UnturnedChat.Say(caller, string.Format(Economy.Instance.Translate("mexange_success"), money, Economy.Instance.Configuration.Instance.CurrencyName, money));
                        }
                        else
                        {
                            UnturnedChat.Say(caller, Economy.Instance.Translate("no_balance"));
                        }
                    }
                    else
                    {
                        UnturnedChat.Say(caller, Economy.Instance.Translate("err_ammount"));
                    }
                    break;
                default:
                    UnturnedChat.Say(caller, Syntax);
                    break;
            }
        }
    }
}
