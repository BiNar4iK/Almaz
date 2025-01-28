using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;

namespace App
{
    public interface ISolution : IDisposable
    {
        event Action<ISolution> isStartEvent;
        event Action<ISolution> isFinishEvent;

        int currentN {  get; }
        int N { get; }

        bool Active { get; }
        void Start();
        void CurveDraw();
        double GetX(int i);
        double GetY(int i);
        void AttachSolution(ISolution solution);

        ZedGraphControl zedGraphControl { get; }
    }
}
