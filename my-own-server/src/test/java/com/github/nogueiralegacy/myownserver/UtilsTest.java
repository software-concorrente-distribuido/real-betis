package com.github.nogueiralegacy.myownserver;

import com.github.nogueiralegacy.myownserver.utils.Utils;
import org.junit.jupiter.api.Test;

import java.nio.file.Path;

import static org.junit.jupiter.api.Assertions.assertNotNull;

public class UtilsTest {
    @Test
    void testGetPath() {
        Path resourcePath = new Utils().getPath("test.jpg");
        assertNotNull(resourcePath);
    }
}
