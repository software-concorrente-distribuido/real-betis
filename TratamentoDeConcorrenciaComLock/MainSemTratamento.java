import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.TimeUnit;

public class MainSemTratamento {
    public static void main(String[] args) throws InterruptedException {
        var conta = new ContaBancaria();
        ExecutorService executor = Executors.newFixedThreadPool(8);
        executor.submit(() -> {
            var saldoAtual = conta.getSaldo();
            saldoAtual += 100;
            conta.setSaldo(saldoAtual);
        });
        executor.submit(() -> {
            var saldoAtual = conta.getSaldo();
            saldoAtual += 300;
            conta.setSaldo(saldoAtual);
        });
        executor.submit(() -> {
            var saldoAtual = conta.getSaldo();
            saldoAtual -= 200;
            conta.setSaldo(saldoAtual);
        });
        executor.submit(() -> {
            var saldoAtual = conta.getSaldo();
            saldoAtual += 150;
            conta.setSaldo(saldoAtual);
        });
        executor.submit(() -> {
            var saldoAtual = conta.getSaldo();
            saldoAtual -= 350;
            conta.setSaldo(saldoAtual);
        });
        executor.submit(() -> {
            var saldoAtual = conta.getSaldo();
            saldoAtual += 200;
            conta.setSaldo(saldoAtual);
        });
        executor.submit(() -> {
            var saldoAtual = conta.getSaldo();
            saldoAtual += 450;
            conta.setSaldo(saldoAtual);
        });
        executor.submit(() -> {
            var saldoAtual = conta.getSaldo();
            saldoAtual -= 250;
            conta.setSaldo(saldoAtual);
        });
        executor.shutdown();
        executor.awaitTermination(1000, TimeUnit.MILLISECONDS);
        System.out.println("Saldo final CORRETO: 400");
        System.out.println("Saldo atual: " + conta.getSaldo());
    }
}
