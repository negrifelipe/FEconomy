using F.Economy.Models;
using LiteDB;
using Rocket.Unturned.Player;


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

		public static bool AccountExist(UnturnedPlayer player)
		{
			bool result;

			using (var db = new LiteDatabase($@"{System.Environment.CurrentDirectory}\Database\Economy.db", null))
			{
				var col = db.GetCollection<Account>("accounts");

				var account = col.FindById($"{player.CSteamID}");

				if (account != null)
				{
					result = true;
					Logger.Log("New account registered");
					return result;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		public static void PayTaxes(int money)
		{
			using (var db = new LiteDatabase($@"{System.Environment.CurrentDirectory}\Database\Economy.db", null))
			{
				var col = db.GetCollection<Account>("accounts");

				var cuentas = col.FindAll();

				foreach (Account account in cuentas)
				{
					if (money < account._money)
					{
						account._money -= money;
					}
					else
					{
						account._money = 0;
					}
					col.Update(account);
				}
			}
		}
	}
}
