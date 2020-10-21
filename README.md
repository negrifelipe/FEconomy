# FEconomy 
![Discord](https://img.shields.io/discord/742861338233274418?label=Discord&logo=Discord) [![Github All Releases](https://img.shields.io/github/downloads/01-Feli/FEconomy/total.svg)]()

FEconomy is an unturned economy plugin that uses a NoSQL document store database.

FPlugins Discord Support: https://discord.gg/4FF2548

This plugin will have a lot of updates with a lot of features like taxes and other stuff.

This plugin uses a workshop mod to display player balance. Mod: https://steamcommunity.com/sharedfiles/filedetails/?id=2256346633

# Commands
```
/pay <player> <ammount>
/mexange <ammount>
/exange <ammount>
/balance
/setbalance <player> <money>
/uir
/wipebal
```

# Configuration File
```
<?xml version="1.0" encoding="utf-8"?>
<EconomyConfiguration xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <DownloadLibraries>false</DownloadLibraries> This is for auto downloading libraries
  <XpMode>false</XpMode> Here you can set the economy mode to xp
  <Taxes>true</Taxes> Here you enable or disable taxes
  <DisconectedPlayersTaxes>2000</DisconectedPlayersTaxes> Here is the taxes ammount for disconected players this only work if DisconectedPlayersPayTaxes = true
  <DisconectedPlayersPayTaxes>false</DisconectedPlayersPayTaxes> Here you can enable taxes for disconected players
  <InitialMoney>10000</InitialMoney> Initial money :D
  <SalaryInterval>3060</SalaryInterval> Salary interval (Seconds)
  <TaxesInterval>432000</TaxesInterval> Taxes interval (Seconds)
  <CurrencyName>dolars</CurrencyName> Currency name
  <MoneyUI>true</MoneyUI> Enable or disable monney ui
  <UIID>3624</UIID>
  <Groups>
    <Group> This is the list of gruop that contains salarys and taxes of every group
      <GroupName>vip</GroupName>
      <Salary>10000</Salary>
      <Tax>2000</Tax>
    </Group>
  </Groups>
</EconomyConfiguration>
```
