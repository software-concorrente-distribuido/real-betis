package com.github.nogueiralegacy.httpserver.http;

import com.github.nogueiralegacy.httpserver.http.exception.HttpException;
import lombok.SneakyThrows;
import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.TestInstance;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.io.ByteArrayInputStream;
import java.io.InputStream;
import java.nio.charset.StandardCharsets;

import static org.junit.jupiter.api.Assertions.*;

@TestInstance(TestInstance.Lifecycle.PER_CLASS)
class HttpParserTest {
    private static final Logger logger = LoggerFactory.getLogger(HttpParserTest.class);

    private byte[] generateValidGETTestCase() {
        String rawData = "GET / HTTP/1.1\r\n" +
                "Host: localhost:9090\r\n" +
                "Connection: keep-alive\r\n" +
                "Upgrade-Insecure-Requests: 1\r\n" +
                "User-Agent: Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.97 Safari/537.36\r\n" +
                "Sec-Fetch-User: ?1\r\n" +
                "Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3\r\n" +
                "Sec-Fetch-Site: none\r\n" +
                "Sec-Fetch-Mode: navigate\r\n" +
                "Accept-Encoding: gzip, deflate, br\r\n" +
                "Accept-Language: en-US,en;q=0.9,es;q=0.8,pt;q=0.7,de-DE;q=0.6,de;q=0.5,la;q=0.4\r\n" +
                "\r\n";


        return rawData.getBytes(StandardCharsets.UTF_8);
    }

    private byte[] generateValidPOSTTestCase() {
        String rawData = "POST / HTTP/1.1\r\n" +
                "Host: localhost:9090\r\n" +
                "Content-Length: 18\r\n" +
                "Connection: keep-alive\r\n" +
                "Upgrade-Insecure-Requests: 1\r\n" +
                "User-Agent: Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.97 Safari/537.36\r\n" +
                "Sec-Fetch-User: ?1\r\n" +
                "Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3\r\n" +
                "Sec-Fetch-Site: none\r\n" +
                "Sec-Fetch-Mode: navigate\r\n" +
                "Accept-Encoding: gzip, deflate, br\r\n" +
                "Accept-Language: en-US,en;q=0.9,es;q=0.8,pt;q=0.7,de-DE;q=0.6,de;q=0.5,la;q=0.4\r\n" +
                "\r\n" +
                "{\"name\": \"daniel\"}";


        return rawData.getBytes(StandardCharsets.UTF_8);
    }

    private byte[] generateInvalidMethodTestCase() {
        String rawData = "NOTMET / HTTP/1.1\r\n" +
                "Host: localhost:9090\r\n" +
                "Connection: keep-alive\r\n" +
                "Upgrade-Insecure-Requests: 1\r\n" +
                "User-Agent: Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.97 Safari/537.36\r\n" +
                "Sec-Fetch-User: ?1\r\n" +
                "Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3\r\n" +
                "Sec-Fetch-Site: none\r\n" +
                "Sec-Fetch-Mode: navigate\r\n" +
                "Accept-Encoding: gzip, deflate, br\r\n" +
                "Accept-Language: en-US,en;q=0.9,es;q=0.8,pt;q=0.7,de-DE;q=0.6,de;q=0.5,la;q=0.4\r\n" +
                "\r\n";


        return rawData.getBytes(StandardCharsets.UTF_8);
    }

    @BeforeAll
    public void beforeClass() {
    }

    @SneakyThrows
    @Test
    void parseRequestValidTest() {
        InputStream inputStream = new ByteArrayInputStream(generateValidGETTestCase());
        HttpRequest request = HttpParser.parseRequest(inputStream);

        assertEquals(HttpMethod.GET, request.getMethod());
    }

    @Test
    void parseRequestInvalidMethodTest() {
        InputStream inputStream = new ByteArrayInputStream(generateInvalidMethodTestCase());
        HttpException exception = assertThrows(
                HttpException.class,
                () -> HttpParser.parseRequest(inputStream));

        assertEquals(exception.getStatusError(), HttpStatus.BAD_REQUEST);
    }

    @SneakyThrows
    @Test
    void parsePOSTRequestValidTest() {
        InputStream inputStream = new ByteArrayInputStream(generateValidPOSTTestCase());
        HttpRequest request = HttpParser.parseRequest(inputStream);

        String expectBody = "{\"name\": \"daniel\"}";

        assertEquals(expectBody, request.getBody());
    }

    @SneakyThrows
    @Test
    void parseResponseValidTest() {
        HttpResponse response = new HttpResponse
                (
                        HttpStatus.OK,
                        "{\"message\": \"Success\"}".getBytes(),
                        "application/json"
                );

        String expectResponse =
                "HTTP/1.1 200 OK\r\n" +
                        "Content-Length: 22\r\n" +
                        "Content-Type: application/json\r\n" +
                        "\r\n";

        String resultResponseString = response.toString();

        assertEquals(expectResponse, resultResponseString);
    }
}