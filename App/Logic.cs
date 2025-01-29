using System;
using System.Runtime.InteropServices;

namespace App
{
    public class Logic
    {
        [DllImport("Logic.dll")]
        public static extern unsafe bool createArray(out double* pointer, int size);

        [DllImport("Logic.dll")]
        public static extern unsafe bool cleanArray(ref double* pointer);

        [DllImport("Logic.dll")]
        public static extern unsafe double solveAnalytical(double x0, double y0, double omega0, double gamma, double dt, int N, ref double* x_array, ref double* y_array, ref int currentN);

        [DllImport("Logic.dll")]
        public static extern unsafe double solveEuler(double x0, double y0, double omega, double gamma, double dt, int N, ref double* x_array, ref double* y_array, ref int currentN);

        [DllImport("Logic.dll")]
        public static extern unsafe double solveRungeKutta(double x0, double y0, double omega, double gamma, double dt, int N, ref double* x_array, ref double* y_array, ref int currentN);
    }
}
