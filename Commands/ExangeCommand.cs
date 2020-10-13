using F.Economy.Database;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F.Economy.Commands
{
    public class ExangeCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "exange";

        public string Help => string.Empty;

        public string Syntax => "/exange <ammount>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>()
        {
            "f.exange"
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
                        if (money < player.Experience + 1)
                        {
                            EconomyDB.AddBalance(player, money);
                            player.Experience = player.Experience - (uint)money;
                            UnturnedChat.Say(caller, string.Format(Economy.Instance.Translate("exange_success"), money, money,Economy.Instance.Configuration.Instance.CurrencyName));
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
