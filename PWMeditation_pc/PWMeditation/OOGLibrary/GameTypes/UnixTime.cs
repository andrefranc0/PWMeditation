using System;
using OOGLibrary.IO;

namespace OOGLibrary.GameTypes
{
    /// <summary>
    /// Представляет реализацию Unix-времени.
    /// </summary>
    class UnixTime : ICloneable
    {
        /// <summary>
        /// Возвращает текущее время в формате Unix.
        /// </summary>
        /// <returns></returns>
        public static Int32 GetUnixTime()
        {
            return ToUnixTime(DateTime.Now);
        }

        /// <summary>
        /// Преобразует DateTime в Unix таймстемп.
        /// </summary>
        /// <param name="dateTime">DateTime</param>
        /// <returns></returns>
        public static Int32 ToUnixTime(DateTime dateTime)
        {
            return (int)(dateTime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        }

        /// <summary>
        /// Преобразует Unix таймстемп в DateTime.
        /// </summary>
        /// <param name="timeStamp">Таймстемп</param>
        /// <returns></returns>
        public static DateTime ToDateTime(Int32 timeStamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(timeStamp);
        }

        /// <summary>
        /// Дата/Время.
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Таймстемп.
        /// </summary>
        public Int32 Timestamp
        {
            get
            {
                return ToUnixTime(Time);
            }
            set
            {
                Time = ToDateTime(value);
            }
        }

        /// <summary>
        /// Базовая инициализация. Время присваивается текущее.
        /// </summary>
        public UnixTime()
        {
            Time = DateTime.Now;
        }

        /// <summary>
        /// Инициализация объекта.
        /// </summary>
        /// <param name="timeStamp">Unix таймстемп</param>
        public UnixTime(int timeStamp)
        {
            Time = ToDateTime(timeStamp);
        }

        /// <summary>
        /// Инициализация объекта.
        /// </summary>
        /// <param name="dateTime">DateTime</param>
        public UnixTime(DateTime dateTime)
        {
            Time = dateTime;
        }

        public override string ToString()
        {
            return String.Format("{0}", Time, Timestamp);
        }

        public static UnixTime operator +(UnixTime time1, UnixTime time2)
        {
            return new UnixTime(time1.Timestamp + time2.Timestamp);
        }

        public static UnixTime operator +(UnixTime time, int timeStamp)
        {
            return new UnixTime(time.Timestamp + timeStamp);
        }

        public static UnixTime operator +(UnixTime time1, DateTime time2)
        {
            return new UnixTime(time1.Timestamp + ToUnixTime(time2));
        }

        public static UnixTime operator -(UnixTime time1, UnixTime time2)
        {
            return new UnixTime(time1.Timestamp - time2.Timestamp);
        }

        public static UnixTime operator -(UnixTime time, int timeStamp)
        {
            return new UnixTime(time.Timestamp - timeStamp);
        }

        public static UnixTime operator -(UnixTime time1, DateTime time2)
        {
            return new UnixTime(time1.Timestamp - ToUnixTime(time2));
        }

        /// <summary>
        /// Неявное преобразование (cast) int в UnixTime.
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static implicit operator UnixTime(int timeStamp)
        {
            return new UnixTime(timeStamp);
        }

        /// <summary>
        /// Неявное преобразование (cast) DateTime в UnixTime.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static implicit operator UnixTime(DateTime dateTime)
        {
            return new UnixTime(dateTime);
        }

        /// <summary>
        /// Неявное преобразование (cast) UnixTime в int.
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static implicit operator Int32(UnixTime unixTime)
        {
            return unixTime.Timestamp;
        }

        /// <summary>
        /// Неявное преобразование (cast) UnixTime в DateTime.
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static implicit operator DateTime(UnixTime unixTime)
        {
            return unixTime.Time;
        }

        public object Clone()
        {
            return new UnixTime(Timestamp);
        }
    }
}
