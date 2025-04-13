# 🌟 Field Survey MAUI App

![.NET MAUI](https://img.shields.io/badge/built%20with-.NET%20MAUI-512BD4?logo=dotnet)
![C#](https://img.shields.io/badge/language-C%23-239120?logo=c-sharp)
![Cross Platform](https://img.shields.io/badge/platform-iOS%20%7C%20Android%20%7C%20Windows%20%7C%20macOS-lightgrey)

A robust cross-platform mobile application designed for conducting field surveys following disasters. Built with **.NET MAUI** using the MVVM architecture pattern, this app enables field workers to collect critical data through customizable surveys with various question types including geolocation capture.

---

## ✨ FEATURES

- ✅ **Cross-platform** - Works on iOS, Android, Windows, and macOS
- ✅ **Authentication** - Secure login system
- ✅ **Dynamic surveys** - Support for multiple question types:
  - Text input
  - Numeric data
  - Date selection
  - Multiple choice
  - Location data (GPS coordinates)
- ✅ **Offline capability** - Submit when connectivity is restored
- ✅ **Survey management** - View all previously submitted surveys
- ✅ **MVVM architecture** - Clean separation of concerns
- ✅ **Dependency Injection** - For testable and maintainable code

---

## 🏗️ ARCHITECTURE OVERVIEW

The application follows the MVVM (Model-View-ViewModel) architecture pattern:

```
FieldSurveyMAUIApp/
├── Models/               # Data models
├── Views/                # UI pages/components
├── ViewModels/           # Business logic layer
├── Services/             # Backend communication & business services
│   └── Interfaces/       # Service contracts
└── Converters/           # Value converters for UI
```

### Architecture Diagram

```mermaid
graph TD
    subgraph "UI Layer"
        A[App.xaml] --> B[AppShell.xaml]
        B --> V1[LoginPage]
        B --> V2[HomePage]
        B --> V3[SurveyPage]
        B --> V4[FilledSurveysPage]
    end

    subgraph "ViewModel Layer"
        V1 --> VM1[LoginViewModel]
        V2 --> VM2[HomeViewModel]
        V3 --> VM3[SurveyViewModel]
        V4 --> VM4[FilledSurveysViewModel]
        
        VM1 & VM2 & VM3 & VM4 --> VMB[BaseViewModel]
    end

    subgraph "Service Layer"
        VM1 & VM2 --> S1[AuthService]
        VM2 & VM3 & VM4 --> S2[SurveyService]
        
        S1 --> I1[IAuthService]
        S2 --> I2[ISurveyService]
    end
    
    subgraph "Model Layer"
        S2 --> M1[Survey]
        M1 --> M2[Question]
        S2 --> M3[SurveyResponse]
    end
    
    subgraph "Platform Specific"
        P1[iOS]
        P2[Android]
        P3[MacCatalyst]
        P4[Windows]
    end

    %% Dependency Injection
    MP[MauiProgram.cs] --> S1 & S2
    MP --> VM1 & VM2 & VM3 & VM4
    MP --> V1 & V2 & V3 & V4
    
    %% Templates and Converters
    C1[QuestionTemplateSelector] --> V3
    C2[ValueConverters] --> V1 & V2 & V3 & V4
    
    %% External API
    S2 <--> API[External Survey API]
    
    classDef viewModels fill:#f9f,stroke:#333,stroke-width:2px
    classDef services fill:#bbf,stroke:#333,stroke-width:1px
    classDef models fill:#bfb,stroke:#333,stroke-width:1px
    classDef views fill:#fbb,stroke:#333,stroke-width:1px
    
    class VM1,VM2,VM3,VM4,VMB viewModels
    class S1,S2,I1,I2 services
    class M1,M2,M3 models
    class V1,V2,V3,V4,A,B views
```

---

## 📱 APP SCREENSHOTS

### iOS Screenshots
| Login Screen | Survey List | Survey Form | Location Capture |
|-------------|-------------|-------------|-----------------|
| ![iOS Login](./screenshots/ios_login.png) | ![iOS Surveys](./screenshots/ios_surveys.png) | ![iOS Survey Form](./screenshots/ios_survey_form.png) | ![iOS Location](./screenshots/ios_location.png) |

### Android Screenshots
| Login Screen | Survey List | Survey Form | Location Capture |
|-------------|-------------|-------------|-----------------|
| ![Android Login](./screenshots/android_login.png) | ![Android Surveys](./screenshots/android_surveys.png) | ![Android Survey Form](./screenshots/android_survey_form.png) | ![Android Location](./screenshots/android_location.png) |

### macOS Screenshots
| Login Screen | Survey List | Survey Form | Location Capture |
|-------------|-------------|-------------|-----------------|
| ![macOS Login](./screenshots/macos_login.png) | ![macOS Surveys](./screenshots/macos_surveys.png) | ![macOS Survey Form](./screenshots/macos_survey_form.png) | ![macOS Location](./screenshots/macos_location.png) |

### Completed Surveys View
| iOS | Android | macOS |
|-----|---------|-------|
| ![iOS Completed](./screenshots/ios_completed.png) | ![Android Completed](./screenshots/android_completed.png) | ![macOS Completed](./screenshots/macos_completed.png) |

> **Note:** Place your actual application screenshots in the `/screenshots` directory using the naming convention shown above.

---

## 🛠️ PROJECT STRUCTURE

### 🔹 Views (UI Layer)
- [LoginPage](Views/LoginPage.xaml) - User authentication
- [HomePage](Views/HomePage.xaml) - Displays available surveys
- [SurveyPage](Views/SurveyPage.xaml) - Dynamic survey form
- [FilledSurveysPage](Views/FilledSurveysPage.xaml) - Shows submitted responses

### 🔹 ViewModels (Business Logic)
- [BaseViewModel](ViewModels/BaseViewModel.cs) - Common ViewModel functionality
- [LoginViewModel](ViewModels/LoginViewModel.cs) - Login logic
- [HomeViewModel](ViewModels/HomeViewModel.cs) - Survey listing
- [SurveyViewModel](ViewModels/SurveyViewModel.cs) - Survey interaction
- [FilledSurveysViewModel](ViewModels/FilledSurveysViewModel.cs) - Response management

### 🔹 Models (Data)
- [Survey](Models/Survey.cs) - Survey structure
- [Question](Models/Question.cs) - Question with various types
- [SurveyResponse](Models/SurveyResponse.cs) - Submitted response data

### 🔹 Services
- [AuthService](Services/AuthService.cs) - User authentication
- [SurveyService](Services/SurveyService.cs) - API communication for surveys

---

## 🚀 GETTING STARTED

### Prerequisites
- .NET 9.0 SDK
- Visual Studio 2022 or Visual Studio for Mac (with .NET MAUI workload)
- Android SDK for Android development
- Xcode (for iOS/macOS development)

### Setup & Run
1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/FieldSurveyMAUIApp.git
   cd FieldSurveyMAUIApp
   ```

2. **Open in Visual Studio**
   - Double-click the `.sln` file or open via Visual Studio

3. **Build the project**
   ```bash
   dotnet build
   ```

4. **Run on desired platform**
   - Select target platform (Android, iOS, Windows, macOS)
   - Press F5 or click Run

---

## 🧰 TECHNICAL IMPLEMENTATION

### Authentication
- Token-based authentication with secure storage
- Login screen with validation

### Survey Management
- Dynamic rendering of different question types
- Real-time location capture with device GPS
- Form validation for required questions

### API Communication
- RESTful API integration
- Async operations for responsive UI
- Error handling and retry mechanisms

---

## 📋 REQUIREMENTS

- **iOS**: 15.0 or later
- **Android**: API level 21 (Android 5.0) or later
- **Windows**: Windows 10 version 17763.0 or later
- **macOS**: macOS Catalina 15.0 or later

---

## 🤝 CONTRIBUTING

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

---

## 📄 LICENSE

This project is licensed under the MIT License - see the LICENSE file for details.

---

## ACKNOWLEDGMENTS

- Built with [.NET MAUI](https://dotnet.microsoft.com/apps/maui)
- Using [CommunityToolkit.Mvvm](https://github.com/CommunityToolkit/dotnet) for MVVM implementation
