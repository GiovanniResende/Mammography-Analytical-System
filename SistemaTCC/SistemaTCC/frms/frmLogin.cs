using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using SistemaTCC.data;
using MySql.Data.MySqlClient;


namespace SistemaTCC
{
    public partial class frmLogin : Form
    {
        public MySqlConnection objCnx = new MySqlConnection();
        public MySqlCommand objCmd = new MySqlCommand();
        public MySqlDataReader objDados;

        Thread logar;
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            objCnx.ConnectionString = "Server=localhost;Database=dbSAM;user=root;pwd=''";
            objCmd.Connection = objCnx;
            objCnx.Open();
            try
            {

                string strSQL = "select count(*) as qtd from tblusuario where usuario = '"+ txtUsuario.Text +"' and senha = '"+ txtSenha.Text +"'";

                objCmd.CommandText = strSQL;
                objDados = objCmd.ExecuteReader();
                int qtd = 0;
                while (objDados.Read()) {
                    qtd = objDados.GetInt32("qtd");
                }

                if (qtd != 0)
                {
                    this.Close();
                    logar = new Thread(telaPrincipal);
                    logar.SetApartmentState(ApartmentState.STA);
                    logar.Start();
                }
                else
                {
                    MessageBox.Show("Login ou senha incorreto tente novamente");
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "Erro na conexão com o BD!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            

            objCnx.Close();
        }
        private void telaPrincipal(object obj)
        {
            Application.Run(new frmPrincipal());
        }

        

    }
}
