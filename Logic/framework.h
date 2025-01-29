#pragma once

//#define WIN32_LEAN_AND_MEAN             // Исключите редко используемые компоненты из заголовков Windows
// Файлы заголовков Windows
//#include <windows.h>

extern "C" __declspec(dllexport) bool createArray(double*& pointer, int size);

extern "C" __declspec(dllexport) bool cleanArray(double*& pointer);

extern "C" __declspec(dllexport) double solveAnalytical(double x0, double y0, double omega0, double gamma, double dt, int N, double*& x_array, double*& y_array, int& currentN);

extern "C" __declspec(dllexport) double solveEuler(double x0, double y0, double omega, double gamma, double dt, int N, double*& x_array, double*& y_array, int& currentN);

extern "C" __declspec(dllexport) double solveRungeKutta(double x0, double y0, double omega, double gamma, double dt, int N, double*& x_array, double*& y_array, int& currentN);