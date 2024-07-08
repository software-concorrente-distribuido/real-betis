package com.github.nogueiralegacy.myownserver.core;

import java.net.ServerSocket;
import java.net.Socket;

public class Porteiro {
    private static int quantidadeClientes = 0;
    private final static int PORT = 9090;
    private final static ServerSocket SERVER_SOCKET;

    static {
        // Abre a porta do servidor
        try {
            SERVER_SOCKET = new ServerSocket(PORT);
        } catch (Exception e) {
            throw new RuntimeException("Erro ao criar o socket do servidor", e);
        }
    }

    private static Cliente recebeCliente() {
        try {
            Socket socket = SERVER_SOCKET.accept();
            System.out.println("Bem vindo cliente " + quantidadeClientes);

            return new Cliente(socket);
        } catch (Exception e) {
            throw new RuntimeException("Erro ao receber o cliente", e);
        }
    }

    private static void despedeCliente(Cliente cliente) {
        try {
            cliente.getSOCKET().close();
            System.out.println("Tchau cliente " + quantidadeClientes + "\n");
            quantidadeClientes++;
        } catch (Exception e) {
            throw new RuntimeException("Erro ao fechar o socket do cliente", e);
        }
    }

    public static void comecaTurno() {
        while(true) {
            try {
                Cliente cliente = recebeCliente();

                // Chama o Atendente
                Atendente atendente = new Atendente();

                byte[] resposta = atendente.resolverRequisicao(cliente.getRequisicao());

                cliente.setResposta(resposta);

                despedeCliente(cliente);
            } catch (Exception e) {
                System.out.println(e.getMessage());
            }

        }
    }
}
