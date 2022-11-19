using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using SistemaTCC.metodos;
using SistemaTCC.model;

namespace SistemaTCC
{
    public partial class frmPrincipal : Form
    {
        public MySqlConnection objCnx = new MySqlConnection();
        public MySqlCommand objCmd = new MySqlCommand();
        public MySqlDataReader objDados;
        public string caminhoFotos = System.Environment.CurrentDirectory + @"\foto\";
        public string nomeImagem;
        public string foto = "";
        calculoPearson _calculoPearson = new calculoPearson();
        cadastroPaciente _cadastrarPaciente = new cadastroPaciente();
        consultaPaciente _consultarPaciente = new consultaPaciente();
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            if (btnCadastrar.Text == "Limpar")
            {
                limparCampos();
                return;
            }

            try
            {
                if (txtNome.Text == "" || txtDocumento.Text == "" )
                {
                    MessageBox.Show("Preencha todos os campos corretamente", "Falha ao cadastrar paciente", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }
                
                if (ValidaCPF(txtDocumento.Text) && ValidaData(dtDataNasc.Value))
                {
                    pacienteModel _pacienteModel = new pacienteModel(txtDocumento.Text, txtNome.Text, dtDataNasc.Value, nomeImagem);
                    if (_cadastrarPaciente.cadastrarPaciente(_pacienteModel))
                    {
                        MessageBox.Show("Registrado com sucesso!", "Cadastro realizado com êxito", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        btnCadastrar.Text = "Limpar";
                    }
                    else
                        MessageBox.Show("Falha ao cadastrar novo paciente", "Erro ao cadastrar");

                }
                else
                {
                    if (!ValidaCPF(txtDocumento.Text))
                    {
                        MessageBox.Show("Digite o CPF corretamente", "Falha ao cadastrar paciente", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        objCnx.Close();
                    } else {
                        MessageBox.Show("Data de nascimento inválida", "Falha ao cadastrar paciente", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        objCnx.Close();
                    }
                    return;
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "Erro ao cadastrar", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                objCnx.Close();
            }
            
        }

        public bool ValidaData(DateTime dataNasc)
        {
            DateTime dataHoje = DateTime.Now;
            if(dataHoje >= dataNasc)
            {
                return true;
            }
            return false;
        }

        public bool ValidaCPF(string vrCPF)
        {
            string valor = vrCPF.Replace(".", "");
            valor = valor.Replace("-", "");

            if (valor.Length != 11)
                return false;
            bool igual = true;

            for (int i = 1; i < 11 && igual; i++)
                if (valor[i] != valor[0])
                    igual = false;
            if (igual || valor == "12345678909")
                return false;

            int[] numeros = new int[11];

            for (int i = 0; i < 11; i++)
                numeros[i] = int.Parse(
                    valor[i].ToString());

            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += (10 - i) * numeros[i];

            int resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                    return false;
            }
            else if (numeros[9] != 11 - resultado)
                return false;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += (11 - i) * numeros[i];
            resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0)
                    return false;
            }
            else
                if (numeros[10] != 11 - resultado)
                return false;
            return true;
        }

        private void picMamografia_DoubleClick(object sender, EventArgs e)
        {
            adicionarFoto();
            txtResultado.Text = _calculoPearson.imagensPixels(this.foto);
            btnCadastrar.Text = "Limpar";
        }

        private void btnAddFoto_Click(object sender, EventArgs e)
        {
            adicionarFoto();
            txtResultado.Text = _calculoPearson.imagensPixels(this.foto);
            btnCadastrar.Text = "Limpar";
        }
        private void btnConsultar_Click(object sender, EventArgs e)
        {
            if (txtDocumento.Text != "")
            {
                pacienteModel _pacienteModel = new pacienteModel();
                _pacienteModel = _consultarPaciente.consultarPaciente(txtDocumento.Text);
                if (_pacienteModel.nomeCompleto != null)
                {
                    txtNome.Text = _pacienteModel.nomeCompleto;
                    if (_pacienteModel.caminhoFoto != "")
                    {
                        picMamografia.Image = Image.FromFile(caminhoFotos + _pacienteModel.caminhoFoto);
                        txtResultado.Text = _calculoPearson.imagensPixels(_pacienteModel.caminhoFoto);
                        btnCadastrar.Text = "Limpar";
                    }
                    foto = _pacienteModel.caminhoFoto;
                    dtDataNasc.Value = _pacienteModel.dataNascimento;
                    btnCadastrar.Text = "Limpar";
                }
            }
            else
            {
                MessageBox.Show("Preencha o campo corretamente", "Erro ao consultar", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private string formataHora()
        {
            DateTime data = DateTime.Now;
            string hora = String.Format("{0:T}", data);
            string horaFormatada = hora.Replace(":", "");
            return horaFormatada;
        }

        private void limparCampos()
        {
            txtDocumento.Text = string.Empty;
            txtNome.Text = string.Empty;
            dtDataNasc.Value = DateTime.Now;
            picMamografia.Image = null;
            txtResultado.Text = string.Empty;
            btnCadastrar.Text = "Cadastrar";
        }

        private void adicionarFoto()
        {

            if (txtNome.Text == "" || txtDocumento.Text == "")
            {
                MessageBox.Show("Não é possivel adicionar foto, dados insuficientes", "Falha ao carregar imagem", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            else
            {
                string origemCompleto = "";
                string pastaDestino = caminhoFotos;
                string destinoCompleto = "";
                string horaFormatada = formataHora();

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    origemCompleto = openFileDialog1.FileName;
                    this.foto = txtDocumento.Text + horaFormatada + ".jpg";
                    nomeImagem = this.foto;
                    destinoCompleto = pastaDestino + this.foto;
                }
                System.IO.File.Copy(origemCompleto, destinoCompleto, true);
                if (File.Exists(destinoCompleto))
                {
                    if (MessageBox.Show("Arquivo ja existe, deseja substituir?", "Substituir", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        objCnx.Close();
                        return;
                    }
                }
                if(File.Exists(destinoCompleto)) 
                    picMamografia.ImageLocation = destinoCompleto;
                
              
                if (File.Exists(destinoCompleto))
                {
                    picMamografia.ImageLocation = origemCompleto;
                    objCnx.ConnectionString = "Server=localhost;Database=dbsam;user=root;pwd='123456'";
                    objCnx.Open();

                    objCmd.Connection = objCnx;
                    objCmd.CommandText = "UPDATE `dbsam`.`tblpacientes` SET `caminhoImagem` = '" + nomeImagem + "' WHERE (`CPF` = " + txtDocumento.Text + " ); ";
                    objDados = objCmd.ExecuteReader();

                    MessageBox.Show("Foto adicionada com sucesso", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    objCnx.Close();
                }
                else
                {
                    MessageBox.Show("Falha ao salvar foto na pasta destinada", "Arquivo não copiado", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    objCnx.Close();
                }
            }
        }
    }
}
