# 💱 Fx.Convert – Foreign Exchange Conversion System (.NET 8, Clean Architecture)

Welcome to **Fx.Convert**, a modular FX conversion system built with **.NET 8** and structured using **Clean Architecture**. This project demonstrates how to model currency exchange logic in a clean, extensible way, and is designed to support technical interviews by showcasing thoughtful design, domain modeling, and fallback strategies.

---

## 🎯 Purpose

This exercise is intended to evaluate:

- Understanding of FX exchange domain
- Application of Clean Architecture principles
- Design decisions and trade-offs
- Communication of technical reasoning

Syntax and style are secondary to clarity, maintainability, and architectural integrity.

---

## 🌍 Domain Overview

Foreign exchange (FX) involves converting an amount from one currency to another using standardized currency pairs and exchange rates.

### Key Concepts

- **Currency Pair**: ISO format, e.g. `EUR/DKK`
  - `EUR` → base currency  
  - `DKK` → quote currency
- **Exchange Rate**: Indicates how much of the quote currency is received for one unit of the base currency  
  - Example: `EUR/DKK = 7.4394` means `1 EUR = 7.4394 DKK`

---

## 🛠️ Tech Stack

- **Framework**: [.NET 8](https://learn.microsoft.com/en-us/dotnet/core/dotnet-eight)
- **Architecture**: Clean Architecture
- **Language**: C#
- **Testing**: xUnit
- **Tooling**: `dotnet CLI`
- **External API**: [Free Forex API](https://www.freeforexapi.com/)

---

## 🔧 API Integration & Fallback

The application fetches live exchange rates using the **Free Forex API** for demonstration purposes.

If the API is unavailable or fails, the application gracefully falls back to **static exchange rates** defined in `appsettings.json`.

This ensures the system remains functional and testable even without external connectivity.

---

## 🧱 Solution Structure

```bash
Fx.Convert/
├── Fx.Convert.Domain/         # Core domain models and business rules
├── Fx.Convert.Application/    # Application logic and use cases
├── Fx.Convert.Infrastructure/ # External dependencies (e.g. Free Forex API)
├── Fx.Convert.ConsoleApp/     # Entry point (Console application)
├── Fx.Convert.Framework/      # Shared utilities, abstractions, and constants
├── Fx.Convert.<>.Tests.Unit/  # Unit tests
└── README.md
