package com.github.nogueiralegacy.httpserver.util;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.nio.file.Path;
import java.nio.file.Paths;

/**
 * Utility class for handling various operations.
 */
public class Util {
    private static final Logger logger = LoggerFactory.getLogger(Util.class);

    /**
     * Get the content of small resources which is located at src/main/resources
     *
     * @param fileName The file name
     * @return A byte array with the file content
     * @throws RuntimeException if the resource cannot be loaded
     */
    public byte[] getResource(String fileName) throws RuntimeException {
        ClassLoader classLoader = this.getClass().getClassLoader();

        try (InputStream resource = classLoader.getResourceAsStream(fileName)) {
            if (resource != null) {
                return resource.readAllBytes();
            }
            logger.error("Cannot load resource: " + fileName + " from resources folder.");
            throw new IOException("Cannot load resource: " + fileName + " from resources folder.");
        } catch (IOException e) {
            throw new RuntimeException(e);
        }
    }

    public static Path getHomeDir() throws IllegalStateException{
        String userHome = System.getProperty("user.home");

        if (userHome == null) {
            logger.error("Não foi possível determinar o diretório home do usuário.");
            throw new IllegalStateException("Não foi possível determinar o diretório home do usuário.");
        }

        return Paths.get(userHome);
    }

    public static boolean makeDir(Path path) {
        return path.toFile().mkdirs();
    }

    public static boolean fileExists(Path path) {
        return path.toFile().exists();
    }


    public static BufferedReader getBufferedReader(InputStream inputStream) {
        return new BufferedReader(new InputStreamReader(inputStream));
    }

    public static byte[] appendBytes(byte[] a, byte[] b) {
        byte[] c = new byte[a.length + b.length];
        System.arraycopy(a, 0, c, 0, a.length);
        System.arraycopy(b, 0, c, a.length, b.length);

        return c;
    }
}