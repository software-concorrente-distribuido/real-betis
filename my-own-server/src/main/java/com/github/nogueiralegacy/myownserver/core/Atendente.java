package com.github.nogueiralegacy.myownserver.core;

import com.github.nogueiralegacy.myownserver.utils.Utils;
import org.json.JSONObject;

public class Atendente implements Handler {
    private static final int QUANTIDADE_MAX_IMAGENS = 5;

    public boolean requisicaoValida(byte[] requisicao) throws IllegalArgumentException {
        String stringRequisicao = converteRequisicao(requisicao);

        String[] argumentos = stringRequisicao.split(" ");
        if (!argumentos[0].equals("image")) {
            return false;
        }
        int numeroImagem = 0;
        try {
            numeroImagem = Integer.parseInt(argumentos[1].trim());
        } catch (NumberFormatException e) {
            throw new IllegalArgumentException("O segundo argumento deve ser um número");
        }

        if (numeroImagem < 1 || numeroImagem > QUANTIDADE_MAX_IMAGENS) {
            return false;
        }

        return true;
    }

    // Resposta
    public byte[] resolverRequisicao(byte[] requisicao) throws IllegalArgumentException {
        if (!requisicaoValida(requisicao)) {
            throw new IllegalArgumentException("Requisicao invalida");
        }
        String stringRequisicao = converteRequisicao(requisicao);
        String[] argumentos = stringRequisicao.split(" ");

        int numeroImagem = Integer.parseInt(argumentos[1].trim());

        byte[] imagemBuffer = getImagem(numeroImagem);

        return formataResposta(imagemBuffer).getBytes();
    }

    private static String converteRequisicao(byte[] requisicao) {
        return new String(requisicao);
    }

    public String formataResposta(byte[] imagem) {
        JSONObject resposta = new JSONObject();

        resposta.put("imageSize", imagem.length);
        resposta.put("image", Utils.toBase64String(imagem));
        return resposta.toString();
    }

    private byte[] getImagem(int numeroImagem) {
        return getImagem(numeroImagem + ".jpg");
    }

    public byte[] getImagem(String nomeArquivo) {
        return new Utils().getResource(nomeArquivo);
    }
}
