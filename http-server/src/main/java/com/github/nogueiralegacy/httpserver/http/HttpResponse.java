package com.github.nogueiralegacy.httpserver.http;

import com.github.nogueiralegacy.httpserver.util.Util;
import lombok.Getter;
import lombok.Setter;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.io.IOException;
import java.io.OutputStream;
import java.nio.charset.StandardCharsets;
import java.util.Arrays;
import java.util.Hashtable;
import java.util.Objects;

@Getter
@Setter
public class HttpResponse {
    private static final Logger logger = LoggerFactory.getLogger(HttpResponse.class);
    private String httpVersion;
    private HttpStatus status;
    private Hashtable<String, String> headers;
    private byte[] body;

    public HttpResponse(HttpStatus status, byte[] body, String bodyContentType) throws IllegalArgumentException {
        this.httpVersion = "HTTP/1.1";
        this.status = status;
        this.headers = new Hashtable<>();
        this.body = body;
        addHeader("Content-Type", bodyContentType);
        addHeader("Content-Length", String.valueOf((body).length));
    }

    public HttpResponse(HttpStatus status) {
        this.httpVersion = "HTTP/1.1";
        this.status = status;
        this.headers = new Hashtable<>();
        this.body = new byte[0];
        addHeader("Content-Length", "0");
    }

    public void addHeader(String key, String value) {
        this.headers.put(key, value);
    }

    public void addHeaders(Hashtable<String, String> headers) {
        this.headers.putAll(headers);
    }

    //So do cabecalho o corpo fica em byte[]
    @Override
    public String toString() {
        StringBuilder response = new StringBuilder();
        final String CRLF = "\r\n";

        response.append(this.httpVersion)
                .append(" ")
                .append(this.status.getSTATUS_CODE())
                .append(" ")
                .append(this.status.getMESSAGE())
                .append(CRLF);

        this.headers
                .forEach((key, value) -> response
                        .append(key)
                        .append(": ")
                        .append(value)
                        .append(CRLF));

        response
                .append(CRLF);

        return response.toString();
    }

    public void sendResponse(OutputStream outputStream) throws IOException {
        // Enviar cabeçalhos
        String headers = this.toString();
        outputStream.write(headers.getBytes(StandardCharsets.UTF_8));
        outputStream.flush();

        // Enviar corpo da imagem
        outputStream.write(body);
        outputStream.flush();
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        HttpResponse that = (HttpResponse) o;
        return Objects.equals(httpVersion, that.httpVersion)
                && status == that.status
                && Objects.equals(headers, that.headers)
                && Arrays.equals(body, that.body);
    }

    @Override
    public int hashCode() {
        return Objects.hash(httpVersion, status, headers, Arrays.hashCode(body));
    }
}
