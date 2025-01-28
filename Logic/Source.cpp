#include "pch.h"
#include "framework.h"

void createArray(double*& pointer, int size)
{
    pointer = new double[size] {};
    /*for (int i = 0; i < size; i++)
    {
        pointer[i] = i;
    }*/
}

void cleanArray(double*& pointer)
{
	delete[] pointer;
}

double solveRungeKutta(double x0, double y0, double omega, double gamma, double dt, int N, double*& x_array, double*& y_array, int& currentN)
{
    double k11, k21, k31, k41;
    double k12, k22, k32, k42;

    double omega2 = omega * omega;

    x_array[0] = x0; y_array[0] = y0;

    double x1 = x0; double y1 = y0;
    double x2, y2;

    auto start = std::chrono::high_resolution_clock::now();

    for (int i = 1; i < N; i++)
    {
        k11 = y1;
        k21 = y1 + dt / 2 * k11;
        k31 = y1 + dt / 2 * k21;
        k41 = y1 + dt * k31;

        x2 = x1 + dt / 6 * (k11 + 2 * k21 + 2 * k31 + k41);

        k12 = -2 * gamma * y1 - omega2 * x1;
        k22 = -2 * gamma * (y1 + dt / 2 * k12) - omega2 * (x1 + dt / 2 * k12);
        k32 = -2 * gamma * (y1 + dt / 2 * k22) - omega2 * (x1 + dt / 2 * k22);
        k42 = -2 * gamma * (y1 + dt * k32) - omega2 * (x1 + dt * k32);

        y2 = y1 + dt / 6 * (k12 + 2 * k22 + 2 * k32 + k42);

        x_array[i] = x2; y_array[i] = y2;

        x1 = x2; y1 = y2;

        currentN = i;
    }

    auto end = std::chrono::high_resolution_clock::now();
    std::chrono::duration<double> duration = end - start;

    return duration.count();
}

double solveAnalytical(double x0, double y0, double omega0, double gamma, double dt, int N, double*& x_array, double*& y_array, int& currentN)
{
    double omega = std::sqrt(omega0 * omega0 - gamma * gamma);
    double C1 = (y0 + gamma * x0) / omega;
    double C2 = -x0 * omega;
    double C3 = C1 * omega;

    double S1, S2, E1;

    x_array[0] = x0; y_array[0] = y0;

    auto start = std::chrono::high_resolution_clock::now();

    for (int i = 1; i < N; i++)
    {
        S1 = std::sin(omega * i * dt);
        S2 = std::cos(omega * i * dt);
        E1 = std::exp(-gamma * i * dt);

        x_array[i] = E1 * (x0 * S2 + C1 * S1);
        y_array[i] = -gamma * x_array[i] + E1 * (C2 * S1 + C3 * S2);

        currentN = i;
    }

    auto end = std::chrono::high_resolution_clock::now();
    std::chrono::duration<double> duration = end - start;

    return duration.count();
}

double solveEuler(double x0, double y0, double omega, double gamma, double dt, int N, double*& x_array, double*& y_array, int& currentN)
{
    x_array[0] = x0; y_array[0] = y0;

    double omega2 = omega * omega;

    double x1 = x0; double y1 = y0;
    double x2, y2;

    auto start = std::chrono::high_resolution_clock::now();

    for (int i = 1; i < N; i++)
    {
        x2 = x1 + y1 * dt;
        y2 = y1 - (2 * gamma * y1 + omega2 * x1) * dt;

        x_array[i] = x2; y_array[i] = y2;

        x1 = x2; y1 = y2;

        currentN = i;
    }

    auto end = std::chrono::high_resolution_clock::now();
    std::chrono::duration<double> duration = end - start;

    return duration.count();
}

//double solveRungeKutta(double x0, double y0, double omega, double gamma, double dt, int N, double*& x_array, double*& y_array, int& currentN)
//{
//    double k11, k21, k31, k41;
//    double k12, k22, k32, k42;
//
//    double omega2 = omega * omega;
//
//    x_array[0] = x0; y_array[0] = y0;
//
//    auto start = std::chrono::high_resolution_clock::now();
//
//    for (int i = 0; i < N - 1; i++)
//    {
//        k11 = y_array[i];
//        k21 = y_array[i] + dt / 2 * k11;
//        k31 = y_array[i] + dt / 2 * k21;
//        k41 = y_array[i] + dt * k31;
//
//        x_array[i + 1] = x_array[i] + dt / 6 * (k11 + 2 * k21 + 2 * k31 + k41);
//
//        k12 = -2 * gamma * y_array[i] - omega2 * x_array[i];
//        k22 = -2 * gamma * (y_array[i] + dt / 2 * k12) - omega2 * (x_array[i] + dt / 2 * k12);
//        k32 = -2 * gamma * (y_array[i] + dt / 2 * k22) - omega2 * (x_array[i] + dt / 2 * k22);
//        k42 = -2 * gamma * (y_array[i] + dt * k32) - omega2 * (x_array[i] + dt * k32);
//
//        y_array[i + 1] = y_array[i] + dt / 6 * (k12 + 2 * k22 + 2 * k32 + k42);
//
//        currentN = i + 1;
//    }
//
//    auto end = std::chrono::high_resolution_clock::now();
//    std::chrono::duration<double> duration = end - start;
//
//    return duration.count();
//}

