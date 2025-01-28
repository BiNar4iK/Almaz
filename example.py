import numpy as np
import matplotlib.pyplot as plt
from matplotlib import rcParams

def lotka(x, p):
	dx = np.zeros(2)
	dx[0] = (p['alpha']- p['beta']*x[1]) * x[0]
	dx[1] = (-p['gamma'] + p['delta']*x[0]) * x[1]
	return dx

def lotka_rk4(x0, N, p):
	""" Решает Лотки-Вольтерра методом Рунге-Кутты 4-го порядка """
	x = x0
	series = [x0]
	for i in range(N-1):
		print(x)
		k1 = lotka(x, p)
		k2 = lotka(x+dt/2*k1, p)
		k3 = lotka(x+dt/2*k2, p)
		k4 = lotka(x+dt*k3, p)
		x = x + dt/6 * (k1 + 2*k2 + 2*k3 + k4)
		series.append(x)
	return np.array(series)

p = {'alpha': 0.9, 'beta': 0.1, 'gamma': 0.8, 'delta': 0.2}
dt = 0.25
x0 = [10., 21]
N = 200
rk4ser = lotka_rk4(x0, N, p)
print(len(rk4ser))
tser = np.linspace(0, dt*N, N, False)
plt.plot(rk4ser[:, 0], rk4ser[:, 1], color='gray')
#plt.plot(tser, rk4ser[:, 1], '--', color='black')
plt.show()