using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Web;

namespace SysWbudProject.SerialPorts
{
    public class SafeSerialPort : SerialPort
    {
        public SafeSerialPort(string s, int p1, Parity parity, int p2)
            : base(s, p1, parity, p2)
        {

        }
        public new void Open()
        {
            if (!base.IsOpen)
            {
                base.Open();
                GC.SuppressFinalize(this.BaseStream);
            }
        }

        public new void Close()
        {
            if (base.IsOpen)
            {
                GC.ReRegisterForFinalize(this.BaseStream);
                base.Close();
            }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                base.Dispose(disposing);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }
    }
}