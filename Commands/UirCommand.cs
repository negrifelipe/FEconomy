using F.Economy.Database;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F.Economy.Commands
{
    public class UirCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "uir";

        public string Help => string.Empty;

        public string Syntax => string.Empty;

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>()
        {
            "f.uir"
        };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var uplayer = (UnturnedPlayer)caller;
            if (Economy.Instance.Configuration.Instance.MoneyUI == true)
            {
                if (Economy.Instance.Configuration.Instance.XpMode == false)
                {
                    EffectManager.sendUIEffectText(5456, uplayer.CSteamID, true, "Dinero", $"${EconomyDB.GetBalance(uplayer)}");
                }
                else
                {
                    EffectManager.sendUIEffectText(5456, uplayer.CSteamID, true, "Dinero", $"${uplayer.Experience}");
                }
            }
        }
    }
}

