package com.github.nogueiralegacy.httpserver.http;

import lombok.Getter;
import lombok.Setter;

import java.util.Hashtable;
import java.util.Objects;

@Getter
@Setter
public class HttpRequest {
    private HttpMethod method;
    private String url;
    private String httpVersion;
    private Hashtable<String, String> headers;
    private String body;

    public HttpRequest() {
        headers = new Hashtable<>();
    }

    public void addHeader(String key, String value) {
        this.headers.put(key, value);
    }

    public void addHeaders(Hashtable<String, String> headers) {
        this.headers.putAll(headers);
    }
}
