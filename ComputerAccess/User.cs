using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Web;

namespace ComputerAccess
{
    class User
    {
        public ListBox listBox;
        public Socket socket;
        byte[] buffer = new byte[1024 * 30];
        ListBox proccess;

        public User(Socket _socket, ListBox _panel, ListBox _proccess = null)
        {
            listBox = _panel;
            socket = _socket;
            proccess = _proccess;
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(dataReceive), socket);
        }

        void dataReceive(IAsyncResult AR)
        {
            try
            {
                Socket sck = (Socket)AR.AsyncState;
                int index = sck.EndReceive(AR);
                byte[] sized = new byte[index];
                Array.Copy(buffer, sized, index);

                doSmth(sized);
                String str = Encoding.Unicode.GetString(sized);
                if (!(str[0] == '*' && str[1] == '5'))
                    socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(dataReceive), socket);
            }
            catch { }
        }

        void doSmth(byte[] data)
        {
            String str = Encoding.Unicode.GetString(data);
            String str1 = "";
            try
            {
                str1 = str.Substring(0, "Proccess List".Length);
            }
            catch { }
            if (str1.Equals("Proccess List"))
            {
                proccessListReceive(str);
                return;
            }

            moveNextDir(str);

        }

        void proccessListReceive(String str)
        {
            String name = "";
            str = str.Remove(0, "Proccess List".Length);
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '*')
                {
                    proccess.Invoke(new Action(() => proccess.Items.Add(name)));

                    str = str.Remove(i, 1);
                    i--;
                    name = "";
                    continue;
                }
                name += str[i];
            }
        }

        void moveNextDir(String str)
        {
            String name = "";
            name = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '*')
                {
                    listBox.Invoke(new Action(() => listBox.Items.Add(name)));

                    str = str.Remove(i, 1);
                    i--;
                    name = "";
                    continue;
                }
                name += str[i];
            }
        }

        void clearBuffer()
        {
            byte[] buffer = new byte[1024 * 1024 * 60];
        }

    }
}
                    