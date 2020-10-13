using F.Economy.Models;
using LiteDB;
using Rocket.Core;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;


namespace F.Economy.Database
{
    public class EconomyDB
    {
		public static void CreateDatabase()
		{
			using (var db = new LiteDatabase($@"{System.Environment.CurrentDirectory}\Database\Economy.db", null))
			{
				var col = db.GetCollection<Account>("accounts");
			}
		}

		public static void NewAccount(UnturnedPlayer player, int money)
		{
			using (var db = new LiteDatabase($@"{System.Environment.CurrentDirectory}\Database\Economy.db", null))
			{
				var col = db.GetCollection<Account>("accounts");

				var Cuenta = new Account
				{
					_id = $"{player.CSteamID}",
					_money = money
				};

				bool flag = col.FindById($"{player.CSteamID}") == null;

				if (flag)
				{
					col.Insert(Cuenta);
				}
			}
		}

		public static void AddBalance(UnturnedPlayer player, int money)
		{
			int pmoney = GetBalance(player);

			using (var db = new LiteDatabase($@"{System.Environment.CurrentDirectory}\Database\Economy.db", null))
			{
				var col = db.GetCollection<Account>("accounts");


				var Cuenta = new Account
				{
					_id = $"{player.CSteamID}",
					_money = pmoney + money
				};

				col.Update(Cuenta);

			}
			Economy.Instance.BalanceUpdate(player.CSteamID, money);
		}

		public static void RemoveBalance(UnturnedPlayer player, int money)
		{
			int pmoney = GetBalance(player);

			using (var db = new LiteDatabase($@"{System.Environment.CurrentDirectory}\Database\Economy.db", null))
			{
				var col = db.GetCollection<Account>("accounts");


				var Cuenta = new Account
				{
					_id = $"{player.CSteamID}",
					_money = pmoney - money
				};

				col.Update(Cuenta);
			}
			Economy.Instance.BalanceUpdate(player.CSteamID, money);
		}

		public static void SetBalance(UnturnedPlayer player, int money)
		{
			using (var db = new LiteDatabase($@"{System.Environment.CurrentDirectory}\Database\Economy.db", null))
			{
				var col = db.GetCollection<Account>("accounts");

				var Cuenta = new Account
				{
					_id = $"{player.CSteamID}",
					_money = money
				};

				col.Update(Cuenta);
			}

			Economy.Instance.BalanceUpdate(player.CSteamID, money);
		}

		public static void WipeAccounts()
		{
			using (var db = new LiteDatabase($@"{System.Environment.CurrentDirectory}\Database\Economy.db", null))
			{
				var col = db.GetCollection<Account>("accounts");

				col.DeleteAll();
			}
		}

		public static int GetBalance(UnturnedPlayer player)
		{
			int result;
			using (var db = new LiteDatabase($@"{System.Environment.CurrentDirectory}\Database\Economy.db", null))
			{
				var col = db.GetCollection<Account>("accounts");

				var dinero = col.FindById($"{player.CSteamID}");

				result = dinero._money;
			}
			return result;
		}
	}
}
