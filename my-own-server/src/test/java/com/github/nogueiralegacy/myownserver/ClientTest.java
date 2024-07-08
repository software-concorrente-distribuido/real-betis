package com.github.nogueiralegacy.myownserver;

import com.github.nogueiralegacy.myownserver.core.Atendente;
import com.github.nogueiralegacy.myownserver.utils.Utils;
import lombok.SneakyThrows;
import org.json.JSONObject;
import org.junit.jupiter.api.Disabled;
import org.junit.jupiter.api.Test;

import java.io.*;
import java.net.Socket;

import static org.junit.jupiter.api.Assertions.assertEquals;

public class ClientTest {
    private Socket clientSocket;
    public JSONObject connection(String ip, int port, String request) {
        try {
            clientSocket = new Socket(ip, port);

            PrintWriter out = new PrintWriter(clientSocket.getOutputStream(), true);
            // Manda requisicao
            out.println(request);

            byte[] buffer = clientSocket.getInputStream().readAllBytes();

            JSONObject resposta = new JSONObject(new String(buffer));

            return resposta;
        } catch (IOException e) {
            e.printStackTrace();
            return null;
        }
    }

    @Disabled
    @SneakyThrows
    @Test
    void testConnection() {
        String request = "image 2";
        JSONObject resposta = connection("localhost", 9090, request);

        // test.jpg == image 2
        byte[] imagemBuffer = new Atendente().getImagem("test.jpg");

        String imagemBase64 = Utils.toBase64String(imagemBuffer);

        assertEquals(imagemBase64, resposta.get("image"));
    }
}
