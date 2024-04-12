namespace AlgoritmosCSD {
    internal class Algoritmo1 {
        private static char _vez;
        static void Main(string[] args) {
            Console.WriteLine("INICIANDO ALGORITMO 1\n\n");
            _vez = 'A';
            Thread t1 = new Thread(ProcedureA);
            Thread t2 = new Thread(ProcedureB);
            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();
        }

        private static void ProcedureA() {
            while (true) {
                while (_vez == 'B') {
                    // Não faz nada
                }
                Console.WriteLine("=========== Seção crítica A ===========");
                Task.Delay(5000).Wait();
                _vez = 'B';
                Console.WriteLine("Processamento da seção não crítica A");
                Task.Delay(5000).Wait();
            }
        }

        private static void ProcedureB() {
            while (true) {
                while (_vez == 'A') {
                    // Não faz nada
                }
                Console.WriteLine("=========== Seção crítica B ===========");
                Task.Delay(5000).Wait();
                _vez = 'A';
                Console.WriteLine("Processamento da seção não crítica B");
                Task.Delay(5000).Wait();
            }
        }
    }
}
