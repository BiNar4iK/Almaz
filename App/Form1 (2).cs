using System;
using System.Globalization;
using System.Windows.Forms;

namespace App
{
    public partial class Form1 : Form
    {
        SolutionPool solutionPool;

        public Form1()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            // Инициализация графика
            zedGraphControl1.GraphPane.Title.Text = "Фазоый портрет";
            zedGraphControl1.GraphPane.XAxis.Title.Text = "x";
            zedGraphControl1.GraphPane.YAxis.Title.Text = "ẋ";

            zedGraphControl2.GraphPane.Title.Text = "График ошибки";
            zedGraphControl2.GraphPane.YAxis.Title.Text = "Величина ошибки";
            zedGraphControl2.GraphPane.XAxis.Title.Text = "Время";

            timer1.Interval = 10; // Интервал в миллисекундах (1 секунда)
            timer1.Tick += Timer_Tick;

            solutionPool = new SolutionPool(timer1);

            this.FormClosed += new FormClosedEventHandler(MyForm_FormClosed);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < solutionPool.ActiveCount; i++)
            {
                solutionPool.activeSolutions[i].CurveDraw();
            }

            // Обновление графика
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //solve_test();

            double x0 = double.Parse(textBox_X0.Text, CultureInfo.InvariantCulture);
            double y0 = double.Parse(textBox_Y0.Text, CultureInfo.InvariantCulture);
            double omega = double.Parse(textBoxOmega.Text, CultureInfo.InvariantCulture);
            double gamma = double.Parse(textBoxGamma.Text, CultureInfo.InvariantCulture);
            double dt = double.Parse(textBoxdT.Text, CultureInfo.InvariantCulture);
            int N = int.Parse(textBoxN.Text, CultureInfo.InvariantCulture);

            solutionPool.Add(new Solution(zedGraphControl1, labelCurrentN1, labelFinalTime1)).Solve(x0, y0, omega, gamma, dt, N, SolveMethod.Euler);

            x0 += 0.01; y0 += 0.01;

            solutionPool.Add(new Solution(zedGraphControl1, labelCurrentN2, labelFinalTime2)).Solve(x0, y0, omega, gamma, dt, N, SolveMethod.RungeKutta);
        }

        //private unsafe void solve_test()
        //{
        //    double x0 = 0.5;
        //    double y0 = 0.5;
        //    double omega = 0.2;
        //    double gamma = 0.1;
        //    double dt = 0.00001;
        //    N = 20_000_000;

        //    zedGraphControl1.GraphPane.AddCurve("Рунге-Кутта", new[] { x0 }, new[] { y0 }, System.Drawing.Color.Red, SymbolType.None);

        //    X = new Array(N);
        //    Y = new Array(N);

        //    thread = new Thread(() =>
        //    {
        //        time = Logic.solveRungeKutta(x0, y0, omega, gamma, dt, N, ref X.array, ref Y.array, ref currentN);
        //        labelTime.Text = Convert.ToString(time);
        //    });

        //    thread.Start();


        //    return;
        //}

        private void MyForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            solutionPool.Dispose();
        }
    }
}
