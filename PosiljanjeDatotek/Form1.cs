using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading;
using System.IO;
using System.Text;

// This is the code for your desktop app.
// Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.

namespace PosiljanjeDatotek
{
    public partial class Form1 : Form
    {
        /************
      * Spremenljivke
      * 
      * 
      * **********/
        // odjemalec/strežnik
        TcpClient client = null;
        TcpListener listener = null;
        IPAddress ip = null;
        int port = 1234;

        // niti
        Thread thClient = null;
        Thread thListener = null;

        string iv = null;
        RijndaelManaged newRijndael;
        string path = "bidgo.png";

        // podatkovni tok
        NetworkStream dataStream = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Odjemalec_Click(object sender, EventArgs e)
        {
            ipBox.Text = "124.0.0.1";
            ip = IPAddress.Parse(ipBox.Text);
            portBox.Text = "1234";
            port = Convert.ToInt32(portBox.Text);

            client = new TcpClient();
            SendFile(client);
        }


        private void SendFile(object pClient)
        {
            try
            {
                client = (TcpClient)pClient;
                client.Connect(ip, port);

                dataStream = client.GetStream();

                newRijndael = new RijndaelManaged();

                byte[] sharedKey = DiffieHellmanClient(newRijndael);
                    
                string original = System.IO.File.ReadAllText(@"C:\\Users\\neven\\Desktop\\Poslano\\" + path);
                
                //Send(dataStream, Convert.ToBase64String(newRijndael.IV));

                byte[] encrypted = Encrypt(original, sharedKey);

                Send(dataStream, Convert.ToBase64String(encrypted));
                

                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Napaka pri pošiljanju podatkov" + ex);
            }
        }

        private byte[] DiffieHellmanClient(Rijndael newRijndael)
        {
            ECDiffieHellmanCng ECDF = new ECDiffieHellmanCng();
            ECDF.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
            ECDF.HashAlgorithm = CngAlgorithm.Sha256;
            byte[] pubkey = ECDF.PublicKey.ToByteArray();

            string base64pubkey = Convert.ToBase64String(pubkey);

            Send(dataStream, base64pubkey); //pošlje javni ključ kot base64

            string serverPublicKey = Recieve(dataStream); //prejme njegov javni ključ

            //byte[] iv = Encoding.UTF32.GetBytes("sestnajst1234567");
            //Send(dataStream, Convert.ToBase64String(iv));
            newRijndael.GenerateIV();
            iv = Convert.ToBase64String(newRijndael.IV);
            MessageBox.Show("iv je: " + iv);
            Send(dataStream, iv);

            byte[] byteServerKey = Convert.FromBase64String(serverPublicKey);
            byte[] sharedKey = ECDF.DeriveKeyMaterial(ECDiffieHellmanCngPublicKey.FromByteArray(byteServerKey, CngKeyBlobFormat.EccPublicBlob));

            return sharedKey;
        }

        private byte[] Encrypt(string original, byte[] sharedKey)
        {
            FileStream fStream = new FileStream(@"C:\\Users\\neven\\Desktop\\Poslano2\\" + path, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            using (RijndaelManaged newRijndael = new RijndaelManaged())
            {
                newRijndael.Key = sharedKey;
                newRijndael.IV = Convert.FromBase64String(iv);

                CryptoStream cStream = new CryptoStream(fStream, newRijndael.CreateEncryptor(), CryptoStreamMode.Write);

                byte[] message = System.IO.File.ReadAllBytes(@"C:\\Users\\neven\\Desktop\\Poslano\\" + path);

                cStream.Write(message, 0, message.Length);

                cStream.Close();
                fStream.Close();
                byte[] readText = System.IO.File.ReadAllBytes(@"C:\\Users\\neven\\Desktop\\Poslano2\\" + path);
                return readText;
            }
        }
        private void Streznik_Click(object sender, EventArgs e)
        {
            // IP & port
            ipBox.Text = "127.0.0.1";
            ip = IPAddress.Parse(ipBox.Text);
            portBox.Text = "1234";
            port = Convert.ToInt32(portBox.Text);

            thListener = new Thread(new ThreadStart(GetFile));
            thListener.IsBackground = true;
            thListener.Start();
        }
        private void GetFile()
        {
            listener = new TcpListener(ip, port);
            listener.Start();

            while (true)
            {
                client = listener.AcceptTcpClient();

                dataStream = client.GetStream();

                byte[] sharedKey = DiffieHellmanServer();

                //iv = Recieve(dataStream);

                string encrypted = Recieve(dataStream);
                MessageBox.Show("Encrypted recieved: \n" + encrypted);
                
               
                Decrypt(encrypted, sharedKey, Convert.FromBase64String(iv));

                dataStream.Close();
            }
        }

        private byte[] DiffieHellmanServer()
        {
            ECDiffieHellmanCng ECDF = new ECDiffieHellmanCng();
            ECDF.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
            ECDF.HashAlgorithm = CngAlgorithm.Sha256;
            byte[] pubkey = ECDF.PublicKey.ToByteArray();

            //byte[] data = Encoding.UTF8.GetBytes(Recieve(dataStream));
            string clientPublicKey = Recieve(dataStream); // Prejme base64 ključ

            Send(dataStream, Convert.ToBase64String(pubkey)); //pošlje base64 ključ

            iv = Recieve(dataStream);

            byte[] byteClientKey = Convert.FromBase64String(clientPublicKey);
            byte[] sharedKey = ECDF.DeriveKeyMaterial(ECDiffieHellmanCngPublicKey.FromByteArray(byteClientKey, CngKeyBlobFormat.EccPublicBlob));

            return sharedKey;
        }

        
        private void Decrypt(string encrypted, byte[] sharedKey, byte[] iVector)
        {
            FileStream fStream = new FileStream(@"C:\\Users\\neven\\Desktop\\Prejeto\\" + path, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            newRijndael = new RijndaelManaged();

            //iv = Recieve(dataStream);
            newRijndael.Key = sharedKey;
            newRijndael.IV = iVector;
            
            
            CryptoStream cStream = new CryptoStream(fStream, newRijndael.CreateDecryptor(), CryptoStreamMode.Write);
            byte[] message = Convert.FromBase64String(encrypted);
            cStream.Write(message, 0, message.Length);

            //string data = reader.ReadToEnd();

            cStream.Close();
            fStream.Close();
            /*
            // Create a decryptor to perform the stream transform.
            ICryptoTransform decryptor = newRijndael.CreateDecryptor(newRijndael.Key, newRijndael.IV);

            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(encrypted))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        // Read the decrypted bytes from the decrypting stream
                        // and place them in a string.
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
            fStream.Close();
            */
        }

       


        private void Send(NetworkStream stream, string data)
        {
            try
            {
                byte[] send = Encoding.UTF8.GetBytes(data.ToCharArray(), 0, data.Length);
                //System.Threading.Thread.Sleep(2000);
                stream.Write(send, 0, send.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Napaka pri posiljanju\n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private string Recieve(NetworkStream stream)
        {
            try
            {
                byte[] recv = new byte[1024];
                int length = stream.Read(recv, 0, recv.Length);
                return Encoding.UTF8.GetString(recv, 0, length);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Napaka pri prejemanju" + ex.Message + "\n" + ex.StackTrace);
                return null;
            }
        }
    }
}
