using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CalculadoraCientifica
{
    public partial class Form1
    {
        private void CriarTecladoComTabela()
        {
            var teclado = new TableLayoutPanel
            {
                Location = new Point(10, 400),
                Size = new Size(320, 280),
                ColumnCount = 5,
                RowCount = 6,
                BackColor = Color.Transparent,
                AutoSize = true
            };

            for (int i = 0; i < 5; i++)
                teclado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            for (int i = 0; i < 6; i++)
                teclado.RowStyles.Add(new RowStyle(SizeType.Percent, 16.66F));

            string[] teclas = {
                "7", "8", "9", "/", "sin",
                "4", "5", "6", "*", "cos",
                "1", "2", "3", "-", "tan",
                "0", ".", "(", ")", "+",
                "log", "ln", "^", "√", "exp",
                "C", "←", "=", "", ""
            };

            foreach (var tecla in teclas)
            {
                if (string.IsNullOrWhiteSpace(tecla))
                {
                    teclado.Controls.Add(new Label());
                    continue;
                }

                var btn = new Button
                {
                    Text = tecla,
                    Dock = DockStyle.Fill,
                    BackColor = Color.FromArgb(60, 60, 60),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                };

                btn.Click += (s, e) =>
                {
                    string t = (s is Button b && b.Text != null) ? b.Text : "";

                    switch (t)
                    {
                        case "=":
                            try
                            {
                                string expr = txtExpressao?.Text?
                                    .Replace("√", "sqrt")
                                    .Replace("π", Math.PI.ToString(CultureInfo.InvariantCulture))
                                    .Replace("e", Math.E.ToString(CultureInfo.InvariantCulture))
                                    .Replace(",", ".") ?? "";

                                if (string.IsNullOrWhiteSpace(expr))
                                {
                                    MessageBox.Show("Digite uma expressão válida.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }

                                double resultado = evaluator.Evaluate(expr);
                                txtExpressao!.Text = resultado.ToString("G10", CultureInfo.InvariantCulture);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Erro na expressão: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            break;

                        case "C":
                            txtExpressao!.Text = "";
                            break;

                        case "←":
                            if (txtExpressao!.Text.Length > 0)
                                txtExpressao.Text = txtExpressao.Text[..^1];
                            break;

                        default:
                            txtExpressao!.Text += t;
                            break;
                    }
                };

                teclado.Controls.Add(btn);
            }

            Controls.Add(teclado);
        }
    }
}