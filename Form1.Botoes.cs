using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculadoraCientifica
{
    public partial class Form1
    {
        private static Button CriarBotaoPersonalizado(string texto, EventHandler evento)
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
    }
}