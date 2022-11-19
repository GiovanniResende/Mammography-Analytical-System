using System;

namespace SistemaTCC.model
{
    public class pacienteModel
    {
        public string documento{ get; set; }
        public string nomeCompleto { get; set; }
        public DateTime dataNascimento { get; set; }
        public string caminhoFoto { get; set; }

        public pacienteModel(string _documento, string _nomeCompleto, DateTime _dataNascimento, string _caminhoFoto)
        {
            this.documento = _documento;
            this.nomeCompleto = _nomeCompleto;
            this.dataNascimento = _dataNascimento;
            this.caminhoFoto = _caminhoFoto;
        }
        public pacienteModel()
        {
        }
    }
}
