using MySql.Data.MySqlClient;
using SistemaTCC.model;
using System;
using System.Windows.Forms;

namespace SistemaTCC.metodos
{
    public class consultaPaciente
    {
        public MySqlConnection objCnx = new MySqlConnection();
        public MySqlCommand objCmd = new MySqlCommand();
        public MySqlDataReader objDados;

        public pacienteModel consultarPaciente(string cpf)
        {
            try
            {
                objCnx.ConnectionString = "Server=localhost;Database=dbsam;user=root;pwd='123456'";
                objCnx.Open();
                string strSQL = "SELECT * FROM dbsam.tblpacientes where CPF = ";
                strSQL += "'" + cpf + "';";


                objCmd.Connection = objCnx;
                objCmd.CommandText = strSQL;
                objDados = objCmd.ExecuteReader();

                if (!objDados.HasRows)
                {
                    MessageBox.Show("Preencha o campo corretamente", "Erro ao consultar", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    objCnx.Close();
                }
                else
                {
                    while (objDados.Read())
                    {
                            try
                            {
                                pacienteModel _pacienteModel = new pacienteModel(cpf, objDados[1].ToString(), Convert.ToDateTime(objDados[3]), objDados[2].ToString());
                                objCnx.Close();
                                return _pacienteModel;
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Imagem não encontrada", "Erro ao consultar", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                                objCnx.Close();
                                return new pacienteModel();
                            }
                        
                    }                    
                }
                return new pacienteModel();
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "Erro ao consultar", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return new pacienteModel();
            }
        }
    }
}
