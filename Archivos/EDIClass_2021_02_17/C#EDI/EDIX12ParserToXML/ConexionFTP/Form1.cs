using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConexionFTP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string server = txtServer.Text;
            string user = txtUser.Text;
            string pass = txtPassword.Text;
            if (server != null || user != null)
            {
                listarFTP(server, user, pass);
            }else
            {
                MessageBox.Show("Llene todos los campos");
            }
           
        }

        private void listarFTP(string dir, string user, string pass)
        {
            try
            {
                FtpWebRequest dirFtp = ((FtpWebRequest)FtpWebRequest.Create(dir));
           
            // Los datos del usuario (credenciales)
            NetworkCredential cr = new NetworkCredential(user, pass);
            dirFtp.Credentials = cr;

            // El comando a ejecutar
            dirFtp.Method = "LIST";

            // También usando la enumeración de WebRequestMethods.Ftp
            dirFtp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

            // Obtener el resultado del comando
            StreamReader reader =
                new StreamReader(dirFtp.GetResponse().GetResponseStream());

            // Leer el stream
            string res = reader.ReadToEnd();

            // Mostrarlo.
            Console.WriteLine(res);
            txtRespuesta.Text = res;

            // Cerrar el stream abierto.
            reader.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
