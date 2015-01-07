using System;
using System.Collections.Generic;
using System.Text;
using OOGLibrary.IO.PacketBase.Server;

namespace OOGLibrary.Network.Templates
{
    delegate void OnPlayersLoadedEventHandler(object sender, OnPlayersLoadedEventArgs e);
    class OnPlayersLoadedEventArgs
    {
        List<RoleList_ReS53> players = new List<RoleList_ReS53>();
        public RoleList_ReS53 this[int index]
        {
            get
            {
                return players[index];
            }
        }
        public int Count
        {
            get
            {
                return players.Count;
            }
        }
        internal void Add(RoleList_ReS53 role)
        {
            players.Add(role);
        }
    }
}
