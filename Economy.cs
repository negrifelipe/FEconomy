using F.Economy.Database;
using F.Economy.Models;
using LiteDB;
using Rocket.API.Collections;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace F.Economy
{
    public class Economy : RocketPlugin<EconomyConfiguration>
    {
        public static Economy Instance;

        protected override void Load()
        {
            Instance = this;

            Logger.Log("FEconomy Loaded");

            if (Configuration.Instance.DownloadLibraries)
            {
                if (!File.Exists($@"{Rocket.Core.Environment.LibrariesDirectory}\LiteDB.dll"))
                {
                    Logger.Log("Downloading Librarie LiteDB..", ConsoleColor.Yellow);
                    using (var Client = new WebClient())
                    {
                        Client.DownloadFile("https://cdn.discordapp.com/attachments/764924498364989481/764924576584695869/LiteDB.dll", $@"{Rocket.Core.Environment.LibrariesDirectory}\LiteDB.dll");
                    }
                    Logger.Log("LiteDB Downloaded!", ConsoleColor.Yellow);
                }
            }
            
            var DirectoryD = $@"{System.Environment.CurrentDirectory}\Database";

            if (!System.IO.Directory.Exists(DirectoryD))
            {
                Logger.Log("Creating Directory Database..", ConsoleColor.Yellow);
                System.IO.Directory.CreateDirectory(DirectoryD);
                Logger.Log("Directory Database Created!", ConsoleColor.Yellow);
            }

            if (!File.Exists($@"{DirectoryD}\Economy.db"))
            {
                Logger.Log("Creating Economy Database..", ConsoleColor.Yellow);
                Database.EconomyDB.CreateDatabase();
                Logger.Log("Economy Database Created!", ConsoleColor.Yellow);
            }

            U.Events.OnPlayerConnected += Events_OnPlayerConnected;

            if (Configuration.Instance.MoneyUI == true)
            {
                Instance.OnBalanceUpdated += Instance_OnBalanceUpdated;
            }
        }

        public delegate void PlayerBalanceUpdated(UnturnedPlayer player, int money);
        public event PlayerBalanceUpdated OnBalanceUpdated;

        private void Instance_OnBalanceUpdated(UnturnedPlayer player, int money)
        {
            EffectManager.sendUIEffectText(5456, player.CSteamID, true, "Dinero", $"${EconomyDB.GetBalance(player)}");
        }

        public void BalanceUpdate(CSteamID cSteamID, int money)
        {
            if (OnBalanceUpdated != null)
            {
                UnturnedPlayer player = UnturnedPlayer.FromCSteamID(cSteamID);
                OnBalanceUpdated(player, money);
            }
        }

        private void Events_OnPlayerConnected(UnturnedPlayer player)
        {
            Database.EconomyDB.NewAccount(player, Configuration.Instance.InitialMoney);

            if (Configuration.Instance.MoneyUI == true) 
            { 
                EffectManager.sendUIEffect(Configuration.Instance.UIID, 5456, player.CSteamID, true);
                EffectManager.sendUIEffectText(5456, player.CSteamID, true, "Dinero", $"${EconomyDB.GetBalance(player)}");
            }
        }


        public override TranslationList DefaultTranslations
        {
            get
            {
                TranslationList translationList = new TranslationList();
                translationList.Add("pay_nopoint", "There is no point in paying yourself!");
                translationList.Add("err_ammount", "The ammount of money must be positive!");
                translationList.Add("no_balance", "You don't have enough money!");
                translationList.Add("pay_success", "You paid {0} {1} to {2}!");
                translationList.Add("pay_recieve", "You received {0} {1} from {2}!");
                translationList.Add("mexange_success", "Successful exanged {0} {1} to: {2} Xp!");
                return translationList;
            }
        }
    }
}
