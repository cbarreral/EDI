
namespace CargaDescargaFTP_ArchivosEDI
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtServer = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.frmClienteFTP = new System.Windows.Forms.RichTextBox();
            this.lbCargado = new System.Windows.Forms.Label();
            this.btnDescarga = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtRuta = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnCarga = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtArchivoCarga = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.trview = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(56, 11);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(220, 20);
            this.txtServer.TabIndex = 0;
            this.txtServer.Text = "test.rebex.net/pub/example";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(558, 12);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(151, 20);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.Text = "password";
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(340, 11);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(135, 20);
            this.txtUser.TabIndex = 2;
            this.txtUser.Text = "demo";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Server";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(499, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Password";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(307, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "User";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(734, 14);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 24);
            this.button1.TabIndex = 6;
            this.button1.Text = "Conectar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmClienteFTP
            // 
            this.frmClienteFTP.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.frmClienteFTP.Location = new System.Drawing.Point(322, 109);
            this.frmClienteFTP.Name = "frmClienteFTP";
            this.frmClienteFTP.Size = new System.Drawing.Size(500, 321);
            this.frmClienteFTP.TabIndex = 7;
            this.frmClienteFTP.Text = "";
            // 
            // lbCargado
            // 
            this.lbCargado.AutoSize = true;
            this.lbCargado.Location = new System.Drawing.Point(12, 39);
            this.lbCargado.Name = "lbCargado";
            this.lbCargado.Size = new System.Drawing.Size(16, 13);
            this.lbCargado.TabIndex = 8;
            this.lbCargado.Text = "...";
            // 
            // btnDescarga
            // 
            this.btnDescarga.Enabled = false;
            this.btnDescarga.Location = new System.Drawing.Point(120, 80);
            this.btnDescarga.Name = "btnDescarga";
            this.btnDescarga.Size = new System.Drawing.Size(88, 23);
            this.btnDescarga.TabIndex = 11;
            this.btnDescarga.Text = "Descargar";
            this.btnDescarga.UseVisualStyleBackColor = true;
            this.btnDescarga.Click += new System.EventHandler(this.btnDescarga_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Lista de elementos";
            // 
            // txtRuta
            // 
            this.txtRuta.Location = new System.Drawing.Point(322, 83);
            this.txtRuta.Name = "txtRuta";
            this.txtRuta.Size = new System.Drawing.Size(292, 20);
            this.txtRuta.TabIndex = 14;
            this.txtRuta.Text = "C:\\PruebasArchivos";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(319, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(182, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Ruta de descarga/carga de archivos";
            // 
            // btnCarga
            // 
            this.btnCarga.Enabled = false;
            this.btnCarga.Location = new System.Drawing.Point(214, 80);
            this.btnCarga.Name = "btnCarga";
            this.btnCarga.Size = new System.Drawing.Size(88, 23);
            this.btnCarga.TabIndex = 16;
            this.btnCarga.Text = "Cargar";
            this.btnCarga.UseVisualStyleBackColor = true;
            this.btnCarga.Click += new System.EventHandler(this.btnCarga_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(635, 67);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(99, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Nombre del archivo";
            // 
            // txtArchivoCarga
            // 
            this.txtArchivoCarga.Location = new System.Drawing.Point(638, 83);
            this.txtArchivoCarga.Name = "txtArchivoCarga";
            this.txtArchivoCarga.Size = new System.Drawing.Size(169, 20);
            this.txtArchivoCarga.TabIndex = 18;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(734, 44);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(88, 23);
            this.button2.TabIndex = 19;
            this.button2.Text = "Limpiar";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // trview
            // 
            this.trview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.trview.Location = new System.Drawing.Point(12, 109);
            this.trview.Name = "trview";
            this.trview.Size = new System.Drawing.Size(304, 320);
            this.trview.TabIndex = 20;
            this.trview.DoubleClick += new System.EventHandler(this.btnDescarga_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 442);
            this.Controls.Add(this.trview);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.txtArchivoCarga);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnCarga);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtRuta);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnDescarga);
            this.Controls.Add(this.lbCargado);
            this.Controls.Add(this.frmClienteFTP);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtServer);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox frmClienteFTP;
        private System.Windows.Forms.Label lbCargado;
        private System.Windows.Forms.Button btnDescarga;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtRuta;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnCarga;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtArchivoCarga;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TreeView trview;
    }
}

