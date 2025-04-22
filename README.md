
# ExpenseSplitterApp

## **Main Logic of the App**  
The ExpenseSplitterApp is designed to manage shared expenses among a group of people. Users can add who paid what and for whom, and the app calculates how much each person owes or is owed. The system outputs a simplified result indicating who should pay whom and how much to settle all debts in the most efficient way. This makes it ideal for roommates, group travels, or shared activities.


📱 Expense Splitter App (MVVM + Clean Architecture + SQLite)

This application is built using .NET MAUI with a clear separation of concerns and modular structure. It is designed for mobile-first scenarios with extended support for WinUI desktop, using SQLite as the local storage engine.

## 📂 Architecture Overview

🧩 MVVM STRUCTURE (Model-View-ViewModel)
- Implements the MVVM pattern to keep Views free of logic.
- ViewModels handle the interaction logic and communicate with services.
- Views only bind to ViewModels, ensuring testability and separation of concerns.

📁 LAYERS
- **DataAccess Layer**: Contains Repositories that handle SQLite-based CRUD operations.
- **Service Layer**: Handles business logic, injected into ViewModels via constructor. This keeps the app clean and scalable.
- **ViewModels**: Handle UI-related state and commands. They do not access data sources directly.

## 📱 Mobile-Conform Data Storage

- Uses SQLite as the data provider for persistent local storage.
- Repository pattern abstracts away platform details, making code mobile-friendly and testable.
- Async methods used throughout for efficient database interaction.

## ⚙️ Program State Pattern in ViewModels

- ViewModels maintain an `AppState` enum that controls program flow.
- States like `Observe`, `Action`, and `Result` help determine which part of the UI should be visible.
- Used with property binding to dynamically update UI without manual triggers.

## 🔧 Helpers for UI Binding and Logic

- Custom converters for binding:
  - **Enum ↔ Bool**: Used for toggles and selections.
  - **Bool ↔ String**: Used to convert internal logic to user-friendly labels.
- Makes Views cleaner and minimizes the need for logic in XAML.

## 🖼️ Cross-Platform UI Adjustments

- Supports swipe gestures on mobile devices for intuitive UX.
- On WinUI desktop: swipe menus don't work, so **alternate buttons** are shown conditionally.
- Responsive design adapts to screen size and platform.

## 🌙 Dark Mode & Custom Styling

- Fully supports dark mode with a personalized color palette.
- Overridden default .NET MAUI styles to implement consistent theming.
- Custom resource dictionary for Light and Dark modes.

## 🧭 Navigation

- Navigation handled using .NET MAUI Shell or NavigationPage.
- Different views are opened based on command execution in ViewModels.
- Ensures loose coupling between UI and navigation logic.

##🚀 Summary

The Expense Splitter App is a clean, modular .NET MAUI application showcasing:
- MVVM with program state control.
- Service and repository layer separation.
- Custom UI behaviors for cross-platform support.
- SQLite storage with async repository methods.
- Themed UI with light/dark mode support.

This project is demonstrating professional-grade architecture and practices in mobile or cross-platform development.
