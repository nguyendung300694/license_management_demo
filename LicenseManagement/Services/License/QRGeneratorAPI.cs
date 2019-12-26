using System;
using System.Runtime.InteropServices;
using LicenseManagement.Helpers;

namespace LicenseGen
{
    public class QRGeneratorAPI
    {
        //#if DEBUG
        //        const string QRGENERATOR_DLL = "I3App_Dlls\\QRGeneratord.i3dll";
        //#else
        //        const string QRGENERATOR_DLL = "I3App_Dlls\\qrgenerator.i3dll";
        //#endif
        const string QRGENERATOR_DLL = "I3App_Dlls\\qrgenerator.i3dll";

        [DllImport(QRGENERATOR_DLL, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private extern static void qr_free(IntPtr qrcode);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct _QRcode
        {
            public int version;
            public int width;
            public IntPtr data;
        }

        [DllImport(QRGENERATOR_DLL, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private extern static IntPtr qr_encode_string(string _str, EnumCollection.QRecLevel level);
        public static EnumCollection.QRcode QREncodeString(string _str, EnumCollection.QRecLevel level)
        {
            EnumCollection.QRcode qr_code;
            IntPtr ptr = qr_encode_string(_str, level);
            if (ptr != IntPtr.Zero)
            {
                _QRcode _qr_code = (_QRcode)Marshal.PtrToStructure(ptr, typeof(_QRcode));
                qr_code.version = _qr_code.version;
                qr_code.width = _qr_code.width;
                qr_code.data = new bool[qr_code.width * qr_code.width];
                byte[] pSourceByte = new byte[_qr_code.width * _qr_code.width];
                Marshal.Copy(_qr_code.data, pSourceByte, 0, _qr_code.width * _qr_code.width);
                for (int i = 0; i < _qr_code.width * _qr_code.width; i++)
                {
                    qr_code.data[i] = (pSourceByte[i] & 1) == 1;
                }
                qr_free(ptr);
            }
            else
            {
                qr_code = new EnumCollection.QRcode();
            }
            return qr_code;
        }

        [DllImport(QRGENERATOR_DLL, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private extern static IntPtr qr_encode_data(int size, IntPtr data, EnumCollection.QRecLevel level);
        public static EnumCollection.QRcode QREncodeData(int size, IntPtr data, EnumCollection.QRecLevel level)
        {
            EnumCollection.QRcode qr_code;
            IntPtr ptr = qr_encode_data(size, data, level);
            if (ptr != IntPtr.Zero)
            {                
                _QRcode _qr_code = (_QRcode)Marshal.PtrToStructure(ptr, typeof(_QRcode));
                qr_code.version = _qr_code.version;
                qr_code.width = _qr_code.width;
                qr_code.data = new bool[qr_code.width * qr_code.width];
                byte[] pSourceByte = new byte[_qr_code.width * _qr_code.width];
                Marshal.Copy(_qr_code.data, pSourceByte, 0, _qr_code.width * _qr_code.width);
                for (int i = 0; i < _qr_code.width * _qr_code.width; i++)
                {
                    qr_code.data[i] = (pSourceByte[i] & 1) == 1;
                }
                qr_free(ptr);
            }
            else
            {
                qr_code = new EnumCollection.QRcode();
            }
            return qr_code;
        }
    }
}