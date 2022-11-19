using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;

namespace SistemaTCC
{
    public partial class frmPrincipal : Form
    {
        public MySqlConnection objCnx = new MySqlConnection();
        public MySqlCommand objCmd = new MySqlCommand();
        public MySqlDataReader objDados;
        public static string caminho = System.Environment.CurrentDirectory;
        public static string caminhoFotos = caminho + @"\foto\";
        public string nomeImagem;
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNome.Text == "" || txtDocumento.Text == "" )
                {
                    MessageBox.Show("Preencha todos os campos corretamente", "Falha ao cadastrar paciente");
                    return;
                }

                objCnx.ConnectionString = "Server=localhost;Database=dbsam;user=root;pwd=''";
                objCnx.Open();
                string strSQL = "insert into tblpacientes(CPF, nomeCompleto, caminhoImagem, dataNascimento) values(";
                strSQL += "'" + txtDocumento.Text + "',";
                strSQL += "'" + txtNome.Text + "',";
                strSQL += "'" + nomeImagem + "',";
                strSQL += "'" + dtDataNasc.Text + "');";
                

                objCmd.Connection = objCnx;
                objCmd.CommandText = strSQL;
                objDados = objCmd.ExecuteReader();


                MessageBox.Show("Registrado com sucesso!");
                objCnx.Close();
            }
            catch (Exception erro)
            {

                MessageBox.Show(erro.Message, "Erro ao cadastrar");
            }
        }

        private void picMamografia_DoubleClick(object sender, EventArgs e)
        {

            if (txtNome.Text == "" || txtDocumento.Text == "")
            {
                MessageBox.Show("Não é possivel adicionar foto, dados insuficientes", "Falha ao carregar imagem");
            }
            else
            {
                string origemCompleto = "";
                string foto = "";
                string pastaDestino = caminhoFotos;
                string destinoCompleto = "";

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    origemCompleto = openFileDialog1.FileName;
                    foto = txtDocumento.Text + ".jpg";
                    nomeImagem = foto;
                    destinoCompleto = pastaDestino + foto;
                }
                if (File.Exists(destinoCompleto))
                    if (MessageBox.Show("Arquivo ja existe, deseja substituir?", "Substituir", MessageBoxButtons.YesNo) == DialogResult.No)
                        return;

                if (!string.IsNullOrEmpty(origemCompleto))
                    System.IO.File.Copy(origemCompleto, destinoCompleto, true);
                else
                    return;

                if (File.Exists(destinoCompleto))
                {
                    picMamografia.ImageLocation = origemCompleto;
                    objCnx.ConnectionString = "Server=localhost;Database=dbsam;user=root;pwd=''";
                    objCnx.Open();
                    string strSQL = "UPDATE `dbsam`.`tblpacientes` SET `caminhoImagem` = '" + nomeImagem + "' WHERE (`CPF` = " + txtDocumento.Text + " ); ";

                    objCmd.Connection = objCnx;
                    objCmd.CommandText = strSQL;
                    objDados = objCmd.ExecuteReader();

                    MessageBox.Show("Foto adicionada com sucesso", "Sucesso");
                    objCnx.Close();
                }
                else
                    MessageBox.Show("Arquivo não copiado");
            }
        }

        private void btnAddFoto_Click(object sender, EventArgs e)
        {
            if (txtNome.Text == "" || txtDocumento.Text == "")
            {
                MessageBox.Show("Não é possivel adicionar foto, dados insuficientes", "Falha ao carregar imagem");
            }
            else
            {
                string origemCompleto = "";
                string foto = "";
                string pastaDestino = caminhoFotos;
                string destinoCompleto = "";

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    origemCompleto = openFileDialog1.FileName;
                    foto = txtDocumento.Text + ".jpg";
                    nomeImagem = foto;
                    destinoCompleto = pastaDestino + foto;
                }
                if (File.Exists(destinoCompleto))
                    if (MessageBox.Show("Arquivo ja existe, deseja substituir?", "Substituir", MessageBoxButtons.YesNo) == DialogResult.No)
                        return;

                if (!string.IsNullOrEmpty(origemCompleto))
                    System.IO.File.Copy(origemCompleto, destinoCompleto, true);
                else
                    return;

                if (File.Exists(destinoCompleto))
                {
                    picMamografia.ImageLocation = origemCompleto;
                    objCnx.ConnectionString = "Server=localhost;Database=dbsam;user=root;pwd=''";
                    objCnx.Open();
                    string strSQL = "UPDATE `dbsam`.`tblpacientes` SET `caminhoImagem` = '"+ nomeImagem +"' WHERE (`CPF` = " + txtDocumento.Text +" ); ";
 
                    objCmd.Connection = objCnx;
                    objCmd.CommandText = strSQL;
                    objDados = objCmd.ExecuteReader();

                    MessageBox.Show("Foto adicionada com sucesso", "Sucesso");
                    objCnx.Close();
                }
                else
                    MessageBox.Show("Arquivo não copiado");
            }
            
        }

        public double calculaBarra(int[] imgSaudavel, int[] imgExame)
        {
            double mediaExame = 0.00;
            double mediaSaudavel = 0.00;

            for (int cont = 0; cont <= imgExame.Length - 1; cont++)
            {
                mediaExame = mediaExame + imgExame[cont];
                mediaSaudavel = mediaSaudavel + imgSaudavel[cont];
            }
            mediaExame = (double)mediaExame / imgExame.Length;
            mediaSaudavel = (double)mediaExame / imgSaudavel.Length;

            mediaExame = (double)mediaExame / imgExame.Length;
            mediaSaudavel = (double)mediaSaudavel / imgSaudavel.Length;

            return getPearson(mediaSaudavel, mediaExame, imgSaudavel, imgExame);
        }

        public double mostrarResult(double imprimirResult)
        {
            if(imprimirResult >= 0.9)
                txtResultado.Text = "As possibilidade de existir um tumor são menores que 10%";
            if (imprimirResult >= 0.8 && imprimirResult <= 0.9)
                txtResultado.Text = "As possibilidade de existir um tumor são entre 20% e 10%";
            if (imprimirResult >= 0.7 && imprimirResult <= 0.8)
                txtResultado.Text = "As possibilidade de existir um tumor são entre 30% e 20%";
            if (imprimirResult >= 0.6 && imprimirResult <= 0.7) 
                txtResultado.Text = "As possibilidade de existir um tumor são entre 40% e 30%";
            if (imprimirResult >= 0.5 && imprimirResult <= 0.6)
                txtResultado.Text = "As possibilidade de existir um tumor são entre 50% e 40%";
            if (imprimirResult >= 0.4 && imprimirResult <= 0.5)
                txtResultado.Text = "As possibilidade de existir um tumor são entre 60% e 50%";
            if (imprimirResult >= 0.3 && imprimirResult <= 0.4)
                txtResultado.Text = "As possibilidade de existir um tumor são entre 70% e 60%";
            if (imprimirResult >= 0.2 && imprimirResult <= 0.3)
                txtResultado.Text = "As possibilidade de existir um tumor são entre 80% e 70%";
            if (imprimirResult >= 0.1 && imprimirResult <= 0.2)
                txtResultado.Text = "As possibilidade de existir um tumor são entre 90% e 80%";
            if (imprimirResult >= 0.0 && imprimirResult <= 0.1)
                txtResultado.Text = "As possibilidade de existir um tumor são maiores que 90%";
            return 0;
        }

        public double getPearson(double mediaSaudavel, double mediaExame, int[] imgSaudavel, int[] imgExame)
        {
            double somaXY = 0.00;
            double somaExame = 0.00;
            double somaSaudavel = 0.00;
            double resultado = 0.00;

            for (int cont = 0; cont <= imgSaudavel.Length - 1; cont++)
            {
                somaXY = (double)somaXY + (imgExame[cont] - mediaExame) * (imgSaudavel[cont] - mediaSaudavel);
                somaExame = (double)somaExame + Math.Pow((imgExame[cont] - mediaExame), 2);
                somaSaudavel = (double)somaSaudavel + Math.Pow((imgSaudavel[cont] - mediaSaudavel), 2);
            }
            somaXY = (double)somaXY / (imgExame.Length - 1);
            somaExame = (double)somaExame / (imgExame.Length - 1);
            somaSaudavel = (double)somaSaudavel / (imgSaudavel.Length - 1);
            resultado = (double)somaXY / (Math.Sqrt(somaExame) * Math.Sqrt(somaSaudavel));

            return mostrarResult(resultado);
        }
        
        private void btnProcessar_Click(object sender, EventArgs e)
        {

            Bitmap imgExame = new Bitmap(picMamografia.Image);
            int[] exame = new int[imgExame.Height * imgExame.Width];
            Color cor = new Color();

            for (int i = 0; i <= imgExame.Width - 1; i++)
            {
                for (int j = 0; j <= imgExame.Height - 1; j++)
                {
                    exame[j * i] = imgExame.GetPixel(i, j).R;
                }
            }

            Bitmap imgSaudavel = new Bitmap("E:\\SistemaTCC\\SistemaTCC\bin\\Debug\foto\\mamografiaSaudavel.jpg");
            int[] saudavel = new int[imgSaudavel.Height * imgSaudavel.Width];

            for (int i = 0; i <= imgSaudavel.Width - 1; i++)
            {
                for (int j = 0; j <= imgSaudavel.Height - 1; j++)
                {

                    saudavel[j * i] = imgSaudavel.GetPixel(i, j).R;
                }
            }
            calculaBarra(saudavel, exame);

        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            if (txtDocumento.Text != "")
            {
                try
                {
                    objCnx.ConnectionString = "Server=localhost;Database=dbsam;user=root;pwd=''";
                    objCnx.Open();
                    string strSQL = "SELECT * FROM dbsam.tblpacientes where CPF = ";
                    strSQL += "'" + txtDocumento.Text + "';";


                    objCmd.Connection = objCnx;
                    objCmd.CommandText = strSQL;
                    objDados = objCmd.ExecuteReader();

                    while (objDados.Read())
                    {
                        txtNome.Text = objDados[1].ToString();
                        dtDataNasc.Text = objDados[3].ToString();
                        if (objDados[2].ToString() != "")
                            picMamografia.Image = Image.FromFile("E:\\SistemaTCC\\SistemaTCC\\bin\\Debug\\foto\\" + objDados[2]);
                    }
                    objCnx.Close();
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.Message, "Erro ao consultar");
                }
            }
            else
            {
                MessageBox.Show("Preencha o campo corretamente", "Erro ao consultar");
            }
        }
    }
}
