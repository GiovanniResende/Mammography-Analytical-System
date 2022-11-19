using System;
using MySql.Data.MySqlClient;
using SistemaTCC.model;


namespace SistemaTCC.metodos
{
    
    public class cadastroPaciente
    {
        public MySqlConnection objCnx = new MySqlConnection();
        public MySqlCommand objCmd = new MySqlCommand();
        public MySqlDataReader objDados;
        public bool cadastrarPaciente(pacienteModel dados)
        {
            try
            {
                objCnx.ConnectionString = "Server=localhost;Database=dbsam;user=root;pwd='123456'";
                objCnx.Open();
                string strSQL = "insert into tblpacientes(CPF, nomeCompleto, caminhoImagem, dataNascimento) values(";
                strSQL += "'" + dados.documento + "',";
                strSQL += "'" + dados.nomeCompleto + "',";
                strSQL += "'" + dados.caminhoFoto + "',";
                strSQL += "'" + dados.dataNascimento.ToString("dd/MM/yyyy") + "');";


                objCmd.Connection = objCnx;
                objCmd.CommandText = strSQL;
                objDados = objCmd.ExecuteReader();

                objCnx.Close();
                return true;
            }
            catch (Exception ex)
            {
                objCnx.Close();
                return false;
            }
            
        }
    }
}
