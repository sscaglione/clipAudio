using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

// Some code in this class is adapted from http://stackoverflow.com/a/12150289

namespace TimeUtil
{
    class TimeUtil
    {
        private const int TIMEOUT = 1000;

        private static TimeSpan ntpOffset = TimeSpan.Zero;

        public static DateTime getCurrentTime()
        {
            if (TimeUtil.ntpOffset == TimeSpan.Zero)
                TimeUtil.updateNTP(); // Get the internet time for the first time.
            return DateTime.Now + TimeUtil.ntpOffset;
        }

        public static string getCurrentTimeString()
        {
            return TimeUtil.getCurrentTime().ToString("MM/dd/yyyy H:mm:ss.fff");
        }

        public static void updateNTP()
        {
            try
            {
                const string ntpServer = "3.north-america.pool.ntp.org";
                byte[] ntpData = new byte[48];

                // Setting the Leap Indicator, Version Number and Mode values
                ntpData[0] = 0x1B; // LI = 0 (no warning), VN = 3 (IPv4 only), Mode = 3 (Client Mode)

                IPAddress[] addresses = Dns.GetHostEntry(ntpServer).AddressList;
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                socket.SetSocketOption(SocketOptionLevel.Socket,
                    SocketOptionName.ReceiveTimeout, TimeUtil.TIMEOUT);
                socket.SetSocketOption(SocketOptionLevel.Socket,
                    SocketOptionName.SendTimeout, TimeUtil.TIMEOUT);
                socket.Connect(new IPEndPoint(addresses[0], 123));

                DateTime t1 = DateTime.Now;
                socket.Send(ntpData);
                socket.Receive(ntpData);
                socket.Close();
                DateTime t4 = DateTime.Now;
                DateTime t2 = TimeUtil.timeFromBytes(ntpData, 32);
                DateTime t3 = TimeUtil.timeFromBytes(ntpData, 40);
                // Offset calculation from RFC 5905.
                TimeUtil.ntpOffset = new TimeSpan(((t2 - t1) + (t3 - t4)).Ticks / 2);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Could not get NTP time!");
                TimeUtil.ntpOffset = TimeSpan.FromTicks(1);
            }
        }

        private static DateTime timeFromBytes(byte[] data, int position)
        {
            ulong intPart = TimeUtil.swapEndianness(BitConverter.ToUInt32(data, position));
            ulong fractPart = TimeUtil.swapEndianness(BitConverter.ToUInt32(data, position + 4));
            ulong milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
            return new DateTime(1900, 1, 1).AddMilliseconds(milliseconds);
        }

        private static uint swapEndianness(uint x)
        {
            return (uint)(((x & 0x000000ff) << 24) |
                           ((x & 0x0000ff00) << 8) |
                           ((x & 0x00ff0000) >> 8) |
                           ((x & 0xff000000) >> 24));
        }
    }
}
