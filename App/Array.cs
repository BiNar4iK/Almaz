using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public unsafe class Array : IDisposable
    {
        int Capacity { get; }
        int Size { get; }

        public double* array;

        public bool Alive;

        public Array(int Capacity)
        {
            this.Capacity = Capacity;
            Alive = Logic.createArray(out array, Capacity);
        }

        // Индексатор для доступа к элементам массива
        public double this[int index]
        {
            get
            {
                if (index < 0 || index >= Capacity || !Alive)
                    throw new IndexOutOfRangeException("Индекс вне диапазона.");
                return array[index];
            }
            set
            {
                if (index < 0 || index >= Capacity || !Alive)
                    throw new IndexOutOfRangeException("Индекс вне диапазона.");
                array[index] = value;
            }
        }

        private bool disposed = false; // Флаг для отслеживания состояния

        // Метод Dispose для освобождения ресурсов
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); // Предотвращает вызов деструктора
        }

        // Защищенный метод для освобождения ресурсов
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Освобождение управляемых ресурсов (если есть)
                }

                // Освобождение неуправляемых ресурсов (если есть)
                Alive = !Logic.cleanArray(ref array);

                disposed = true;
            }
        }

        // Деструктор (необязательно)
        ~Array()
        {
            Dispose(false);
        }
    }
}
