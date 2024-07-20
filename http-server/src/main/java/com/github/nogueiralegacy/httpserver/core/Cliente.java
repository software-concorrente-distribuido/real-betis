package com.github.nogueiralegacy.httpserver.core;

import com.github.nogueiralegacy.httpserver.http.HttpResponse;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;
import java.net.SocketException;


public class Cliente {
    private static final Logger logger = LoggerFactory.getLogger(Cliente.class);

    private static final int TAMANHO_MAX_REQUISICAO = 1024 * 50;
    private final Socket SOCKET;

    public Cliente(Socket SOCKET) {
        this.SOCKET = SOCKET;
    }


    public void setResposta(HttpResponse response) {
        try {
            OutputStream outputStream = SOCKET.getOutputStream();

            response.sendResponse(outputStream);
            outputStream.close();
        } catch (SocketException e) {
            logger.error("Erro ao enviar a resposta: Conexão fechada pelo cliente", e);
        }
        catch (Exception e) {
            throw new RuntimeException("Erro ao enviar a resposta para o cliente", e);
        }
    }

    public InputStream getRequisicao() {
        try {
            return SOCKET.getInputStream();
        } catch (Exception e) {
            throw new RuntimeException("Erro ao receber a requisição do cliente", e);
        }
    }

    public Socket getSOCKET() {
        return SOCKET;
    }
}