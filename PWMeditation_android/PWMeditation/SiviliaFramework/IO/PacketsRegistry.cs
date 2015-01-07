using System;
using System.Collections.Generic;
using System.Reflection;

namespace SiviliaFramework.IO
{
    public class PacketsRegistry
    {
        static Dictionary<uint, Type> ServerPacketDict = new Dictionary<uint, Type>();
        static Dictionary<uint, Type> ServerContainerDict = new Dictionary<uint, Type>();

        public static bool Registered { get; private set; }

        public static void RegisterPackets()
        {
            RegisterPackets(false);
        }
        /// <summary>
        /// Регистрация всех пакетов сборки
        /// </summary>
        /// <param name="update">Обновить реестр</param>
        public static void RegisterPackets(bool update)
        {
            if (!update && Registered) return;

            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type type in types)
            {
                RegisterPacket(type);
            }

            Registered = true;
        }
        /// <summary>
        /// Регистрирует пакет
        /// </summary>
        /// <param name="packetType">Тип пакета</param>
        public static void RegisterPacket(Type packetType)
        {
            if (!packetType.IsSubclassOf(typeof(GamePacket)) ||
                packetType == typeof(GamePacket) ||
                packetType == typeof(GamePacket<>)) return;

            object[] attributes = packetType.GetCustomAttributes(typeof(PacketIdentifier), false);
            if (attributes.Length == 0) throw new Exception("Unknown packet");

            PacketIdentifier packetId = attributes[0] as PacketIdentifier;


            if (packetId.PacketType == PacketType.ServerPacket)
            {
                if (ServerPacketDict.ContainsKey(packetId.PacketId))
                {
                    ServerPacketDict[packetId.PacketId] = packetType;
                    return;
                }
                ServerPacketDict.Add(packetId.PacketId, packetType);
            }
            if (packetId.PacketType == PacketType.ServerContainer)
            {
                if (ServerContainerDict.ContainsKey(packetId.PacketId))
                {
                    ServerContainerDict[packetId.PacketId] = packetType;
                    return;
                }

                ServerContainerDict.Add(packetId.PacketId, packetType);
            }
        }

        /// <summary>
        /// Возвращает тип для известного пакета
        /// </summary>
        /// <param name="type">Id пакета</param>
        /// <returns></returns>
        public static Type GetPacket(uint type)
        {
            if (ServerPacketDict.ContainsKey(type))
            {
                return ServerPacketDict[type];
            }
            return null;
        }
        /// <summary>
        /// Возвращат тип для известного контейнера
        /// </summary>
        /// <param name="type">Id контейнера</param>
        /// <returns></returns>
        public static Type GetContainer(uint type)
        {
            if (ServerContainerDict.ContainsKey(type))
            {
                return ServerContainerDict[type];
            }
            return null;
        }
        /// <summary>
        /// Возвращает идентификатор пакета
        /// </summary>
        /// <param name="type">Тип пакета</param>
        /// <returns></returns>
        public static PacketIdentifier GetPacketIdentifier(Type type)
        {
            string name = typeof(GamePacket).Name;
            if (type.GetInterface(name) == null) throw new Exception("Is no packet");

            object[] attributes = type.GetCustomAttributes(typeof(PacketIdentifier), false);
            if (attributes.Length == 0) throw new Exception("PacketIdentifier not found");

            return (attributes[0] as PacketIdentifier);

        }
    }
}
