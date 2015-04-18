using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace TSDFF.IO
{
  /*  public static class NetIO
    {
        /// <summary>
        /// Permet d'envoyer des données par TCP-IP.
        /// </summary>
        /// <param name="datas">Les données a envoyer.</param>
        /// <param name="adress">L'adresse de destination.</param>
        /// <param name="DebugMode">(Par defaut désactivé) Permet de sortir dans le fichier Net.log.bin la sortie.</param>
        /// <param name="TimeOut">(Par defaut "-1") Permet de définir une limite de temps d'envoi.</param>>
        public static void SendData(Data datas, IPAddress adress, int port, int TimeOut = -1, bool DebugMode = false)
        {
            byte[] data = Data.BuildData(datas);
            TcpClient client = new TcpClient(new IPEndPoint(adress, port));

            client.SendBufferSize = 256; // Taille des packets.
            client.SendTimeout = TimeOut; // Limite de temps pour l'envoi.

            Ping TestPing = new Ping();
            PingReply pingreply = TestPing.Send(adress, 5000);
            if (pingreply.Status == IPStatus.Success)
            {
                if (DebugMode)
                {
                    Stream debugstream = new FileStream("Net.log.bin", FileMode.Append);
                }

                for (int i = 0; i < data.Length; i++)
                {
                    client.GetStream().WriteByte(data[i]); // Send the Data to the destination.
                }
            }
            else
            {
                throw new PingException("(NBI.NetIO.SendData) [Error] Ping has failled, cannot sent data.");
            }
        }
    

    }*/
}