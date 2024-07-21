package com.github.nogueiralegacy.httpserver.core;

import com.github.nogueiralegacy.httpserver.http.HttpResponse;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.util.concurrent.Executors;
import java.util.concurrent.ThreadPoolExecutor;

public class Recepcionista {
    private static final Logger logger = LoggerFactory.getLogger(Recepcionista.class);
    private static final int THREADS_NUMBER = 10;
    private static final ThreadPoolExecutor executor =
            (ThreadPoolExecutor) Executors.newFixedThreadPool(THREADS_NUMBER);

    public static void repassaCliente(Cliente cliente) {
        executor.execute(() -> {
            Handler atendente = new Atendente();

            HttpResponse resposta = atendente.resolverRequisicao(cliente.getRequisicao());

            cliente.setResposta(resposta);

            Porteiro.despedeCliente(cliente);
        });
    }
}
