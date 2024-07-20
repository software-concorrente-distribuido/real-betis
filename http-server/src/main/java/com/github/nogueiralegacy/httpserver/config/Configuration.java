package com.github.nogueiralegacy.httpserver.config;

import com.github.nogueiralegacy.httpserver.config.exception.ConfigurationException;
import com.github.nogueiralegacy.httpserver.util.Util;

import java.io.FileOutputStream;
import java.io.IOException;
import java.nio.file.Path;
import java.nio.file.Paths;


public class Configuration {
    private static int port = 8081;
    private static String webroot;

    static {
        webroot = getServerResourcesDir();
        putSampleResource();
    }
    private static String getServerResourcesDir() {
        Path homeDir = Util.getHomeDir();

        Path serverResourcesDir = Paths.get(homeDir.toString(), ".realbetis");

        if (Util.fileExists(serverResourcesDir) || Util.makeDir(serverResourcesDir)) {
            return serverResourcesDir.toString();
        } else {
            throw new ConfigurationException("Não foi possível criar o diretório de recursos do servidor.");
        }
    }

    private static void putSampleResource() {
        String[] resourcesName = {"root.html", "favicon.ico", "monalisa.jpeg"};

        for (String name : resourcesName) {
            byte[] sampleResource = new Util().getResource(name);
            String resourcePath = Paths.get(webroot, name).toString();

            try (FileOutputStream fos = new FileOutputStream(resourcePath)) {
                fos.write(sampleResource);
            } catch (IOException e) {
                throw new RuntimeException(e);
            }
        }
    }

    public static int getPort() {
        return port;
    }

    public static void setPort(int port) {
        Configuration.port = port;
    }

    public static String getWebroot() {
        return webroot;
    }

    public static void setWebroot(String webroot) {
        Configuration.webroot = webroot;
    }
}
