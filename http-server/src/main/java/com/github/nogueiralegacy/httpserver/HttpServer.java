package com.github.nogueiralegacy.httpserver;

import com.github.nogueiralegacy.httpserver.core.Porteiro;
import com.github.nogueiralegacy.httpserver.util.AscciArt;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class HttpServer {
    private static final Logger logger = LoggerFactory.getLogger(HttpServer.class);
    public static void main(String[] args) {
        logger.info("Server started...");

        AscciArt.printRealBetis();
        Porteiro.comecaTurno();
    }
}
