#include <iostream>
#include <fstream>
#include <chrono>

int main()
{
    double k11, k21, k31, k41;
    double k12, k22, k32, k42;

    double x0 = 0.5;
    double y0 = 0.5;
    double omega = 0.2;
    double gamma = 0.1;

    double dt = 0.001;
    int N = 50'000'000;

    double* x_array = new double[N];
    double* y_array = new double[N];

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

        k12 = -2 * gamma * y1 - omega * x1;
        k22 = -2 * gamma * (y1 + dt / 2 * k12) - omega * (x1 + dt / 2 * k12);
        k32 = -2 * gamma * (y1 + dt / 2 * k22) - omega * (x1 + dt / 2 * k22);
        k42 = -2 * gamma * (y1 + dt * k32) - omega * (x1 + dt * k32);

        y2 = y1 + dt / 6 * (k12 + 2 * k22 + 2 * k32 + k42);

        x_array[i] = x2; y_array[i] = y2;

        x1 = x2; y1 = y2;
    }

    auto end = std::chrono::high_resolution_clock::now();
    std::chrono::duration<double> duration = end - start;
    std::cout << "Execution time: " << duration.count() << " seconds" << std::endl;
/*
    std::fstream file("data.txt", std::ios_base::out);

    for (int i = 0; i < N; i++)
    {
        file << x_array[i] << ' ' << y_array[i] << std::endl;
    }

    file.close();*/
}