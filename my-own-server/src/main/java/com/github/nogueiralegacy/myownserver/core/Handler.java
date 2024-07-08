package com.github.nogueiralegacy.myownserver.core;

public interface Handler {

    boolean requisicaoValida(byte[] requisicao);

    byte[] resolverRequisicao(byte[] requisicao);
}
