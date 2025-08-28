using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms; 
using CalculadoraCientifica.Components;
using OxyPlot.WindowsForms;

namespace CalculadoraCientifica
{
    public partial class Form1 : Form
    {
        private readonly PlotManager plotManager;
        private readonly MathEvaluator evaluator = new MathEvaluator();
        private readonly FunctionAnalyzer analyzer;
        private TextBox? txtExpressao;
        private ListBox? lstHistorico;
        private ComboBox? cmbFuncoes;
        private ListBox? lstAnalise;
        private Button? btnPlotarExpressao;
        private Button? btnPlotarFuncao;
        private Button? btnSalvarImagem;
        private Button? btnAnalisarFuncao;

        public Form1(PlotView plotView)
        {
            InitializeComponent();

            plotManager = new PlotManager(plotView);
            evaluator = new MathEvaluator();
            analyzer = new FunctionAnalyzer(evaluator);

            InicializarInterface();
            CriarTecladoComTabela();
        }
    }
}