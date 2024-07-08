package com.github.nogueiralegacy.myownserver.utils;

import java.io.IOException;
import java.io.InputStream;
import java.net.URI;
import java.net.URL;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.Base64;

public class Utils {
    /**
     * Retorna caminho de um arquivo no diretório resources.
     *
     * @param fileName O nome do arquivo desejado no resources.
     *
     * @return Retorna o caminho para o arquivo no diretório resources.
     */
    public Path getPath(String fileName) {
        ClassLoader classLoader = this.getClass().getClassLoader();
        URL resource = classLoader.getResource(fileName);
        URI uri = null;

        try {
            uri = resource.toURI();
        } catch (Exception exp) {
            return null;
        }

        return Paths.get(uri);
    }

    public byte[] getResource(String fileName) {
        ClassLoader classLoader = this.getClass().getClassLoader();

        try (InputStream resource = classLoader.getResourceAsStream(fileName)) {
            return resource.readAllBytes();
        } catch (IOException e) {
            throw new RuntimeException(e);
        }

    }

    public static byte[] toBase64(byte[] b) {
        return Base64.getEncoder().encode(b);
    }

    public static String toBase64String(byte[] b) {
        return Base64.getEncoder().encodeToString(b);
    }
}
