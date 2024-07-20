package com.github.nogueiralegacy.httpserver.http;

import com.github.nogueiralegacy.httpserver.http.exception.HttpException;
import com.github.nogueiralegacy.httpserver.util.Util;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.net.URLDecoder;
import java.nio.charset.StandardCharsets;
import java.util.Hashtable;

public class HttpParser {
    private HttpParser() {
    }

    public static HttpRequest parseRequest(InputStream inputStream) throws IOException, HttpException {
        HttpRequest request = new HttpRequest();

        BufferedReader reader = Util.getBufferedReader(inputStream);
        String startline = reader.readLine();

        if (startline == null || startline.isEmpty()) {
            throw new HttpException(HttpStatus.BAD_REQUEST);
        }

        String[] temp = startline.split(" ");

        if (temp.length != 3) {
            throw new HttpException(HttpStatus.BAD_REQUEST);
        }
        if (!isHttpMethod(temp[0])) {
            throw new HttpException(HttpStatus.BAD_REQUEST);

        }
        HttpMethod method = HttpMethod.valueOf(temp[0]);
        String urlDecode = URLDecoder.decode(temp[1], StandardCharsets.UTF_8);
        String httpVersion = temp[2];

        request.setMethod(method);
        request.setUrl(urlDecode);
        request.setHttpVersion(httpVersion);

        Hashtable<String, String> cabecalho = parseHeaders(reader);
        request.addHeaders(cabecalho);

        // Ler corpo se não for GET e se Content-Length estiver presente
        if (!method.equals(HttpMethod.GET)) {
            String contentLengthHeader = request.getHeaders().get("content-length");
            if (contentLengthHeader != null) {
                int contentLength = Integer.parseInt(contentLengthHeader);
                String body = parseBody(reader, contentLength);
                request.setBody(body);
            } else {
                throw new HttpException(HttpStatus.BAD_REQUEST);
            }
        }


        return request;
    }

    public static Hashtable<String, String> parseHeaders(BufferedReader reader) throws IOException {
        String linha;
        Hashtable<String, String> cabecalho = new Hashtable<>();
        while ((linha = reader.readLine()) != null && !linha.isEmpty()) {
            String[] header = linha.split(":");
            cabecalho.put(header[0].toLowerCase(), header[1].trim());
        }

        return cabecalho;
    }

    public static String parseBody(BufferedReader reader, int contentLength) throws IOException {
        char[] bodyChars = new char[contentLength];
        int read = reader.read(bodyChars, 0, contentLength);
        if (read != contentLength) {
            throw new HttpException(
                    "Body mismatch to Content-lenght header",
                    HttpStatus.BAD_REQUEST);
        }
        return new String(bodyChars);
    }

    public static boolean isHttpMethod(String str) {
        for (HttpMethod method : HttpMethod.values()) {
            if (method.name().equals(str)) {
                return true;
            }
        }

        return false;
    }
}
