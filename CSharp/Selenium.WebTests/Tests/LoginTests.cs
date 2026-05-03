using FluentAssertions;
using Selenium.WebTests.Config;
using Xunit;

namespace Selenium.WebTests.Tests;

[Trait("Category", "Login")]
public class LoginTests : BaseTest
{
    [Fact]
    public void Login_WithValidCredentials_ShouldNavigateToInventory()
    {
        var inventory = LoginPage.LoginAs(TestSettings.ValidUsername, TestSettings.ValidPassword);

        inventory.IsLoaded.Should().BeTrue();
        Driver.Url.Should().Contain("inventory");
    }

    [Fact]
    public void Login_WithInvalidPassword_ShouldShowErrorMessage()
    {
        LoginPage.LoginExpectingError(TestSettings.ValidUsername, TestSettings.InvalidPassword);

        LoginPage.HasError.Should().BeTrue();
        LoginPage.ErrorText.Should().Contain("Username and password do not match");
    }

    [Fact]
    public void Login_WithLockedOutUser_ShouldShowLockedError()
    {
        LoginPage.LoginExpectingError(TestSettings.LockedUsername, TestSettings.ValidPassword);

        LoginPage.HasError.Should().BeTrue();
        LoginPage.ErrorText.Should().Contain("locked out");
    }

    [Fact]
    public void Login_WithEmptyCredentials_ShouldShowRequiredError()
    {
        LoginPage.LoginExpectingError(string.Empty, string.Empty);

        LoginPage.HasError.Should().BeTrue();
        LoginPage.ErrorText.Should().Contain("Username is required");
    }

    [Fact]
    public void Logout_AfterLogin_ShouldReturnToLoginPage()
    {
        var inventory = LoginPage.LoginAs(TestSettings.ValidUsername, TestSettings.ValidPassword);
        inventory.Logout();

        Driver.Url.Should().Be($"{TestSettings.BaseUrl}/");
    }
}
