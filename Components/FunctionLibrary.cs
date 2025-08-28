using System;
using System.Collections.Generic;

namespace CalculadoraCientifica.Components
{
    public static class FunctionLibrary
    {
        public static Func<double, double> GetFunction(string nome)
        {
            var funcoes = new Dictionary<string, Func<double, double>>
            {
                { "Sin", Math.Sin },
                { "Cos", Math.Cos },
                { "Tan", Math.Tan },
                { "Log", Math.Log10 },
                { "Exp", Math.Exp }
            };

            return funcoes.ContainsKey(nome) ? funcoes[nome] : x => 0;
        }
    }
}