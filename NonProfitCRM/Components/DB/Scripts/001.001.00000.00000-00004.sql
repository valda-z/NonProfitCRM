INSERT INTO [dbo].[EventTaskTemplate] ([Name], [Data]) VALUES (N'Prázdná', 
N'[
]');
INSERT INTO [dbo].[EventTaskTemplate] ([Name], [Data]) VALUES (N'Standard', 
N'[
  {"BDays":-5,"Description":"Potvrzení činností"},
  {"BDays":-3,"Description":"Pojišťovací seznamy"},
  {"BDays":-1,"Description":"Pojistit"},
  {"BDays":-1,"Description":"Poslední ověření, obědy, BOZP"},
  {"BDays":1,"Description":"Evaluační dotazníky a zpětná vazba neziskovky"},
  {"BDays":3,"Description":"Follow-up a evaluační dotazníky"},
  {"BDays":5,"Description":"Vyhodnocení evaluačních dotazníků"},
  {"BDays":7,"Description":"Fakturace"}
]');
INSERT INTO [dbo].[EventTaskTemplate] ([Name], [Data]) VALUES (N'Standard bez pojištění', 
N'[
  {"BDays":-5,"Description":"Potvrzení činností"},
  {"BDays":-1,"Description":"Poslední ověření, obědy, BOZP"},
  {"BDays":1,"Description":"Evaluační dotazníky a zpětná vazba neziskovky"},
  {"BDays":3,"Description":"Follow-up a evaluační dotazníky"},
  {"BDays":5,"Description":"Vyhodnocení evaluačních dotazníků"},
  {"BDays":7,"Description":"Fakturace"}
]');
