# Assessment of Ukrainian Financial Statements (UFIN)

[![.NET 9](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Blazor WebAssembly](https://img.shields.io/badge/Blazor-WebAssembly-512BD4?logo=blazor)](https://blazor.net/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![Deploy to GitHub Pages](https://github.com/whitewAw/Assessment-of-Ukrainian-financial-statements/actions/workflows/main.yml/badge.svg)](https://github.com/whitewAw/Assessment-of-Ukrainian-financial-statements/actions/workflows/main.yml)

> 🎯 A comprehensive Progressive Web App (PWA) for analyzing Ukrainian financial statements, built with Blazor WebAssembly and optimized for performance with AOT compilation.

## 🌐 [Live Demo](https://whitewaw.github.io/Assessment-of-Ukrainian-financial-statements/)

---

## 📋 Table of Contents

- [Overview](#-overview)
- [Features](#-features)
- [Technology Stack](#️-technology-stack)
- [Getting Started](#-getting-started)
- [Project Structure](#-project-structure)
- [Financial Analysis Features](#-financial-analysis-features)
- [Contributing](#-contributing)
- [Deployment](#-deployment)
- [License](#-license)

---

## 🎯 Overview

This application provides a powerful, browser-based tool for comprehensive analysis of Ukrainian financial statements. Built with modern web technologies, it runs entirely in the browser using WebAssembly - no server required!

### Why This Project?

- ✅ **Offline-first** - Works without internet after initial load
- ✅ **Privacy-focused** - All calculations happen in your browser
- ✅ **No installation** - Just open and use
- ✅ **Multi-language** - Supports 6 languages (Ukrainian, English, German, Spanish, French, Russian)
- ✅ **Professional-grade** - Comprehensive financial analysis tools
- ✅ **Open Source** - Free to use, modify, and contribute

---

## ✨ Features

### 📊 Financial Analysis
- **Balance Sheet Analysis** (Form 1)
- **Income Statement Analysis** (Form 2)
- **Liquidity Ratios** - Current, Quick, Cash ratios
- **Solvency Indicators** - Debt-to-equity, Coverage ratios
- **Profitability Metrics** - ROA, ROE, Profit margins
- **Business Activity Indicators** - Turnover ratios, Activity cycles
- **Financial Stability Assessment** - Classification and trends
- **Working Capital Analysis** - Efficiency and turnover time
- **Fixed Assets Evaluation** - Availability, movement, efficiency
- **Interactive Charts** - Visual representation of financial data

### 🌍 Internationalization
- Ukrainian (Українська)
- English
- German (Deutsch)
- Spanish (Español)
- French (Français)
- Russian (Русский)

### 💾 Data Management
- **Local Storage** - Save your data securely in browser
- **Import/Export** - JSON-based data portability
- **Sample Data** - Pre-loaded examples for learning

### ⚡ Performance
- **AOT Compilation** - Ahead-of-time compiled for maximum speed
- **PWA Support** - Install as standalone app
- **Optimized Bundle** - Compressed and trimmed for fast loading
- **Service Worker** - Offline caching for instant access

---

## 🛠️ Technology Stack

| Technology | Version | Purpose |
|------------|---------|---------|
| [.NET](https://dotnet.microsoft.com/) | 9.0 | Runtime and SDK |
| [Blazor WebAssembly](https://blazor.net/) | 9.0 | UI Framework |
| [Radzen Blazor](https://blazor.radzen.com/) | 8.1.5 | Component Library |
| [C#](https://docs.microsoft.com/en-us/dotnet/csharp/) | 13.0 | Programming Language |
| WebAssembly | Latest | Execution Environment |

### Build Features
- ✅ AOT (Ahead-of-Time) Compilation
- ✅ IL Trimming for smaller bundles
- ✅ Brotli Compression
- ✅ Native WASM optimization

---

## 🚀 Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Any modern browser (Chrome, Firefox, Edge, Safari)
- (Optional) [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

### Local Development

1. **Clone the repository**
    ```bash
    git clone https://github.com/whitewAw/Assessment-of-Ukrainian-financial-statements.git
    cd Assessment-of-Ukrainian-financial-statements
    ```

2. **Restore dependencies**
    ```bash
    dotnet restore
    ```

3. **Install WebAssembly workload**
    ```bash
    dotnet workload install wasm-tools
    ```

4. **Run the application**
    ```bash
    dotnet run --project AFS
    ```

5. **Open in browser**
    ```
    https://localhost:7157
    ```

### Build for Production

```bash
dotnet publish AFS/AFS.csproj -c Release -o publish
```

---

## 📁 Project Structure

```
Assessment-of-Ukrainian-financial-statements/
├── 📂 AFS/          # Main Blazor WebAssembly app
│   ├── Program.cs  # Application entry point
│   ├── App.razor       # Root component with router
│   └── wwwroot/ # Static assets
│
├── 📂 AFS.ComponentLibrary/# Reusable components
│ ├── Components/
│   │   ├── Charts/     # Chart components
│   │   ├── Tables/    # Data table components
│   │   └── TableComponents/        # Shared table elements
│   └── Resources/       # Localization files (.resx)
│
├── 📂 AFS.Core/      # Business logic
│   ├── Models/           # Data models
│   │   ├── Form1.cs         # Balance sheet model
│   │   ├── Form2.cs                # Income statement model
│   │   └── TablsModels/   # Table-specific models
│   ├── Services/    # Business services
│   │   ├── DataCalculations/       # Financial calculation services
│   │   ├── LocalStorageHandler.cs  # Browser storage
│   │   └── JsInterop.cs    # JavaScript interop
│   └── Json/       # JSON serialization
│
├── 📂 .github/         # GitHub configuration
│   ├── workflows/         # CI/CD workflows
│   │   ├── main.yml    # Deployment workflow
│   │   └── pr-validation.yml       # PR quality checks
│   └── ISSUE_TEMPLATE/          # Issue templates
│
├── 📄 CONTRIBUTING.md    # Contribution guidelines
├── 📄 CODE_OF_CONDUCT.md      # Community standards
├── 📄 LICENSE               # MIT License
└── 📄 README.md       # This file
```

---

## 📊 Financial Analysis Features

### Balance Sheet Analysis (Form 1)
- Asset composition and structure
- Current vs. Non-current assets
- Equity and liabilities analysis
- Year-over-year comparisons

### Income Statement Analysis (Form 2)
- Revenue analysis
- Operating expenses
- Net profit calculations
- Profitability trends

### Financial Ratios

#### Liquidity Indicators
- Current Ratio
- Quick Ratio
- Cash Ratio
- Working Capital

#### Solvency Ratios
- Debt-to-Equity Ratio
- Interest Coverage
- Financial Leverage
- Solvency Recovery/Loss Ratio

#### Business Activity
- Asset Turnover
- Receivables Turnover
- Inventory Turnover
- Payables Turnover

#### Financial Stability
- Autonomy Coefficient
- Financial Independence
- Capital Structure Analysis
- Stability Classification

### Interactive Visualizations
- 📊 Pie charts for composition analysis
- 📈 Line charts for trend analysis
- 📉 Bar charts for comparative data
- 🎯 Donut charts for proportions

---

## 🤝 Contributing

We welcome contributions from the community! Whether you're fixing bugs, adding features, improving documentation, or translating to new languages - every contribution helps.

### Quick Start
1. Read [CONTRIBUTING.md](CONTRIBUTING.md)
2. Check [CODE_OF_CONDUCT.md](CODE_OF_CONDUCT.md)
3. Look for [good first issues](https://github.com/whitewAw/Assessment-of-Ukrainian-financial-statements/issues?q=is%3Aissue+is%3Aopen+label%3A%22good+first+issue%22)
4. Fork, code, and submit a PR!

### Ways to Contribute
- 🐛 **Report bugs** - [Create an issue](https://github.com/whitewAw/Assessment-of-Ukrainian-financial-statements/issues/new?template=bug_report.md)
- 💡 **Suggest features** - [Request a feature](https://github.com/whitewAw/Assessment-of-Ukrainian-financial-statements/issues/new?template=feature_request.md)
- 🌍 **Add translations** - Help support more languages
- 📝 **Improve docs** - Documentation is always appreciated
- 💻 **Submit PRs** - Fix bugs or implement features

---

## 🚀 Deployment

### GitHub Pages (Automated)

The project includes automated deployment to GitHub Pages using GitHub Actions.

#### How It Works

1. **Push to main branch** → Triggers deployment
2. **Workflow runs** → Builds with AOT compilation
3. **Deploys to gh-pages** → Available at your GitHub Pages URL

#### Workflow Features
- ✅ .NET 9 with AOT compilation
- ✅ WebAssembly optimization
- ✅ Service worker hash fixing
- ✅ Automatic base path handling
- ✅ PWA manifest generation

See [.github/WORKFLOW_IMPROVEMENTS.md](.github/WORKFLOW_IMPROVEMENTS.md) for detailed workflow documentation.

#### Manual Deployment

```bash
# Build the project
dotnet publish AFS/AFS.csproj -c Release -o release

# Update base path (replace with your repo name)
sed -i 's|<base href="/" />|<base href="/your-repo-name/" />|g' \
  release/wwwroot/index.html

# Deploy release/wwwroot to your hosting provider
```

### Alternative Hosting Options

- **Azure Static Web Apps** - [Guide](https://learn.microsoft.com/azure/static-web-apps/deploy-blazor)
- **Netlify** - Simply drag & drop `wwwroot` folder
- **Vercel** - Connect your GitHub repository
- **Firebase Hosting** - Use Firebase CLI
- **Any static hosting** - Upload `wwwroot` contents

---

## 📚 Documentation

### For Users
- [Live Application](https://whitewaw.github.io/Assessment-of-Ukrainian-financial-statements/)
- [Financial Analysis Guide](docs/FINANCIAL_ANALYSIS.md) _(coming soon)_
- [User Manual](docs/USER_MANUAL.md) _(coming soon)_

### For Developers
- [Contributing Guide](CONTRIBUTING.md)
- [Architecture Overview](MIGRATION_SUMMARY.md)
- [Workflow Documentation](.github/WORKFLOW_IMPROVEMENTS.md)
- [API Documentation](docs/API.md) _(coming soon)_

### Technical Resources
- [Blazor WebAssembly Docs](https://learn.microsoft.com/aspnet/core/blazor/)
- [.NET 9 What's New](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-9/overview)
- [Radzen Blazor Components](https://blazor.radzen.com/)
- [Ukrainian Accounting Standards](https://mof.gov.ua/en/accounting-standards)

---

## 🔒 Privacy & Security

- ✅ **No data collection** - All processing happens locally
- ✅ **No server communication** - Fully client-side application
- ✅ **Secure storage** - Browser's local storage only
- ✅ **Open source** - Fully auditable code
- ✅ **Regular updates** - Active maintenance and security patches

---

## 🌟 Acknowledgments

### Built With
- [Blazor](https://blazor.net/) - Microsoft's web framework
- [Radzen](https://blazor.radzen.com/) - Blazor component library
- [Open Iconic](https://useiconic.com/open/) - Icon set

### Inspired By
- Ukrainian accounting standards and practices
- Financial analysis methodologies
- Community feedback and contributions

---

## 📄 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

### What This Means
- ✅ Free to use commercially
- ✅ Free to modify and distribute
- ✅ Free to use privately
- ✅ Must include license and copyright notice

---

## 📞 Support & Community

### Get Help
- 💬 [GitHub Discussions](https://github.com/whitewAw/Assessment-of-Ukrainian-financial-statements/discussions) - Ask questions
- 🐛 [Issues](https://github.com/whitewAw/Assessment-of-Ukrainian-financial-statements/issues) - Report bugs
- 📧 Contact maintainers - See GitHub profile

### Stay Updated
- ⭐ Star this repository
- 👀 Watch for releases
- 🔔 Follow for updates

---

## 🗺️ Roadmap

### Completed ✅
- [x] Multi-language support (6 languages)
- [x] Comprehensive financial analysis
- [x] PWA with offline support
- [x] AOT compilation optimization
- [x] Automated CI/CD deployment
- [x] Component-based architecture

### In Progress 🚧
- [ ] Unit test coverage
- [ ] End-to-end testing
- [ ] Enhanced mobile responsiveness
- [ ] Dark theme support

### Planned 🎯
- [ ] PDF report generation
- [ ] Excel export/import
- [ ] Historical data comparison
- [ ] Advanced forecasting tools
- [ ] Multi-company analysis
- [ ] Cloud synchronization (optional)

---

## 📊 Project Statistics

![GitHub stars](https://img.shields.io/github/stars/whitewAw/Assessment-of-Ukrainian-financial-statements?style=social)
![GitHub forks](https://img.shields.io/github/forks/whitewAw/Assessment-of-Ukrainian-financial-statements?style=social)
![GitHub issues](https://img.shields.io/github/issues/whitewAw/Assessment-of-Ukrainian-financial-statements)
![GitHub pull requests](https://img.shields.io/github/issues-pr/whitewAw/Assessment-of-Ukrainian-financial-statements)

---

## 🙏 Thank You

Contributions make the open-source community an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**!

See [CONTRIBUTING.md](CONTRIBUTING.md) for ways to get started.

---

<div align="center">

**Made with ❤️ by the community**

[Report Bug](https://github.com/whitewAw/Assessment-of-Ukrainian-financial-statements/issues/new?template=bug_report.md) · 
[Request Feature](https://github.com/whitewAw/Assessment-of-Ukrainian-financial-statements/issues/new?template=feature_request.md) · 
[Contribute](CONTRIBUTING.md)

</div>
