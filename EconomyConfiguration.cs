using JetBrains.Annotations;
using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F.Economy
{
    public class EconomyConfiguration : IRocketPluginConfiguration
    {
        public bool DownloadLibraries;
        public bool XpMode;
        public int InitialMoney;
        public string CurrencyName;
        public bool MoneyUI;
        public ushort UIID;

        public void LoadDefaults()
        {
            DownloadLibraries = false;
            XpMode = false;
            InitialMoney = 10000;
            CurrencyName = "dolars";
            MoneyUI = true;
            UIID = 3624;
        }
    }
}
