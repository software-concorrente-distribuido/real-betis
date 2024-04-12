public class ContaBancaria {
    private int saldo = 0;

    public int getSaldo() {
        try {
            Thread.sleep(100);
        } catch (InterruptedException e) {
            throw new RuntimeException(e);
        }
        return saldo;
    }
    public void setSaldo(int saldo) {
        try {
            Thread.sleep(100);
        } catch (InterruptedException e) {
            throw new RuntimeException(e);
        }
        this.saldo = saldo;
    }
}
