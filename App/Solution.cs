using System;
using System.Threading;
using ZedGraph;

namespace App
{
    public enum SolveMethod
    {
        Analytical = 0,
        Euler = 1,
        RungeKutta = 2,
    }

    public unsafe class Solution : IDisposable
    {
        Array X;
        Array Y;

        int currentNmin = 1;
        int currentNmax = 1;
        int currentN = 1;
        int N = 1;
        double time = 0;
        public int graphStep = 1;

        public event Action<Solution> isStartEvent;
        public event Action<Solution> isFinishEvent;

        int id;
        public ZedGraphControl zedGraphControl;
        System.Windows.Forms.Label labelTime;
        System.Windows.Forms.Label labelCurrentN;

        Thread thread;

        public Solution(ZedGraphControl zedGraphControl, System.Windows.Forms.Label labelCurrentN, System.Windows.Forms.Label labelTime, int graphStep)
        {
            this.zedGraphControl = zedGraphControl;
            this.labelTime = labelTime;
            this.labelCurrentN = labelCurrentN;
            this.graphStep = graphStep;
        }

        public void Solve(double x0, double y0, double omega, double gamma, double dt, int N, SolveMethod solveMethod)
        {
            this.N = N;
            X = new Array(N);
            Y = new Array(N);

            switch (solveMethod)
            {
                case SolveMethod.Analytical:
                    thread = new Thread(() =>
                    {
                        time = Logic.solveAnalytical(x0, y0, omega, gamma, dt, N, ref X.array, ref Y.array, ref currentN);
                        this.labelTime.Text = Convert.ToString(time);
                        thread = null;
                        isFinishEvent.Invoke(this);
                    });

                    zedGraphControl.GraphPane.AddCurve("Аналитическое", new[] { x0 }, new[] { y0 }, System.Drawing.Color.Green, SymbolType.None);

                    break;
                case SolveMethod.Euler:
                    thread = new Thread(() =>
                    {
                        time = Logic.solveEuler(x0, y0, omega, gamma, dt, N, ref X.array, ref Y.array, ref currentN);
                        this.labelTime.Text = Convert.ToString(time);
                        thread = null;
                        isFinishEvent.Invoke(this);
                    });

                    zedGraphControl.GraphPane.AddCurve("Эйлер", new[] { x0 }, new[] { y0 }, System.Drawing.Color.Blue, SymbolType.None);

                    break;
                case SolveMethod.RungeKutta:
                    thread = new Thread(() =>
                    {
                        time = Logic.solveRungeKutta(x0, y0, omega, gamma, dt, N, ref X.array, ref Y.array, ref currentN);
                        this.labelTime.Text = Convert.ToString(time);
                        thread = null;
                        isFinishEvent.Invoke(this);
                    });

                    zedGraphControl.GraphPane.AddCurve("Рунге-Кутта", new[] { x0 }, new[] { y0 }, System.Drawing.Color.Red, SymbolType.None);

                    break;
            }

            id = zedGraphControl.GraphPane.CurveList.Count - 1;

            thread.Start();

            isStartEvent.Invoke(this);
        }

        public void ErrorDraw(Solution analyticalSolution, ZedGraphControl zedGraphControl)
        {

        }

        public void CurveDraw()
        {
            currentNmax = currentN;

            for (int i = currentNmin; i < currentNmax; i += graphStep)
            {
                zedGraphControl.GraphPane.CurveList[id].AddPoint(X[i], Y[i]);
            }

            currentNmin = currentNmax;

            labelCurrentN.Text = Convert.ToString(currentN+1);
        }

        public void CurveDrawError()
        {

        }

        public void Dispose()
        {
            thread?.Abort();
            X.Dispose(); Y.Dispose();
        }
    }
}
