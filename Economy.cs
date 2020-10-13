using F.Economy.Models;
using LiteDB;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using SDG.Unturned;
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
        protected override void Load()
        {
            Logger.Log("FEconomy Loaded");
            
            if (!File.Exists($@"{Rocket.Core.Environment.LibrariesDirectory}\LiteDB.dll"))
            {
                Logger.Log("Downloading Librarie LiteDB..", ConsoleColor.Yellow);
                using (var Client = new WebClient())
                {
                    Client.DownloadFile("https://cdn.discordapp.com/attachments/764924498364989481/764924576584695869/LiteDB.dll", $@"{Rocket.Core.Environment.LibrariesDirectory}\LiteDB.dll");
                }
                Logger.Log("LiteDB Downloaded!", ConsoleColor.Yellow);
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
        }

        private void Events_OnPlayerConnected(Rocket.Unturned.Player.UnturnedPlayer player)
        {
            Database.EconomyDB.NewAccount(player, Configuration.Instance.InitialMoney);
        }
    }
}
