package com.github.nogueiralegacy.httpserver.http.exception;

import com.github.nogueiralegacy.httpserver.http.HttpStatus;
import lombok.Getter;
import lombok.Setter;

public class HttpException extends RuntimeException {
    @Getter
    private HttpStatus statusError;
    public HttpException(String message) {
        super(message);
    }
    public HttpException(String message, HttpStatus statusError) {
        super(message);
        this.statusError = statusError;
    }

    public HttpException(HttpStatus statusError) {
        super(statusError.getMESSAGE());
        this.statusError = statusError;
    }


    public HttpException(String message, Throwable cause) {
        super(message, cause);
    }
}
