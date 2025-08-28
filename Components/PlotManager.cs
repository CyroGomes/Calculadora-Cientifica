using System;
using System.IO;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using OxyPlot.ImageSharp;

namespace CalculadoraCientifica.Components
{
    public class PlotManager
    {
        private readonly PlotView plotView;

        public PlotManager(PlotView view)
        {
            plotView = view;
        }

        public void PlotFunctionWithDerivativeAndIntegral(string expr, MathEvaluator evaluator)
        {
            var model = new PlotModel { Title = $"f(x) = {expr}" };
            var series = new LineSeries { Title = "Função", Color = OxyColors.Cyan };

            for (double x = -10; x <= 10; x += 0.1)
            {
                string exprX = expr.Replace("x", x.ToString(System.Globalization.CultureInfo.InvariantCulture));
                double y = evaluator.Evaluate(exprX);
                series.Points.Add(new DataPoint(x, y));
            }

            model.Series.Add(series);
            plotView.Model = model;
        }

        public void PlotNamedFunction(Func<double, double> func, string nome)
        {
            var model = new PlotModel { Title = nome };
            var series = new LineSeries { Title = nome, Color = OxyColors.Orange };

            for (double x = -10; x <= 10; x += 0.1)
            {
                double y = func(x);
                series.Points.Add(new DataPoint(x, y));
            }

            model.Series.Add(series);
            plotView.Model = model;
        }

        public void SavePlotAsImage()
        {
            // Implementar exportação como PNG usando OxyPlot
        }
    }
}
