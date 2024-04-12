import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.TimeUnit;

public class Main {
    public static void main(String[] args) throws InterruptedException {
        var conta = new ContaBancaria();
        ExecutorService executor = Executors.newFixedThreadPool(8);
        executor.submit(() -> {
            synchronized (conta) {
                System.out.println("Acao 1");
                var saldoAtual = conta.getSaldo();
                saldoAtual += 100;
                conta.setSaldo(saldoAtual);
            }
        });
        executor.submit(() -> {
            synchronized (conta) {
                System.out.println("Acao 2");
                var saldoAtual = conta.getSaldo();
                saldoAtual += 300;
                conta.setSaldo(saldoAtual);
            }
        });
        executor.submit(() -> {
            synchronized (conta) {
                System.out.println("Acao 3");
                var saldoAtual = conta.getSaldo();
                saldoAtual -= 200;
                conta.setSaldo(saldoAtual);
            }
        });
        executor.submit(() -> {
            synchronized (conta) {
                System.out.println("Acao 4");
                var saldoAtual = conta.getSaldo();
                saldoAtual += 150;
                conta.setSaldo(saldoAtual);
            }
        });
        executor.submit(() -> {
            synchronized (conta) {
                System.out.println("Acao 5");
                var saldoAtual = conta.getSaldo();
                saldoAtual -= 350;
                conta.setSaldo(saldoAtual);
            }
        });
        executor.submit(() -> {
            synchronized (conta) {
                System.out.println("Acao 6");
                var saldoAtual = conta.getSaldo();
                saldoAtual += 200;
                conta.setSaldo(saldoAtual);
            }
        });
        executor.submit(() -> {
            synchronized (conta) {
                System.out.println("Acao 7");
                var saldoAtual = conta.getSaldo();
                saldoAtual += 450;
                conta.setSaldo(saldoAtual);
            }
        });
        executor.submit(() -> {
            synchronized (conta) {
                System.out.println("Acao 8");
                var saldoAtual = conta.getSaldo();
                saldoAtual -= 250;
                conta.setSaldo(saldoAtual);
            }
        });
        executor.shutdown();
        executor.awaitTermination(5000, TimeUnit.MILLISECONDS);
        System.out.println("Saldo final CORRETO: 400");
        System.out.println("Saldo atual: " + conta.getSaldo());
    }
}