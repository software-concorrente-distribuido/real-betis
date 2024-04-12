using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    public static Queue<int> buffer = new Queue<int>();
    public static int BUFFER_SIZE = 5;
    public static int OBJECT_QUANTITY = 30;
    public static object lockObject = new object();

    static void Main(string[] args)
    {
        Thread producerThread = new Thread(Producer); // Cria as threads com os métodos que eles irão executar
        Thread consumerThread = new Thread(Consumer);

        producerThread.Start(); // Liga as threads
        consumerThread.Start();

        producerThread.Join(); // Faz com que o programa espere as threads terminarem sua execução
        consumerThread.Join();
    }

    static void Producer()
    {
        for (int i = 0; i < OBJECT_QUANTITY; i++)
        {
            lock (lockObject) // (MUTEX) Faz lock em um objeto dummy por questões de segurança e performance
            {
                while (buffer.Count >= BUFFER_SIZE)
                {
                    Monitor.Wait(lockObject); // A thread que está com o lock do objeto "solta" o lock e espera até que a thread seja notificada por outra thread sobre uma mudança no objeto
                }

                buffer.Enqueue(i);
                Console.WriteLine($"Produtor: {i}");
                Monitor.PulseAll(lockObject); // A thread que possui o lock no objeto especificado notifica as outras threads que estão aguardando de que houve uma mudança no estado do objeto e o lock já pode ser adquirido por elas
            }

            Thread.Sleep(100);
        }
    }

    static void Consumer()
    {
        for (int i = 0; i < OBJECT_QUANTITY; i++)
        {
            lock (lockObject)
            {
                while (buffer.Count == 0)
                {
                    Monitor.Wait(lockObject);
                }

                int item = buffer.Dequeue();
                Console.WriteLine($"Consumidor: {item}");
                Monitor.PulseAll(lockObject);
            }

            Thread.Sleep(300);
        }
    }
}