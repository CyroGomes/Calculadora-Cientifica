using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OxyPlot.WindowsForms;

namespace CalculadoraCientifica
{
    public partial class Form1 : Form
    {
        // Estado da interface
        private bool usarTemaEscuro = true;
        private bool usarGraus = false;
        private PlotView? plotView;

        public Form1()
        {
            InitializeComponent();

            // Tamanho da janela
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(1000, 700);

            plotView = new PlotView
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            InicializarInterface();
            InicializarGrafico();
        }

        // Interface principal
        private void InicializarInterface()
        {
            var painelPrincipal = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 3,
                RowCount = 3,
                AutoSize = true,
                Padding = new Padding(30),
                BackColor = Color.Transparent
            };

            painelPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            painelPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            painelPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));

            painelPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            painelPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            painelPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            txtExpressao = new TextBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };
            txtExpressao.TextChanged += txtExpressao_TextChanged;
            painelPrincipal.Controls.Add(txtExpressao, 0, 0);
            painelPrincipal.SetColumnSpan(txtExpressao, 3);

            var painelBotoes = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true
            };

            btnPlotarExpressao = CriarBotaoPersonalizado("Plotar Expressão", btnPlotarExpressao_Click);
            btnSalvarImagem = CriarBotaoPersonalizado("Salvar Imagem", btnSalvarImagem_Click);
            btnPlotarFuncao = CriarBotaoPersonalizado("Plotar Função", btnPlotarFuncao_Click);
            btnAnalisarFuncao = CriarBotaoPersonalizado("Analisar Função", btnAnalisarFuncao_Click);

            painelBotoes.Controls.AddRange(new Control[]
            {
                btnPlotarExpressao, btnSalvarImagem, btnPlotarFuncao, btnAnalisarFuncao
            });

            painelPrincipal.Controls.Add(painelBotoes, 0, 1);
            painelPrincipal.SetColumnSpan(painelBotoes, 3);

            lstHistorico = new ListBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };
            lstHistorico.DoubleClick += lstHistorico_DoubleClick;
            painelPrincipal.Controls.Add(lstHistorico, 0, 2);

            cmbFuncoes = new ComboBox
            {
                Dock = DockStyle.Top,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };
            cmbFuncoes.Items.AddRange(new string[] { "Sin", "Cos", "Tan", "Log", "Exp" });

            var painelFuncoes = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true
            };
            painelFuncoes.Controls.Add(cmbFuncoes);
            painelPrincipal.Controls.Add(painelFuncoes, 1, 2);

            lstAnalise = new ListBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };
            painelPrincipal.Controls.Add(lstAnalise, 2, 2);

            Controls.Add(painelPrincipal);
        }

        // Painel gráfico
        private void InicializarGrafico()
        {
            var painelGrafico = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 1,
                Padding = new Padding(10)
            };

            painelGrafico.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            painelGrafico.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            painelGrafico.Controls.Add(plotView!, 0, 0);
            Controls.Add(painelGrafico);
        }

        // Criação de botões reutilizáveis
        private Button CriarBotao(string texto, EventHandler evento)
        {
            var btn = new Button
            {
                Text = texto,
                AutoSize = true,
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White,
                Margin = new Padding(5),
                Padding = new Padding(5),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                UseVisualStyleBackColor = false
            };
            btn.Click += evento;
            return btn;
        }

        // Alternância de tema
        private void AlternarTema(bool escuro)
        {
            usarTemaEscuro = escuro;

            Color fundo = escuro ? Color.FromArgb(45, 45, 45) : Color.White;
            Color texto = escuro ? Color.White : Color.Black;

            this.BackColor = fundo;
            txtExpressao!.BackColor = fundo;
            txtExpressao.ForeColor = texto;
            lstHistorico!.BackColor = fundo;
            lstHistorico.ForeColor = texto;
            lstAnalise!.BackColor = fundo;
            lstAnalise.ForeColor = texto;
            cmbFuncoes!.BackColor = fundo;
            cmbFuncoes.ForeColor = texto;

            foreach (Control ctrl in Controls)
            {
                if (ctrl is Button btn)
                {
                    btn.BackColor = escuro ? Color.FromArgb(60, 60, 60) : Color.LightGray;
                    btn.ForeColor = texto;
                }
            }
        }

        // Modo radiano/graus
        private double AplicarTrigonometria(string funcao, double valor)
        {
            if (usarGraus)
                valor *= Math.PI / 180;

            return funcao switch
            {
                "sin" => Math.Sin(valor),
                "cos" => Math.Cos(valor),
                "tan" => Math.Tan(valor),
                _ => throw new NotImplementedException()
            };
        }

        // Validação visual da expressão
        private void txtExpressao_TextChanged(object? sender, EventArgs e)
        {
            try
            {
                string expr = txtExpressao!.Text;
                evaluator.Evaluate(expr);
                txtExpressao.BackColor = Color.DarkGreen;
            }
            catch
            {
                txtExpressao!.BackColor = Color.DarkRed;
            }
        }

        // Histórico persistente
        private void SalvarHistorico()
        {
            File.WriteAllLines("historico.txt", lstHistorico!.Items.Cast<string>());
        }

        private void CarregarHistorico()
        {
            if (File.Exists("historico.txt"))
            {
                var linhas = File.ReadAllLines("historico.txt");
                lstHistorico!.Items.AddRange(linhas);
            }
        }

        // Eventos de botões (a implementar)
        private void btnAnalisarFuncao_Click(object? sender, EventArgs e) => throw new NotImplementedException();
        private void btnPlotarFuncao_Click(object? sender, EventArgs e) => throw new NotImplementedException();
        private void btnSalvarImagem_Click(object? sender, EventArgs e) => throw new NotImplementedException();
        private void lstHistorico_DoubleClick(object? sender, EventArgs e) { /* implementar */ }
    }
}