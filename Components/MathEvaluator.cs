using System;
using System.Collections.Generic;
using System.Globalization;

namespace CalculadoraCientifica.Components
{
    /// <summary>
    /// Avalia expressões matemáticas usando o algoritmo Shunting Yard e RPN.
    /// Suporta operadores e funções como sin, cos, tan, log, ln, sqrt, exp.
    /// </summary>
    public class MathEvaluator
    {
        private static readonly Dictionary<string, int> Precedencia = new()
        {
            { "+", 1 }, { "-", 1 },
            { "*", 2 }, { "/", 2 },
            { "^", 3 }
        };

        private static readonly HashSet<string> Funcoes = new()
        {
            "sin", "cos", "tan", "log", "ln", "sqrt", "exp"
        };
        private MathEvaluator? evaluator;

        public MathEvaluator(MathEvaluator evaluator)
        {
            this.evaluator = evaluator;
        }

        public MathEvaluator()
        {
        }

        /// <summary>
        /// Avalia uma expressão matemática em formato string.
        /// </summary>
        public double Evaluate(string expr)
        {
            var tokens = Tokenizar(expr);
            var rpn = ConverterParaRPN(tokens);
            return AvaliarRPN(rpn);
        }

        /// <summary>
        /// Converte a expressão em tokens legíveis.
        /// </summary>
        private List<string> Tokenizar(string expr)
        {
            var tokens = new List<string>();
            string numero = "";

            for (int i = 0; i < expr.Length; i++)
            {
                char c = expr[i];

                if (char.IsDigit(c) || c == '.')
                {
                    numero += c;
                }
                else
                {
                    if (numero != "")
                    {
                        tokens.Add(numero);
                        numero = "";
                    }

                    if (char.IsLetter(c))
                    {
                        string func = c.ToString();
                        while (i + 1 < expr.Length && char.IsLetter(expr[i + 1]))
                        {
                            func += expr[++i];
                        }
                        tokens.Add(func);
                    }
                    else if ("+-*/^()".Contains(c))
                    {
                        tokens.Add(c.ToString());
                    }
                }
            }

            if (numero != "")
                tokens.Add(numero);

            return tokens;
        }

        /// <summary>
        /// Converte os tokens para notação pós-fixada (RPN).
        /// </summary>
        private List<string> ConverterParaRPN(List<string> tokens)
        {
            var output = new List<string>();
            var operadores = new Stack<string>();

            foreach (var token in tokens)
            {
                if (double.TryParse(token, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                {
                    output.Add(token);
                }
                else if (Funcoes.Contains(token))
                {
                    operadores.Push(token);
                }
                else if (Precedencia.ContainsKey(token))
                {
                    while (operadores.Count > 0 &&
                           Precedencia.TryGetValue(operadores.Peek(), out int precTopo) &&
                           precTopo >= Precedencia[token])
                    {
                        output.Add(operadores.Pop());
                    }
                    operadores.Push(token);
                }
                else if (token == "(")
                {
                    operadores.Push(token);
                }
                else if (token == ")")
                {
                    while (operadores.Count > 0 && operadores.Peek() != "(")
                    {
                        output.Add(operadores.Pop());
                    }
                    operadores.Pop(); // Remove "("
                    if (operadores.Count > 0 && Funcoes.Contains(operadores.Peek()))
                    {
                        output.Add(operadores.Pop());
                    }
                }
            }

            while (operadores.Count > 0)
                output.Add(operadores.Pop());

            return output;
        }

        /// <summary>
        /// Avalia a expressão em RPN e retorna o resultado.
        /// </summary>
        private double AvaliarRPN(List<string> rpn)
        {
            var pilha = new Stack<double>();

            foreach (var token in rpn)
            {
                if (double.TryParse(token, NumberStyles.Any, CultureInfo.InvariantCulture, out double num))
                {
                    pilha.Push(num);
                }
                else if (Precedencia.ContainsKey(token))
                {
                    double b = pilha.Pop();
                    double a = pilha.Pop();
                    pilha.Push(token switch
                    {
                        "+" => a + b,
                        "-" => a - b,
                        "*" => a * b,
                        "/" => a / b,
                        "^" => Math.Pow(a, b),
                        _ => throw new Exception("Operador inválido")
                    });
                }
                else if (Funcoes.Contains(token))
                {
                    double x = pilha.Pop();
                    pilha.Push(token switch
                    {
                        "sin" => Math.Sin(x),
                        "cos" => Math.Cos(x),
                        "tan" => Math.Tan(x),
                        "log" => Math.Log10(x),
                        "ln" => Math.Log(x),
                        "sqrt" => Math.Sqrt(x),
                        "exp" => Math.Exp(x),
                        _ => throw new Exception("Função inválida")
                    });
                }
            }

            return pilha.Pop();
        }
    }
}