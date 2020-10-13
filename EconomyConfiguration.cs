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
        public bool XpMode;
        public int InitialMoney;
        public bool MoneyUI;
        public ushort UIID;

        public void LoadDefaults()
        {
            XpMode = false;
            InitialMoney = 10000;
            MoneyUI = true;
            UIID = 3624;
        }
    }
}
