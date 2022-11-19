using System;
using System.Drawing;


namespace SistemaTCC.metodos
{
    public class calculoPearson
    {
        public string caminhoFotos = System.Environment.CurrentDirectory + @"\foto\";

        public string imagensPixels(string foto)
        {

            Bitmap imgExame = new Bitmap(caminhoFotos + foto);
            int[] exame = new int[imgExame.Height * imgExame.Width];

            for (int i = 0; i <= imgExame.Width - 1; i++)
            {
                for (int j = 0; j <= imgExame.Height - 1; j++)
                {
                    exame[j * i] = imgExame.GetPixel(i, j).R;
                }
            }

            Bitmap imgSaudavel = new Bitmap(caminhoFotos + "mamografiaSaudavel.jpg");
            int[] saudavel = new int[imgSaudavel.Height * imgSaudavel.Width];

            for (int i = 0; i <= imgSaudavel.Width - 1; i++)
            {
                for (int j = 0; j <= imgSaudavel.Height - 1; j++)
                {

                    saudavel[j * i] = imgSaudavel.GetPixel(i, j).R;
                }
            }
            return calculaBarra(saudavel, exame);

        }

        public string calculaBarra(int[] imgSaudavel, int[] imgExame)
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

        public string getPearson(double mediaSaudavel, double mediaExame, int[] imgSaudavel, int[] imgExame)
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

        public string mostrarResult(double imprimirResult)
        {
            string resposta = string.Empty;
            if (imprimirResult >= 0.9)
                resposta = "A mamografia analisada tem uma baixa possibilidade de possuir um tumor";
            if (imprimirResult >= 0.7 && imprimirResult < 0.9)
                resposta = "A mamografia analisada tem uma possibilidade média de possuir um tumor";
            if (imprimirResult < 0.7)
                resposta = "A mamografia analisada tem uma alta possibilidade de possuir um tumor";
            return resposta;
        }
    }
}
