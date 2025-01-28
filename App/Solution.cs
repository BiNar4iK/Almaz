using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ZedGraph;

namespace App
{
    public enum SolveMethod
    {
        Analytical = 0,
        Euler = 1,
        RungeKutta = 2,
    }

    public unsafe class Solution : ISolution
    {
        Array X;
        Array Y;
        List<ISolution> attachedSolutions = new List<ISolution>();

        int currentNmin = 0;
        int currentNmax = 0;

        private int _currentN;

        public int currentN { get { return _currentN; } private set { _currentN = value; } }
        public int N { get; private set; } = 0;

        double time = 0;
        public int graphStep = 500;

        public bool Active { get; private set; } = false;
        public event Action<ISolution> isStartEvent;
        public event Action<ISolution> isFinishEvent;

        int id;
        public ZedGraphControl zedGraphControl { get; }

        System.Windows.Forms.Label labelTime;
        System.Windows.Forms.Label labelCurrentN;

        Thread thread;

        string title;
        Color color;

        SymbolType symbolType;

        public Solution(ZedGraphControl zedGraphControl, System.Windows.Forms.Label labelCurrentN, System.Windows.Forms.Label labelTime, string title, Color color, bool markers = false)
        {
            this.zedGraphControl = zedGraphControl;
            this.labelTime = labelTime;
            this.labelCurrentN = labelCurrentN;
            this.title = title;
            this.color = color;
            if (markers)
            {
                symbolType = SymbolType.Default;
            }
            else
            {
                symbolType = SymbolType.None;
            }
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
                        time = Logic.solveAnalytical(x0, y0, omega, gamma, dt, N, ref X.array, ref Y.array, ref _currentN);
                        this.labelTime.Text = Convert.ToString(time);
                        thread = null;
                        Active = false;
                        CurveDraw();
                        isFinishEvent.Invoke(this);
                    });

                    zedGraphControl.GraphPane.AddCurve(title, null, null, color, symbolType);

                    break;
                case SolveMethod.Euler:
                    thread = new Thread(() =>
                    {
                        time = Logic.solveEuler(x0, y0, omega, gamma, dt, N, ref X.array, ref Y.array, ref _currentN);
                        this.labelTime.Text = Convert.ToString(time);
                        thread = null;
                        Active = false;
                        CurveDraw();
                        isFinishEvent.Invoke(this);
                    });

                    zedGraphControl.GraphPane.AddCurve(title, null, null, color, symbolType);

                    break;
                case SolveMethod.RungeKutta:
                    thread = new Thread(() =>
                    {
                        time = Logic.solveRungeKutta(x0, y0, omega, gamma, dt, N, ref X.array, ref Y.array, ref _currentN);
                        this.labelTime.Text = Convert.ToString(time);
                        thread = null;
                        Active = false;
                        CurveDraw();
                        isFinishEvent.Invoke(this);
                    });

                    zedGraphControl.GraphPane.AddCurve(title, null, null, color, symbolType);

                    break;
            }

            id = zedGraphControl.GraphPane.CurveList.Count - 1;
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

        public void Dispose()
        {
            thread?.Abort();

            for (int i = 0; i < attachedSolutions.Count; i++)
            {
                if (attachedSolutions[i].Active) return;
            }

            X.Dispose(); Y.Dispose();
        }

        public void AttachSolution(ISolution solution)
        {
            attachedSolutions.Add(solution);
        }

        public double GetX(int i)
        {
            return X[i];
        }

        public double GetY(int i)
        {
            return Y[i];
        }

        public void Start()
        {
            thread.Start();

            Active = true;
            isStartEvent.Invoke(this);
        }
    }
}
