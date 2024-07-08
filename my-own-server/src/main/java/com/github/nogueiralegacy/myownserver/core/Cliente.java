package com.github.nogueiralegacy.myownserver.core;

import java.io.OutputStream;
import java.net.Socket;

public class Cliente {
    private static final int TAMANHO_MAX_REQUISICAO = 1024;
    private final Socket SOCKET;

    public Cliente(Socket SOCKET) {
        this.SOCKET = SOCKET;
    }


    public void setResposta(byte[] resposta) {
        try {
            OutputStream outputStream = SOCKET.getOutputStream();

            outputStream.write(resposta);
            outputStream.flush();
        } catch (Exception e) {
            throw new RuntimeException("Erro ao enviar a resposta para o cliente", e);
        }
    }

    public byte[] getRequisicao() {
        try {
            byte[] buffer = new byte[TAMANHO_MAX_REQUISICAO];
            int tamanho = SOCKET.getInputStream().read(buffer);

            byte[] requisicao = new byte[tamanho];
            System.arraycopy(buffer, 0, requisicao, 0, tamanho);

            return requisicao;
        } catch (Exception e) {
            throw new RuntimeException("Erro ao receber a requisição do cliente", e);
        }
    }

    public Socket getSOCKET() {
        return SOCKET;
    }
}
