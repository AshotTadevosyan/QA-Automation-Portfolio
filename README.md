# QA Automation Portfolio

A multi-stack test automation portfolio demonstrating end-to-end QA engineering skills across unit, integration, and UI layers.

![CI](https://github.com/ashottadevosyan/QA-Automation-Portfolio/actions/workflows/ci.yml/badge.svg)

---

## Stack at a Glance

| Layer | Technology | Language | Target |
|---|---|---|---|
| Unit Tests | xUnit + FluentAssertions | C# / .NET 9 | ShoppingCart domain |
| UI Tests | Selenium WebDriver 4 | C# / .NET 9 | SauceDemo |
| UI Tests | Playwright 1.50 + TestNG | Java 21 | SauceDemo |
| CI/CD | GitHub Actions | YAML | All three suites |

---

## Project Structure

```
QA-Automation-Portfolio/
├── .github/workflows/ci.yml       # CI pipeline — runs all 3 suites
├── CSharp/
│   ├── xUnit.UnitTests/           # Unit tests — shopping cart domain
│   │   ├── Models/                # Product, CartItem
│   │   ├── Services/              # ShoppingCartService, ProductValidator
│   │   └── Tests/                 # ShoppingCartTests, ProductValidatorTests
│   └── Selenium.WebTests/         # Selenium UI tests
│       ├── Config/                # TestSettings (URLs, credentials)
│       ├── Helpers/               # DriverFactory (headless Chrome)
│       ├── Pages/                 # Page Object Model
│       └── Tests/                 # LoginTests, CartTests
└── Java/
    └── playwright-tests/          # Playwright UI tests
        ├── src/main/java/…/pages/ # Page Object Model
        ├── src/test/java/…/tests/ # LoginTests, InventoryTests, CheckoutTests
        ├── testng.xml             # TestNG suite config
        └── pom.xml
```

---

## C# — xUnit Unit Tests

Tests a shopping-cart domain model in isolation with no external dependencies.

**Covers:** add item, remove item, update quantity, apply discount, cart capacity enforcement, product validation rules.

```bash
cd CSharp/xUnit.UnitTests
dotnet test
```

---

## C# — Selenium WebDriver Tests

Page Object Model against [SauceDemo](https://www.saucedemo.com) with headless Chrome.

**Covers:** valid/invalid login, locked-out user, empty credentials, logout, add/remove cart items, full checkout flow.

```bash
cd CSharp/Selenium.WebTests
dotnet test
```

> Requires Google Chrome installed locally. Tests run headless by default.

---

## Java — Playwright Tests

Page Object Model against [SauceDemo](https://www.saucedemo.com) using Playwright's built-in browser automation (no WebDriver).

**Covers:** login scenarios, inventory product count, add/remove items, sort by name, cart navigation, full checkout.

```bash
cd Java/playwright-tests

# First-time: install Playwright browsers
./mvnw exec:java -e -D exec.mainClass=com.microsoft.playwright.CLI \
  -D exec.args="install --with-deps chromium"

# Run tests
./mvnw test
```

> Maven is not required — the Maven wrapper (`mvnw`) downloads it automatically.

---

## CI / CD

GitHub Actions runs all three suites on every push to `main` or `develop` and on pull requests. Test results are uploaded as artifacts.

```
unit-tests-csharp     → dotnet test (xUnit)
selenium-tests-csharp → dotnet test (Selenium, headless Chrome)
playwright-tests-java → ./mvnw test (Playwright + TestNG)
```

---

## Design Decisions

- **Page Object Model** across both Selenium and Playwright suites — locators are encapsulated, tests read like business language.
- **FluentAssertions / AssertJ** for expressive, readable assertions rather than bare `Assert.Equal` / `assertEquals`.
- **Headless by default** — all browser tests run headless so they work in CI and locally with no GUI.
- **Maven wrapper** — no local Maven installation required; `./mvnw` downloads the correct version on first run.
- **Single CI file** — all three language/framework combinations run in parallel jobs to keep feedback fast.
