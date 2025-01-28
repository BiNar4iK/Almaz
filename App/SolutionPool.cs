using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;

namespace App
{
    public class SolutionPool : IDisposable
    {
        public event Action solutionsStartEvent;
        public event Action solutionsFinishEvent;

        List<ISolution> solutions = new List<ISolution>();
        public List<ISolution> activeSolutions = new List<ISolution>();

        System.Windows.Forms.Timer timer;

        public int Count => solutions.Count;
        public int ActiveCount => activeSolutions.Count;
        public SolutionPool(System.Windows.Forms.Timer timer)
        {
            this.timer = timer;
        }

        public ISolution Add(ISolution solution)
        {
            solution.isStartEvent += solutionStartHandler;
            solution.isFinishEvent += solutionFinishHandler;
            solutions.Add(solution);
            return solution;
        }

        public void Remove(ISolution solution)
        {
            solution.isStartEvent -= solutionStartHandler;
            solution.isFinishEvent -= solutionFinishHandler;
            solutions.Remove(solution);
        }

        private void solutionStartHandler(ISolution solution)
        {
            if (ActiveCount == 0)
            {
                solutionsStartEvent?.Invoke();
                timer.Start();
            }

            activeSolutions.Add(solution);
        }

        private void solutionFinishHandler(ISolution solution)
        {
            activeSolutions.Remove(solution);

            if (ActiveCount == 0)
            {
                solutionsFinishEvent?.Invoke();
                timer.Stop();
                Clear();
            }

            solution.zedGraphControl.AxisChange();
            solution.zedGraphControl.Invalidate();

            //solution.Dispose();
        }

        public ISolution this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    throw new IndexOutOfRangeException("Индекс вне диапазона.");
                return solutions[index];
            }
            set
            {
                if (index < 0 || index >= Count)
                    throw new IndexOutOfRangeException("Индекс вне диапазона.");
                solutions[index] = value;
            }
        }

        public void Dispose()
        {
            foreach (ISolution pool in solutions) pool.Dispose();
        }

        public void Clear()
        {
            Dispose();

            foreach (ISolution solution in solutions)
            {
                solution.isStartEvent -= solutionStartHandler;
                solution.isFinishEvent -= solutionFinishHandler;
            }
            solutions.Clear();

            activeSolutions.Clear();
        }
    }
}
