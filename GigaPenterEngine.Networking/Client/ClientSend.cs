using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigaPenterEngine.Networking.Client
{

    public static class ClientSend
    {
        public static void SendTCPData(Packet _packet)
        {
            _packet.WriteLength();
            Client.tcp.SendData(_packet);
        }

        public static void SendUDPData(Packet _packet)
        {
            _packet.WriteLength();
            Client.udp.SendData(_packet);
        }

        internal static void WelcomeReceived()
        {
            using (Packet _packet = new Packet(0))
            {
                _packet.Write(Client.myId);
                _packet.Write(Client.Username);

                SendTCPData(_packet);
            }
        }

        internal static void UDPTestReceived()
        {
            using (Packet _packet = new Packet(-1))
            {
                _packet.Write("Received a UDP packet.");

                SendUDPData(_packet);
            }
        }
    }

}
