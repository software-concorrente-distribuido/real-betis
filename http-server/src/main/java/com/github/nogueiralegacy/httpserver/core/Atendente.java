package com.github.nogueiralegacy.httpserver.core;

import com.github.nogueiralegacy.httpserver.config.Configuration;
import com.github.nogueiralegacy.httpserver.http.HttpParser;
import com.github.nogueiralegacy.httpserver.http.HttpRequest;
import com.github.nogueiralegacy.httpserver.http.HttpResponse;
import com.github.nogueiralegacy.httpserver.http.HttpStatus;
import com.github.nogueiralegacy.httpserver.http.exception.HttpException;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.io.IOException;
import java.io.InputStream;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;

public class Atendente implements Handler {
    private static final Logger logger = LoggerFactory.getLogger(Atendente.class);

    @Override
    public HttpResponse resolverRequisicao(InputStream requisicaoIS) {
        HttpResponse response;
        HttpRequest request;
        try {
             request = HttpParser.parseRequest(requisicaoIS);
        } catch (IOException e) {
            logger.error("Erro ao fazer o parse da requisição", e);
            throw new RuntimeException(e);
        } catch (HttpException e) {
            logger.error("HTTP Erro ao fazer o parse da requisição", e);
            response = new HttpResponse(e.getStatusError());
            return response;
        }

        String url = request.getUrl();
        logger.info("Requisição para URL: " + url);

        // CORE: pegar o recurso requisitado
        byte[] resource;
        String mimeType;
        try {
            Path resourcePath = getResourcePath(url);
            logger.info("Caminho do recurso: " + resourcePath.toString());
            mimeType = getResourceMimeType(resourcePath);
            logger.info("MimeType do recurso: " + mimeType);

            resource = getResource(resourcePath);

        } catch (IOException e) {
            logger.error("Erro ao tentar carregar o recurso desejado", e);
            response = new HttpResponse(HttpStatus.SERVICE_UNAVAILABLE);
            return response;
        }

        response = new HttpResponse(HttpStatus.OK, resource, mimeType);

        return response;
    }

    public static byte[] getResource(Path resourcePath) throws IOException{
        if (!Files.exists(resourcePath) || !Files.isRegularFile(resourcePath)) {
            throw new IOException("Recurso não encontrado: " + resourcePath);
        }

        try {
            return Files.readAllBytes(resourcePath);
        } catch (IOException e) {
            throw new IOException("Falha ao ler recurso como string: " + resourcePath, e);
        }
    }

    public Path getResourcePath(String url) {
        String webroot = Configuration.getWebroot();
        String[] dirs = url.split("/");

        Path resourcePath = Paths.get(webroot);
        for (String dir : dirs) {
            resourcePath = Paths.get(resourcePath.toString(), dir);
        }

        return resourcePath;
    }

    public String getResourceMimeType(Path resourcePath) throws IOException{
        String mimeType = Files.probeContentType(resourcePath);
        if (mimeType == null) {
            mimeType = "application/octet-stream";
        }
        return mimeType;    }
}
