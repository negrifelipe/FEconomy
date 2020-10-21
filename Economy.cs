using F.Economy.Database;
using F.Economy.Models;
using Rocket.API.Collections;
using Rocket.Core;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

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
                if (Configuration.Instance.XpMode == false)
                {
                    Instance.OnBalanceUpdated += Instance_OnBalanceUpdated;
                }
                else
                {
                    UnturnedPlayerEvents.OnPlayerUpdateExperience += UnturnedPlayerEvents_OnPlayerUpdateExperience;
                }
            }

            StartCoroutine((IEnumerator)Salary());
            StartCoroutine((IEnumerator)Taxes());
        }

        private void UnturnedPlayerEvents_OnPlayerUpdateExperience(UnturnedPlayer player, uint experience)
        {
            EffectManager.sendUIEffectText(5456, player.CSteamID, true, "Dinero", $"${player.Experience}");
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
            EconomyDB.NewAccount(player, Configuration.Instance.InitialMoney);
            if (EconomyDB.AccountExist(player) == false && Configuration.Instance.XpMode == true)
            {
                player.Experience = player.Experience + (uint)Configuration.Instance.InitialMoney;
            }
            if (Configuration.Instance.MoneyUI == true)
            {
                EffectManager.sendUIEffect(Configuration.Instance.UIID, 5456, player.CSteamID, true);
                if (Configuration.Instance.XpMode == false)
                {
                    EffectManager.sendUIEffectText(5456, player.CSteamID, true, "Dinero", $"${EconomyDB.GetBalance(player)}");
                }
                else
                {
                    EffectManager.sendUIEffectText(5456, player.CSteamID, true, "Dinero", $"${player.Experience}");
                }
            }
        }

        private IEnumerator<WaitForSeconds> Salary()
        {
            for (; ; )
            {
                PaySalary();
                yield return new WaitForSeconds((float)Configuration.Instance.SalaryInterval);
            }
        }

        private IEnumerator<WaitForSeconds> Taxes()
        {
            for (; ; )
            {
                PayTaxes();
                yield return new WaitForSeconds((float)Configuration.Instance.TaxesInterval);
            }
        }

        private void PayTaxes()
        {
            if (Configuration.Instance.DisconectedPlayersPayTaxes == true)
            {
                if (Configuration.Instance.XpMode == true)
                {
                    EconomyDB.PayTaxes(Configuration.Instance.DisconectedPlayersTaxes);
                }
            }
            else
            {
                foreach (SteamPlayer steamPlayer in Provider.clients)
                {
                    UnturnedPlayer player = UnturnedPlayer.FromSteamPlayer(steamPlayer);
                    int amount = 0;

                    foreach (Group group in Configuration.Instance.Groups)
                    {
                        var salarygroup = R.Permissions.GetGroup(group.GroupName);
                        if (salarygroup.Members.Contains(player.Id))
                        {
                            amount = group.Tax;
                            if (amount < 0)
                            {
                                amount = 0;
                            }
                            if (Configuration.Instance.XpMode == false)
                            {
                                EconomyDB.RemoveBalance(player, amount);
                            }
                            else
                            {
                                player.Experience = player.Experience - (uint)amount;
                            }
                            UnturnedChat.Say(player, string.Format(Translate("tax_pay"), amount, Configuration.Instance.CurrencyName, salarygroup.DisplayName));
                        }
                    }
                }
            }
        }

        private void PaySalary()
        {
            foreach (SteamPlayer steamPlayer in Provider.clients)
            {
                UnturnedPlayer player = UnturnedPlayer.FromSteamPlayer(steamPlayer);
                int amount = 0;

                foreach (Group group in Configuration.Instance.Groups)
                {
                    var salarygroup = R.Permissions.GetGroup(group.GroupName);
                    if (salarygroup.Members.Contains(player.Id))
                    {
                        amount = group.Salary;
                        if (amount < 0)
                        {
                            amount = 0;
                        }
                        if (Configuration.Instance.XpMode == false)
                        {
                            EconomyDB.AddBalance(player, amount);
                        }
                        else
                        {
                            player.Experience = player.Experience + (uint)amount;
                        }
                        UnturnedChat.Say(player, string.Format(Translate("salary_pay"), amount, Configuration.Instance.CurrencyName, salarygroup.DisplayName));
                    }
                }
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
                translationList.Add("c_pay_success", "{3} paid {0} {1} to {2}!");
                translationList.Add("xppay_success", "You paid {0} to {1}!");
                translationList.Add("pay_recieve", "You received {0} {1} from {2}!");
                translationList.Add("xppay_recieve", "You received {0} from {1}!");
                translationList.Add("mexange_success", "Successfuly exanged {0} {1} to: {2} Xp!");
                translationList.Add("exange_success", "Successfuly exanged {0} experience to: {1} {2}!");
                translationList.Add("xp_enabled", "Xp mode is enabled so you can't use this command!");
                translationList.Add("wipe_success", "Successfuly wiped all player balace!");
                translationList.Add("balance", "Your balance: {0}!");
                translationList.Add("salary_pay", "You received {0} {1} as salary of {2}!");
                translationList.Add("tax_pay", "You pay {0} {1} of {2} taxes!");
                translationList.Add("player_find", "Failed to find a player called: {0}!");
                translationList.Add("setbalance_success", "Successfuly set {0} balance to {1}!");
                return translationList;
            }
        }

        protected override void Unload()
        {
            StopAllCoroutines();
            U.Events.OnPlayerConnected += Events_OnPlayerConnected;

            if (Configuration.Instance.MoneyUI == true)
            {
                if (Configuration.Instance.XpMode == false)
                {
                    Instance.OnBalanceUpdated -= Instance_OnBalanceUpdated;
                }
                else
                {
                    UnturnedPlayerEvents.OnPlayerUpdateExperience -= UnturnedPlayerEvents_OnPlayerUpdateExperience;
                }
            }
        }
    }
}

