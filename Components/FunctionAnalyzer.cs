using System;
using System.Collections.Generic;
using System.Globalization;

namespace CalculadoraCientifica.Components
{
    /// <summary>
    /// Analisa uma expressão matemática e identifica raízes e possíveis assíntotas.
    /// </summary>
    public class FunctionAnalyzer
    {
        private readonly MathEvaluator evaluator;

        public FunctionAnalyzer(MathEvaluator eval)
        {
            evaluator = eval;
        }

        /// <summary>
        /// Analisa a expressão fornecida substituindo x por valores no intervalo [-10, 10].
        /// </summary>
        /// <param name="expr">Expressão matemática com variável x</param>
        /// <returns>Lista de observações sobre a função</returns>
        public List<string> Analisar(string expr)
        {
            var relatorio = new List<string>();

            for (double x = -10; x <= 10; x += 0.5)
            {
                string exprX = expr.Replace("x", x.ToString(CultureInfo.InvariantCulture));
                double y = evaluator.Evaluate(exprX);

                if (y == 0)
                    relatorio.Add($"Raiz em x = {x}");

                if (Math.Abs(y) > 1000)
                    relatorio.Add($"Possível assíntota em x = {x}");
            }

            return relatorio;
        }
    }
}