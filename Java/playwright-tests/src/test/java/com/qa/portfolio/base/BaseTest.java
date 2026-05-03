package com.qa.portfolio.base;

import com.microsoft.playwright.*;
import com.qa.portfolio.pages.LoginPage;
import org.testng.annotations.AfterMethod;
import org.testng.annotations.BeforeMethod;

public abstract class BaseTest {

    protected Playwright playwright;
    protected Browser browser;
    protected BrowserContext context;
    protected com.microsoft.playwright.Page page;
    protected LoginPage loginPage;

    @BeforeMethod
    public void setUp() {
        playwright = Playwright.create();
        browser = playwright.chromium().launch(
                new BrowserType.LaunchOptions().setHeadless(true));
        context = browser.newContext(
                new Browser.NewContextOptions().setViewportSize(1920, 1080));
        page = context.newPage();
        loginPage = new LoginPage(page).navigate();
    }

    @AfterMethod
    public void tearDown() {
        if (context != null) context.close();
        if (browser != null) browser.close();
        if (playwright != null) playwright.close();
    }
}
