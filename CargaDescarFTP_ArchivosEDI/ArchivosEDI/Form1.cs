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
using System.IO;

namespace CargaDescargaFTP_ArchivosEDI
{
    public partial class Form1 : Form
    {
        public struct Archivo
        {
            public string nombre;
            public bool bDirectorio;
            public Int64 tmaño;
            public string fecha;
        }

        public Form1()
        {
            InitializeComponent();
        }
        public Uri uri;
        public FtpWebRequest request;
        public NetworkCredential credential;
        private void button1_Click(object sender, EventArgs e)
        {
            btnCarga.Enabled = btnDescarga.Enabled = true;
            
            try
            {
                lbCargado.Text = "Cargando, espera unos segundos";
                //MessageBox.Show("Cargando...");
                uri = new Uri("ftp://" + txtServer.Text + "//");
                request = (FtpWebRequest)WebRequest.Create(uri);

                credential = new NetworkCredential();
                credential.Domain = "TECHSOFT";
                credential.UserName = txtUser.Text;
                credential.Password = txtPassword.Text;

                request.Credentials = credential;
                request.EnableSsl = false;
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                request.KeepAlive = true;
                request.UsePassive = true;
                request.GetType();
                

                lbCargado.Text = "Cargando, Visualizando datos";


                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);

                string resultado = sr.ReadToEnd();
                frmClienteFTP.Text = response.WelcomeMessage + "\r\n" + resultado;
                response.Close();

                List<Archivo> files = obtenerLita(resultado);
               // LviewArchivos.View = View.List;
                foreach (Archivo item in files)
                {
                   // LviewArchivos.Items.Add(item.nombre);
                    trview.Nodes.Add(item.nombre);
                }

            }
            catch (Exception a)
            {
                lbCargado.Text = a.Message;
                MessageBox.Show(a.Message);
            }
            lbCargado.Text = "Doble click en el elemeno a descargar";

        }

        private List<Archivo> obtenerLita(string datos)
        {
            List<Archivo> retorno = new List<Archivo>();
            string[] registros = datos.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            string procesaItem = "";
            string fechaStr = "";
            string horStr = "";
            foreach (string item in registros)
            {
                Archivo file = new Archivo();
                file.nombre = "..";
                procesaItem = item.Trim();
                fechaStr = procesaItem.Substring(0, 8);
                procesaItem = (procesaItem.Substring(8, procesaItem.Length - 8)).Trim();
                horStr = procesaItem.Substring(0, 7);
                procesaItem = (procesaItem.Substring(7, procesaItem.Length - 7)).Trim();

                file.fecha = fechaStr + " " + horStr;
                if (procesaItem.Substring(0, 5) == "<DIR>")
                {
                    file.bDirectorio = true;
                    procesaItem = (procesaItem.Substring(5, procesaItem.Length - 5)).Trim();
                }
                else
                {
                    string[] strs = procesaItem.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    file.tmaño = Int64.Parse(strs[0]);
                    procesaItem = String.Join(" ", strs, 1, strs.Length - 1);

                    file.bDirectorio = false;

                }
                file.nombre = procesaItem;

                if (file.nombre != "" && file.nombre != "." && file.nombre != "..")
                {
                    retorno.Add(file);
                }



            }

            return retorno;
        }

        private void btnDescarga_Click(object sender, EventArgs e)
        {
            try
            {
                lbCargado.Text = "Descargando...";

                //string descarga = LviewArchivos.SelectedItems[0].Text;
                string descarga = trview.SelectedNode.Text;
                uri = new Uri("ftp://" + txtServer.Text + "/" + descarga);

                request = (FtpWebRequest)WebRequest.Create(uri);
                credential = new NetworkCredential();
                credential.Domain = "TECHSOFT";
                credential.UserName = txtUser.Text;
                credential.Password = txtPassword.Text;

                request.Credentials = credential;
                request.EnableSsl = false;
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.KeepAlive = true;
                request.UsePassive = true;
                
                FtpWebResponse respuest = (FtpWebResponse)request.GetResponse();
                Stream s = respuest.GetResponseStream();
                string ruta = @"" + txtRuta.Text + @"\" + descarga;
                FileStream fs = new FileStream(ruta, FileMode.Create, FileAccess.Write);
                txtRuta.Text = @"C:\PruebasArchivos";
                txtArchivoCarga.Text = descarga;
                lbCargado.Text = "Descarga exitosa en la ruta: " + ruta;
                MessageBox.Show("Descarga exitosa en la ruta: " + ruta);
                crearArchivo(s, fs);

            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                lbCargado.Text = ee.Message;

            }
        }
        private void crearArchivo(Stream origen, Stream destino)
        {
            byte[] buffer = new byte[1024];
            int byteLeidos = origen.Read(buffer, 0, 1024);
            while (byteLeidos != 0)
            {
                destino.Write(buffer, 0, byteLeidos);
                byteLeidos = origen.Read(buffer, 0, 1024);
            }
            origen.Close();
            destino.Close();
        }

        private void btnCarga_Click(object sender, EventArgs e)
        {
            try
            {


                uri = new Uri("ftp://" + txtServer.Text + "/" + txtArchivoCarga.Text);

                request = (FtpWebRequest)WebRequest.Create(uri);
                credential = new NetworkCredential();
                credential.Domain = "TECHSOFT";
                credential.UserName = txtUser.Text;
                credential.Password = txtPassword.Text;

                request.Credentials = credential;
                request.EnableSsl = false;
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.KeepAlive = true;
                request.UsePassive = true;
                lbCargado.Text = "Cargando, Visualizando datos";
               // FtpWebResponse respuest = (FtpWebResponse)request.GetResponse();
                Stream destino = request.GetRequestStream();
                string ruta = @"" + txtRuta.Text + @"\" + txtArchivoCarga.Text;
                FileStream origen = new FileStream(ruta, FileMode.Open, FileAccess.Read);
                txtRuta.Text = @"C:\PruebasArchivos\" + txtArchivoCarga.Text;
                MessageBox.Show("Descarga exitosa en la ruta: " + ruta);
                crearArchivo(origen, destino);

            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                lbCargado.Text = ee.Message;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            btnCarga.Enabled = btnDescarga.Enabled = false;
          
            txtArchivoCarga.Text = txtPassword.Text = txtServer.Text = txtUser.Text = txtRuta.Text = frmClienteFTP.Text = "";
            // LviewArchivos.Items.Clear();
            trview.Nodes.Clear();
        }
    }
}
