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
            zedGraphControl1.GraphPane.Title.Text = "Фазовый портрет";
            zedGraphControl1.GraphPane.XAxis.Title.Text = "x";
            zedGraphControl1.GraphPane.YAxis.Title.Text = "ẋ";

            zedGraphControl1.GraphPane.Title.Text = "График ошибки";
            zedGraphControl2.GraphPane.XAxis.Title.Text = "Время";
            zedGraphControl2.GraphPane.YAxis.Title.Text = "Величина ошибки";

            timer1.Interval = 5;
            solutionPool = new SolutionPool(timer1);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < solutionPool.ActiveCount; i++)
            {
                solutionPool.activeSolutions[i].CurveDraw();
            }

            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double x0 = double.Parse(textBox_X0.Text, CultureInfo.InvariantCulture);
            double y0 = double.Parse(textBox_Y0.Text, CultureInfo.InvariantCulture);
            double omega = double.Parse(textBoxOmega.Text, CultureInfo.InvariantCulture);
            double gamma = double.Parse(textBoxGamma.Text, CultureInfo.InvariantCulture);
            double dt = double.Parse(textBoxdT.Text, CultureInfo.InvariantCulture);
            int N = int.Parse(textBoxN.Text, CultureInfo.InvariantCulture);

            solutionPool.Add(new Solution(zedGraphControl1, labelCurrentN0, labelFinalTime0, 500)).Solve(x0, y0, omega, gamma, dt, N, SolveMethod.Analytical);
            solutionPool.Add(new Solution(zedGraphControl1, labelCurrentN1, labelFinalTime1, 500)).Solve(x0, y0, omega, gamma, dt, N, SolveMethod.Euler);
            solutionPool.Add(new Solution(zedGraphControl1, labelCurrentN2, labelFinalTime2, 500)).Solve(x0, y0, omega, gamma, dt, N, SolveMethod.RungeKutta);
        }
    }
}
