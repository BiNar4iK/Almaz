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

        List<Solution> solutions = new List<Solution>();
        public List<Solution> activeSolutions = new List<Solution>();

        System.Windows.Forms.Timer timer;

        public int Count => solutions.Count;
        public int ActiveCount => activeSolutions.Count;
        public SolutionPool(System.Windows.Forms.Timer timer)
        {
            this.timer = timer;
        }

        public Solution Add(Solution solution)
        {
            solution.isStartEvent += solutionStartHandler;
            solution.isFinishEvent += solutionFinishHandler;
            solutions.Add(solution);
            return solution;
        }

        public void Remove(Solution solution)
        {
            solution.isStartEvent -= solutionStartHandler;
            solution.isFinishEvent -= solutionFinishHandler;
            solutions.Remove(solution);
        }

        private void solutionStartHandler(Solution solution)
        {
            if (ActiveCount == 0)
            {
                solutionsStartEvent?.Invoke();
                timer.Start();
            }

            activeSolutions.Add(solution);
        }

        private void solutionFinishHandler(Solution solution)
        {
            activeSolutions.Remove(solution);

            if (ActiveCount == 0)
            {
                solutionsFinishEvent?.Invoke();
                timer.Stop();
            }

            solution.CurveDraw();

            solution.zedGraphControl.AxisChange();
            solution.zedGraphControl.Invalidate();

            solution.Dispose();
        }

        public void Dispose()
        {
            foreach (Solution pool in solutions) pool.Dispose();
        }
    }
}
