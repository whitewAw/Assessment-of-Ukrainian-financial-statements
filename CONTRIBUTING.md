# Contributing to Assessment of Ukrainian Financial Statements

Thank you for your interest in contributing! ??

This project aims to provide a comprehensive tool for analyzing Ukrainian financial statements. We welcome contributions from developers, accountants, financial analysts, and anyone interested in improving financial analysis tools.

## ?? Ways to Contribute

- ?? **Report bugs** - Found an issue? Let us know!
- ?? **Suggest features** - Have ideas for improvements?
- ?? **Improve documentation** - Help others understand the project
- ?? **Add translations** - Support more languages
- ?? **Submit code** - Fix bugs or implement features
- ?? **Add financial indicators** - Expand analysis capabilities

## ?? Getting Started

### Prerequisites

- .NET 9 SDK
- Visual Studio 2022 or VS Code with C# extension
- Basic knowledge of Blazor WebAssembly

### Setup

1. **Fork the repository**
   ```bash
   # Click the "Fork" button on GitHub
   ```

2. **Clone your fork**
   ```bash
   git clone https://github.com/YOUR-USERNAME/Assessment-of-Ukrainian-financial-statements.git
   cd Assessment-of-Ukrainian-financial-statements
   ```

3. **Create a branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

4. **Build and run**
   ```bash
   dotnet build
   dotnet run --project AFS
   ```

## ?? Project Structure

```
Assessment-of-Ukrainian-financial-statements/
??? AFS/             # Main Blazor WASM application
??? AFS.ComponentLibrary/     # Reusable Blazor components
?   ??? Components/
?   ?   ??? Charts/      # Chart components
?   ?   ??? Tables/        # Table components
?   ?   ??? TableComponents/   # Shared table parts
?   ??? Resources/   # Localization resources
??? AFS.Core/     # Business logic and services
    ??? Models/    # Data models
    ??? Services/     # Business services
    ??? Json/        # JSON serialization
```

## ?? Development Guidelines

### Code Style

- Follow standard C# conventions
- Use meaningful variable and method names
- Add XML documentation comments for public APIs
- Keep methods focused and single-purpose

### Blazor Components

- Use `@page` directive for routable components
- Inject services via `@inject`
- Follow the existing component naming pattern (`*Component.razor`)
- Ensure components are localization-ready

### Adding Financial Indicators

1. Create a model in `AFS.Core/Models/`
2. Add calculation service in `AFS.Core/Services/DataCalculations/`
3. Create a component in `AFS.ComponentLibrary/Components/Tables/` or `Charts/`
4. Add localized strings to resource files
5. Register service in `Program.cs`

### Localization

We support multiple languages (Ukrainian, English, German, Spanish, French, Russian):

1. Add keys to `AFS.ComponentLibrary/Resources/Resource.resx`
2. Add translations to language-specific `.resx` files
3. Use `@ResLoc["Key"]` in components

## ?? Testing

Before submitting:

```bash
# Build the project
dotnet build

# Run in development mode
dotnet run --project AFS

# Check for warnings
dotnet build --no-incremental
```

## ?? Commit Guidelines

Use clear, descriptive commit messages:

```
? Good:
- Add solvency ratio calculation component
- Fix chart rendering in mobile view
- Update Ukrainian translations for tables

? Avoid:
- Fix bug
- Update code
- Changes
```

### Commit Message Format

```
<type>: <subject>

[optional body]
```

**Types:**
- `feat:` New feature
- `fix:` Bug fix
- `docs:` Documentation changes
- `style:` Code style changes (formatting, etc.)
- `refactor:` Code refactoring
- `perf:` Performance improvements
- `test:` Adding tests
- `chore:` Build/tooling changes

## ?? Pull Request Process

1. **Update your branch**
   ```bash
   git fetch origin
   git rebase origin/main
   ```

2. **Test your changes thoroughly**

3. **Commit your changes**
   ```bash
   git add .
 git commit -m "feat: add working capital analysis component"
   ```

4. **Push to your fork**
   ```bash
   git push origin feature/your-feature-name
   ```

5. **Create a Pull Request**
   - Go to the original repository
   - Click "New Pull Request"
   - Select your branch
   - Fill in the PR template

### PR Checklist

- [ ] Code builds without errors
- [ ] No new warnings introduced
- [ ] Follows project code style
- [ ] Added/updated documentation if needed
- [ ] Added/updated localization strings
- [ ] Tested in browser (Chrome, Firefox, Edge)
- [ ] Tested responsive design (mobile/tablet)

## ?? Adding Language Support

To add a new language:

1. Create `Resource.[language-code].resx` in `AFS.ComponentLibrary/Resources/`
2. Translate all keys from `Resource.resx`
3. Add language to `CultureSelector.razor`
4. Update this CONTRIBUTING.md

## ?? Reporting Bugs

Use GitHub Issues with this information:

- **Description**: Clear description of the bug
- **Steps to Reproduce**: How to trigger the bug
- **Expected Behavior**: What should happen
- **Actual Behavior**: What actually happens
- **Browser**: Chrome/Firefox/Edge + version
- **Screenshots**: If applicable

## ?? Feature Requests

We love new ideas! Open an issue with:

- **Feature Description**: What you want to add
- **Use Case**: Why it's useful
- **Financial Context**: How it helps financial analysis
- **Priority**: Nice-to-have vs. essential

## ?? Resources

### Financial Analysis Background
- [Ukrainian Accounting Standards](https://mof.gov.ua/en/accounting-standards)
- Financial ratios and indicators documentation

### Technical Documentation
- [Blazor WebAssembly](https://learn.microsoft.com/aspnet/core/blazor/)
- [Radzen Blazor Components](https://blazor.radzen.com/)
- [.NET 9 Documentation](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-9/overview)

## ?? License

By contributing, you agree that your contributions will be licensed under the MIT License.

## ?? Questions?

Feel free to open a discussion or issue if you need help!

## ?? Good First Issues

Look for issues labeled:
- `good first issue` - Perfect for newcomers
- `help wanted` - We need assistance
- `documentation` - Improve docs
- `translation` - Add language support

## ?? Thank You!

Every contribution helps make financial analysis more accessible. Thank you for being part of this project!

---

**Happy Contributing!** ??
