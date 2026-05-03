package com.qa.portfolio.pages;

import com.microsoft.playwright.Page;

public class LoginPage extends BasePage {

    private static final String BASE_URL      = "https://www.saucedemo.com";
    private static final String USERNAME_INPUT = "#user-name";
    private static final String PASSWORD_INPUT = "#password";
    private static final String LOGIN_BUTTON   = "#login-button";
    private static final String ERROR_MESSAGE  = "[data-test='error']";

    public LoginPage(Page page) {
        super(page);
    }

    public LoginPage navigate() {
        page.navigate(BASE_URL);
        return this;
    }

    public InventoryPage loginAs(String username, String password) {
        waitForVisible(USERNAME_INPUT);
        locate(USERNAME_INPUT).fill(username);
        locate(PASSWORD_INPUT).fill(password);
        locate(LOGIN_BUTTON).click();
        return new InventoryPage(page);
    }

    public LoginPage loginExpectingError(String username, String password) {
        waitForVisible(USERNAME_INPUT);
        locate(USERNAME_INPUT).fill(username);
        locate(PASSWORD_INPUT).fill(password);
        locate(LOGIN_BUTTON).click();
        return this;
    }

    public boolean hasError() {
        return locate(ERROR_MESSAGE).isVisible();
    }

    public String getErrorText() {
        return hasError() ? locate(ERROR_MESSAGE).textContent() : "";
    }
}
