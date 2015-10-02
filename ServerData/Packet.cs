using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    [Serializable]
    public class Packet
    {
        public PacketType packetType;
        public String senderID;
        public List<String> stringData;

        public Packet(PacketType packetType, String senderID)
        {
            this.packetType = packetType;
            this.senderID = senderID;
            this.stringData = new List<string>();
        }

        public Packet(byte[] packetBytes)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(packetBytes);

            Packet p = (Packet)bf.Deserialize(ms);
            ms.Close();

            this.packetType = p.packetType;
            this.senderID = p.senderID;
            this.stringData = p.stringData;
        }

        public byte[] ToBytes()
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();

            bf.Serialize(ms,this);
            byte[] ret = ms.ToArray();
            ms.Close();

            return ret;
        }
    }

    /// <summary>
    /// Enum of all PacketTypes:
    /// Registration: [0] Client ID
    /// 
    /// Message:      [0] Message
    /// 
    /// Action:       [0] Number 
    ///               [1] X-coord 
    ///               [2] Y-coord
    /// 
    /// Movement:     [0] X-coord 
    ///               [1] Y-coord
    /// 
    /// GameState:    [0] GameStatusObject
    ///               [1] List of PlayerObjects (every element followed by '|')
    ///               [2] List of StaticObjects (every element followed by '|')
    ///               [3] List of DynamicObjects (every element followed by '|')
    /// 
    /// Ping:         Empty
    /// </summary>
    public enum PacketType
    {
        Registration,
        Message,
        Action,
        Movement,
        GameState,
        Ping
    }
}
