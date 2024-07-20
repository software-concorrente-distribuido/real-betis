package com.github.nogueiralegacy.httpserver.core;

import com.github.nogueiralegacy.httpserver.http.HttpResponse;

import java.io.InputStream;
import java.io.OutputStream;

public interface Handler {

    HttpResponse resolverRequisicao(InputStream requisicao);
}