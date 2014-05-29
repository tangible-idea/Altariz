using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace songpa
{
    class SharedAPI
    {
        // 구조체 선언
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct NETRESOURCE
        {
            public uint dwScope;
            public uint dwType;
            public uint dwDisplayType;
            public uint dwUsage;
            public string lpLocalName;
            public string lpRemoteName;
            public string lpComment;
            public string lpProvider;
        }

        // API 함수 선언
        [DllImport("mpr.dll", CharSet = CharSet.Auto)]
        public static extern int WNetUseConnection(
                    IntPtr hwndOwner,
                    [MarshalAs(UnmanagedType.Struct)] ref NETRESOURCE lpNetResource,
                    string lpPassword,
                    string lpUserID,
                    uint dwFlags,
                    StringBuilder lpAccessName,
                    ref int lpBufferSize,
                    out uint lpResult);

        // API 함수 선언 (공유해제)
        [DllImport("mpr.dll", EntryPoint = "WNetCancelConnection2", CharSet = CharSet.Auto)]
        public static extern int WNetCancelConnection2A(string lpName, int dwFlags, int fForce);

        // 공유연결
        public int ConnectRemoteServer(string server, string id, string pw)
        {
            int capacity = 64;
            uint resultFlags = 0;
            uint flags = 0;
            System.Text.StringBuilder sb = new System.Text.StringBuilder(capacity);
            NETRESOURCE ns = new NETRESOURCE();
            ns.dwType = 1;              // 공유 디스크
            ns.lpLocalName = null;   // 로컬 드라이브 지정하지 않음
            ns.lpRemoteName = server;
            ns.lpProvider = null;
            int result = 0;
            //if (server == @"\\10.144.70.120\d$")
            //if (server == @"\\192.168.0.14\all")
            {
                result = WNetUseConnection(IntPtr.Zero, ref ns, pw, id, flags, sb, ref capacity, out resultFlags);
            }
            //else
            //{
            //    result = WNetUseConnection(IntPtr.Zero, ref ns, "0401", "Administrator", flags, sb, ref capacity, out resultFlags);
            //}
            return result;
        }

        // 공유해제
        public void CencelRemoteServer(string server)
        {
            WNetCancelConnection2A(server, 1, 0);
        }
    }

}
