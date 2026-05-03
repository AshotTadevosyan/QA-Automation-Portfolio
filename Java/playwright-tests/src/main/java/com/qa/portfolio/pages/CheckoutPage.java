package com.qa.portfolio.pages;

import com.microsoft.playwright.Page;

public class CheckoutPage extends BasePage {

    private static final String FIRST_NAME    = "#first-name";
    private static final String LAST_NAME     = "#last-name";
    private static final String POSTAL_CODE   = "#postal-code";
    private static final String CONTINUE_BTN  = "#continue";
    private static final String FINISH_BTN    = "#finish";
    private static final String CONFIRM_HDR   = ".complete-header";

    public CheckoutPage(Page page) {
        super(page);
    }

    public CheckoutPage fillShippingInfo(String firstName, String lastName, String postalCode) {
        waitForVisible(FIRST_NAME);
        locate(FIRST_NAME).fill(firstName);
        locate(LAST_NAME).fill(lastName);
        locate(POSTAL_CODE).fill(postalCode);
        locate(CONTINUE_BTN).click();
        return this;
    }

    public CheckoutPage finishOrder() {
        waitForVisible(FINISH_BTN);
        locate(FINISH_BTN).click();
        return this;
    }

    public boolean isOrderConfirmed() {
        waitForVisible(CONFIRM_HDR);
        return locate(CONFIRM_HDR).textContent().contains("Thank you");
    }
}
