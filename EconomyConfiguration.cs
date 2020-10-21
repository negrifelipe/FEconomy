using F.Economy.Models;
using Rocket.API;
using System.Collections.Generic;

namespace F.Economy
{
    public class EconomyConfiguration : IRocketPluginConfiguration
    {
        public bool DownloadLibraries;
        public bool XpMode;
        public bool Taxes;
        public int DisconectedPlayersTaxes;
        public bool DisconectedPlayersPayTaxes;
        public int InitialMoney;
        public int SalaryInterval;
        public int TaxesInterval;
        public string CurrencyName;
        public bool MoneyUI;
        public ushort UIID;
        public List<Group> Groups;

        public void LoadDefaults()
        {
            DownloadLibraries = false;
            XpMode = false;
            Taxes = true;
            DisconectedPlayersPayTaxes = false;
            DisconectedPlayersTaxes = 2000;
            InitialMoney = 10000;
            SalaryInterval = 3060;
            TaxesInterval = 432000;
            CurrencyName = "dolars";
            MoneyUI = true;
            UIID = 3624;
            Groups = new List<Group>()
            {
                new Group { GroupName = "vip", Salary = 10000, Tax = 2000 }
            };
        }
    }
}
