using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculadoraCientifica
{
    public partial class Form1
    {
        private void btnPlotarExpressao_Click(object? sender, EventArgs e)
        {
            string expr = txtExpressao?.Text ?? "";
            plotManager.PlotFunctionWithDerivativeAndIntegral(expr, evaluator);
        }
    }
}