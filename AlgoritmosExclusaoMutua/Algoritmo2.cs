namespace AlgoritmosCSD {
    internal class Algoritmo2 {
        private static bool _ca;
        private static bool _cb;
        static void Main(string[] args) {
            Console.WriteLine("INICIANDO ALGORITMO 2\n\n");
            _ca = false;
            _cb = false;
            Thread t1 = new Thread(ProcedureA);
            Thread t2 = new Thread(ProcedureB);
            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();
        }

        private static void ProcedureA() {
            while (true) {
                while (_cb) {
                    // Não faz nada
                }
                _ca = true;
                Console.WriteLine("=========== Seção crítica A ===========");
                Task.Delay(5000).Wait();
                _ca = false;
                Console.WriteLine("Processamento da seção não crítica A");
                Task.Delay(5000).Wait();
            }
        }

        private static void ProcedureB() {
            while (true) {
                while (_ca) {
                    // Não faz nada
                }
                _cb = true;
                Console.WriteLine("=========== Seção crítica B ===========");
                Task.Delay(5000).Wait();
                _cb = false;
                Console.WriteLine("Processamento da seção não crítica B");
                Task.Delay(5000).Wait();
            }
        }
    }
}
