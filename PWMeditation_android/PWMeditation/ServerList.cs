using System;
using System.Collections.Generic;
using SiviliaFramework.Network;

namespace PWMeditation
{
    public class ServerList
    {
        public static void InitializeServerList(List<GameServer> serverList)
        {
            serverList.Add(new GameServer("link1.pwonline.ru",  29000, "Орион"));
            serverList.Add(new GameServer("link2.pwonline.ru",  29000, "Вега"));
            serverList.Add(new GameServer("link3.pwonline.ru",  29000, "Сириус"));
            serverList.Add(new GameServer("link4.pwonline.ru",  29000, "Мира"));
            serverList.Add(new GameServer("link5.pwonline.ru",  29000, "Таразед"));
            serverList.Add(new GameServer("link6.pwonline.ru",  29000, "Альтаир"));
            serverList.Add(new GameServer("link7.pwonline.ru",  29000, "Гелиос"));
            serverList.Add(new GameServer("link9.pwonline.ru",  29000, "Пегас"));
            serverList.Add(new GameServer("link10.pwonline.ru", 29000, "Антарес"));
            serverList.Add(new GameServer("link12.pwonline.ru", 29000, "Кассиопея"));
            serverList.Add(new GameServer("link13.pwonline.ru", 29000, "Лиридан"));
            serverList.Add(new GameServer("link14.pwonline.ru", 29000, "Андромеда"));
            serverList.Add(new GameServer("link15.pwonline.ru", 29000, "Омега"));
            serverList.Add(new GameServer("link11.pwonline.ru", 29000, "Персей"));
        }
    }
}

