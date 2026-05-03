package com.qa.portfolio.tests;

import com.qa.portfolio.base.BaseTest;
import org.testng.annotations.Test;

import static org.assertj.core.api.Assertions.assertThat;

public class LoginTests extends BaseTest {

    private static final String VALID_USER    = "standard_user";
    private static final String LOCKED_USER   = "locked_out_user";
    private static final String INVALID_USER  = "invalid_user";
    private static final String VALID_PASS    = "secret_sauce";
    private static final String INVALID_PASS  = "wrong_password";

    @Test(description = "Valid credentials should navigate to inventory page")
    public void loginWithValidCredentials_shouldNavigateToInventory() {
        var inventory = loginPage.loginAs(VALID_USER, VALID_PASS);

        assertThat(inventory.isLoaded()).isTrue();
        assertThat(page.url()).contains("inventory");
    }

    @Test(description = "Invalid password should show error message")
    public void loginWithInvalidPassword_shouldShowError() {
        loginPage.loginExpectingError(VALID_USER, INVALID_PASS);

        assertThat(loginPage.hasError()).isTrue();
        assertThat(loginPage.getErrorText()).contains("Username and password do not match");
    }

    @Test(description = "Locked-out user should see locked error")
    public void loginWithLockedUser_shouldShowLockedError() {
        loginPage.loginExpectingError(LOCKED_USER, VALID_PASS);

        assertThat(loginPage.hasError()).isTrue();
        assertThat(loginPage.getErrorText()).contains("locked out");
    }

    @Test(description = "Empty credentials should prompt for username")
    public void loginWithEmptyCredentials_shouldShowRequiredError() {
        loginPage.loginExpectingError("", "");

        assertThat(loginPage.hasError()).isTrue();
        assertThat(loginPage.getErrorText()).contains("Username is required");
    }

    @Test(description = "Logging out should return to login page")
    public void logoutAfterLogin_shouldReturnToLoginPage() {
        loginPage.loginAs(VALID_USER, VALID_PASS).logout();

        assertThat(page.url()).isEqualTo("https://www.saucedemo.com/");
    }
}
