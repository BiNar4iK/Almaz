using System;
using System.Drawing;
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

            zedGraphControl2.GraphPane.Title.Text = "График ошибки";
            zedGraphControl2.GraphPane.XAxis.Title.Text = "Время";
            zedGraphControl2.GraphPane.YAxis.Title.Text = "Величина ошибки";

            timer1.Interval = 50;
            solutionPool = new SolutionPool(timer1);
            solutionPool.solutionsFinishEvent += SolutionsFinishHandler;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < solutionPool.ActiveCount; i++)
            {
                solutionPool.activeSolutions[i].CurveDraw();
            }

            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();

            zedGraphControl2.AxisChange();
            zedGraphControl2.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Solve();
        }

        private void Solve()
        {
            int GraphStep1 = int.Parse(textBoxGraphStep1.Text);
            int GraphStep2 = int.Parse(textBoxGraphStep2.Text);

            bool marker1 = checkBox1.Checked;
            bool marker2 = checkBox2.Checked;

            double x0 = double.Parse(textBox_X0.Text, CultureInfo.InvariantCulture);
            double y0 = double.Parse(textBox_Y0.Text, CultureInfo.InvariantCulture);
            double omega = double.Parse(textBoxOmega.Text, CultureInfo.InvariantCulture);
            double gamma = double.Parse(textBoxGamma.Text, CultureInfo.InvariantCulture);
            double dt = double.Parse(textBoxdT.Text, CultureInfo.InvariantCulture);
            int N = int.Parse(textBoxN.Text, CultureInfo.InvariantCulture);

            Solution solution1, solution2, solution3;

            if (checkBoxAnaliticPh.Checked)
            {
                solution1 = new Solution(zedGraphControl1, labelCurrentN0, labelFinalTime0, "Аналитическое", Color.Green, marker1);
                solution1.Solve(x0, y0, omega, gamma, dt, N, SolveMethod.Analytical);
                solution1.graphStep = GraphStep1;
                solutionPool.Add(solution1);
            }
            else
                solution1 = null;

            if (checkBoxEulerFh.Checked)
            {
                solution2 = new Solution(zedGraphControl1, labelCurrentN1, labelFinalTime1, "Эйлер", Color.Blue, marker1);
                solution2.Solve(x0, y0, omega, gamma, dt, N, SolveMethod.Euler);
                solution2.graphStep = GraphStep1;
                solutionPool.Add(solution2);
            }
            else
                solution2 = null;

            if (checkBoxRuKuPh.Checked)
            {
                solution3 = new Solution(zedGraphControl1, labelCurrentN2, labelFinalTime2, "Рунге-Кутта", Color.Red, marker1);
                solution3.Solve(x0, y0, omega, gamma, dt, N, SolveMethod.RungeKutta);
                solution3.graphStep = GraphStep1;
                solutionPool.Add(solution3);
            }
            else
                solution3 = null;

            if (checkBoxEulerErr.Checked && solution1 != null && solution2 != null)
            {
                DifferentSolution differentSolution1 = new DifferentSolution(solution1, solution2, zedGraphControl2, "Эйлер", Color.Blue, marker2);
                differentSolution1.graphStep = GraphStep2;
                solutionPool.Add(differentSolution1);
            }

            if (checkBoxRuKuErr.Checked && solution1 != null && solution3 != null)
            {
                DifferentSolution differentSolution2 = new DifferentSolution(solution1, solution3, zedGraphControl2, "Рунге-Кутта", Color.Red, marker2);
                differentSolution2.graphStep = GraphStep2;
                solutionPool.Add(differentSolution2);
            }

            for (int i = 0; i < solutionPool.Count; i++)
            {
                solutionPool[i].Start();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            solutionPool.Clear();
            zedGraphControl1.GraphPane.CurveList.Clear();
            zedGraphControl2.GraphPane.CurveList.Clear();

            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();

            zedGraphControl2.AxisChange();
            zedGraphControl2.Invalidate();
        }

        private void SolutionsFinishHandler()
        {
        }
    }
}