//double solveRungeKutta(double x0, double y0, double omega, double gamma, double dt, int N, double*& x_array, double*& y_array, int& currentN)
//{
//    double omega2 = omega * omega;
//    x_array[0] = x0; y_array[0] = y0;
//
//    auto start = std::chrono::high_resolution_clock::now();
//
//    for (int i = 0; i < N-1; i++)
//    {
//        x_array[i+1] = x_array[i] + dt / 6 * (y_array[i] + 2 * y_array[i] + dt / 2 * x_array[i] + 2 * y_array[i] + dt / 2 * y_array[i] + dt / 2 * y_array[i] + y_array[i] + dt * y_array[i] + dt / 2 * y_array[i] + dt / 2 * y_array[i]);
//
//        y_array[i+1] = y_array[i] + dt / 6 * (-2 * gamma * (y_array[i] + dt / 2 * -2 * gamma * y_array[i] - omega2 * x_array[i]) - omega2 * (x_array[i] + dt / 2 * -2 * gamma * y_array[i] - omega2 * x_array[i]) + 2 * -2 * gamma * (y_array[i] + dt / 2 * -2 * gamma * (y_array[i] + dt / 2 * -2 * gamma * y_array[i] - omega2 * x_array[i]) - omega2 * (x_array[i] + dt / 2 * -2 * gamma * y_array[i] - omega2 * x_array[i])) - omega2 * (x_array[i] + dt / 2 * -2 * gamma * (y_array[i] + dt / 2 * -2 * gamma * y_array[i] - omega2 * x_array[i]) - omega2 * (x_array[i] + dt / 2 * -2 * gamma * y_array[i] - omega2 * x_array[i])) + 2 * -2 * gamma * (y_array[i] + dt / 2 * -2 * gamma * (y_array[i] + dt / 2 * -2 * gamma * y_array[i] - omega2 * x_array[i]) - omega2 * (x_array[i] + dt / 2 * -2 * gamma * y_array[i] - omega2 * x_array[i])) - omega2 * (x_array[i] + dt / 2 * -2 * gamma * (y_array[i] + dt / 2 * -2 * gamma * y_array[i] - omega2 * x_array[i]) - omega2 * (x_array[i] + dt / 2 * -2 * gamma * y_array[i] - omega2 * x_array[i])) - 2 * gamma * (y_array[i] + dt * -2 * gamma * (y_array[i] + dt / 2 * -2 * gamma * (y_array[i] + dt / 2 * -2 * gamma * y_array[i] - omega2 * x_array[i]) - omega2 * (x_array[i] + dt / 2 * -2 * gamma * y_array[i] - omega2 * x_array[i])) - omega2 * (x_array[i] + dt / 2 * -2 * gamma * (y_array[i] + dt / 2 * -2 * gamma * y_array[i] - omega2 * x_array[i]) - omega2 * (x_array[i] + dt / 2 * -2 * gamma * y_array[i] - omega2 * x_array[i]))) - omega2 * (x_array[i] + dt * -2 * gamma * (y_array[i] + dt / 2 * -2 * gamma * (y_array[i] + dt / 2 * -2 * gamma * y_array[i] - omega2 * x_array[i]) - omega2 * (x_array[i] + dt / 2 * -2 * gamma * y_array[i] - omega2 * x_array[i])) - omega2 * (x_array[i] + dt / 2 * -2 * gamma * (y_array[i] + dt / 2 * -2 * gamma * y_array[i] - omega2 * x_array[i]) - omega2 * (x_array[i] + dt / 2 * -2 * gamma * y_array[i] - omega2 * x_array[i]))));
//
//        currentN = i + 1;
//    }
//
//    auto end = std::chrono::high_resolution_clock::now();
//    std::chrono::duration<double> duration = end - start;
//
//    return duration.count();
//}