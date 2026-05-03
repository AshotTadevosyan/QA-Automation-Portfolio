namespace Selenium.WebTests.Config;

public static class TestSettings
{
    public static string BaseUrl => "https://www.saucedemo.com";
    public static string ValidUsername => "standard_user";
    public static string LockedUsername => "locked_out_user";
    public static string InvalidUsername => "invalid_user";
    public static string ValidPassword => "secret_sauce";
    public static string InvalidPassword => "wrong_password";
    public static int DefaultTimeoutSeconds => 20;
}
