using System;
using System.Collections.Generic;
using System.Drawing;
using ZedGraph;

namespace App
{
    public class DifferentSolution : ISolution
    {
        public ZedGraphControl zedGraphControl { get; }

        public event Action<ISolution> isStartEvent;
        public event Action<ISolution> isFinishEvent;
        public bool ThreadActive { get; private set; } = true;

        ISolution sourceSolution;
        ISolution targetSolution;
        List<ISolution> attachedSolutions = new List<ISolution>();

        int currentNmin = 0;
        int currentNmax = 0;

        int id;

        public int currentN { get; private set; } = 0;
        public int N { get; private set; } = 0;

        private bool isDrawing = false;

        public int graphStep = 200;
        string title;
        Color color;
        SymbolType symbolType;

        public DifferentSolution(ISolution sourceSolution, ISolution targetSolution, ZedGraphControl zedGraphControl, string title, Color color, bool markers = false)
        {
            sourceSolution.AttachSolution(this);
            targetSolution.AttachSolution(this);

            this.sourceSolution = sourceSolution;
            this.targetSolution = targetSolution;
            this.zedGraphControl = zedGraphControl;
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

            if (sourceSolution.N == targetSolution.N)
            {
                N = sourceSolution.N;
            }
            else if (sourceSolution.N > targetSolution.N)
            {
                N = targetSolution.N;
            }
        }

        public void CurveDraw()
        {
            if (!isDrawing)
            {
                isDrawing = true;

                if (sourceSolution.currentN > targetSolution.currentN)
                {
                    currentNmax = targetSolution.currentN;
                }
                else
                {
                    currentNmax = sourceSolution.currentN;
                }

                for (int i = currentNmin; i < currentNmax; i += graphStep)
                {
                    zedGraphControl.GraphPane.CurveList[id].AddPoint(i, Norma(i));
                }

                currentNmin = currentNmax;

                if (currentNmax == N)
                {
                    ThreadActive = false;
                    isFinishEvent.Invoke(this);
                }

                isDrawing = false;
            }
        }

        private double DifferentX(int i)
        {
            return Math.Abs(sourceSolution.GetX(i) - targetSolution.GetX(i));
        }

        private double DifferentY(int i)
        {
            return Math.Abs(sourceSolution.GetX(i) - targetSolution.GetX(i));
        }

        private double Norma(int i)
        {
            return Math.Sqrt(DifferentX(i) * DifferentX(i) + DifferentY(i) * DifferentY(i));
        }

        public void Dispose()
        {
            sourceSolution.Dispose();
            targetSolution.Dispose();
        }

        public void AttachSolution(ISolution solution)
        {
            attachedSolutions.Add(solution);
        }

        public double GetX(int i)
        {
            throw new NotImplementedException();
        }

        public double GetY(int i)
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            zedGraphControl.GraphPane.AddCurve(title, null, null, color, symbolType);
            id = zedGraphControl.GraphPane.CurveList.Count - 1;

            isStartEvent?.Invoke(this);
        }
    }
}
