import matplotlib.pyplot as plt

# Функция для чтения данных из файла
def read_data_from_file(filename):
    x = []
    y = []
    
    with open(filename, 'r') as file:
        for line in file:
            values = line.split()
            if len(values) == 2:
                x.append(float(values[0]))
                y.append(float(values[1]))
    
    return x, y

# Чтение данных из файла
filename = 'data.txt'
x, y = read_data_from_file(filename)

# Создание графика
plt.figure(figsize=(10, 6))
plt.plot(x, y, color='b')

# Настройка графика
plt.title('График данных из файла')
plt.xlabel('X')
plt.ylabel('Y')
plt.grid()
plt.axhline(0, color='black', linewidth=0.5, ls='--')
plt.axvline(0, color='black', linewidth=0.5, ls='--')

# Показать график
plt.show()
