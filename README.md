# PersonalFinance

# Language: C#

## Personal Finance Tracker Project Overview
The Personal Finance Tracker is a web application designed to help users manage their finances effectively. The application allows users to track their income, expenses, budgets, and savings goals.

## Technologies Used
# C#
# .NET
# React
# .NET Core API
# Features
# User authentication
# Income tracking
# Expense tracking
# Budgeting
# Savings goals
# Transaction history
# Data visualization
II. Database Design
Database Schema
The database schema consists of the following tables:

Users: stores user information
IncomeSources: stores income sources
Expenses: stores expenses
Budgets: stores budgets
SavingsGoals: stores savings goals
Transactions: stores transactions

## API Endpoints
User Endpoints
POST /api/users - Create user
GET /api/users/{id} - Get user by id
PUT /api/users/{id} - Update user
DELETE /api/users/{id} - Delete user

Income Endpoints
GET /api/income - Get all income sources
POST /api/income - Create income source
GET /api/income/{id} - Get income source by id
PUT /api/income/{id} - Update income source
DELETE /api/income/{id} - Delete income source

Expense Endpoints
GET /api/expenses - Get all expenses
POST /api/expenses - Create expense
GET /api/expenses/{id} - Get expense by id
PUT /api/expenses/{id} - Update expense
DELETE /api/expenses/{id} - Delete expense

Budget Endpoints
GET /api/budgets - Get all budgets
POST /api/budgets - Create budget
GET /api/budgets/{id} - Get budget by id
PUT /api/budgets/{id} - Update budget
DELETE /api/budgets/{id} - Delete budget

Savings Goal Endpoints
GET /api/savings-goals - Get all savings goals
POST /api/savings-goals - Create savings goal
GET /api/savings-goals/{id} - Get savings goal by id
PUT /api/savings-goals/{id} - Update savings goal
DELETE /api/savings-goals/{id} - Delete savings goal

Transaction Endpoints
GET /api/transactions - Get all transactions
POST /api/transactions - Create transaction
GET /api/transactions/{id} - Get transaction by id
PUT /api/transactions/{id} - Update transaction
DELETE /api/transactions/{id} - Delete transaction

## React Frontend
Components
# User Profile
# Income Tracker
# Expense Tracker
# Budget Tracker
# Savings Goal Tracker
# Transaction History
# Pages
Dashboard
Income
Expenses
Budgets
Savings Goals
Transactions
V. Tasks
Set up .NET Core API project
Implement user authentication using .NET Core Identity
Create database schema
Implement API endpoints
Build React frontend
Integrate React frontend with API
Test and debug application
## Variable Naming Conventions
* Use explicit type names for variable declarations.
* For single-line initialization, use the following format:
SomeClass variable = new();
* For multi-line initialization, use the following format:
SomeClass variable; variable = new SomeClass();
* use _camelCase for private field/property names
* use PascalCase for public or protected field/property names
* Avoid using abbreviations or acronyms unless they are widely recognized.
* When suggesting a viariable for a DateTime use DateCreated for creation and DateUpdated for update variable names. Using the correct capitalization based on public/private/method variable.

## Code Formatting
* Use 4 spaces for indentation.
* Use blank lines to separate logical sections of code.
* Use curly braces to enclose code blocks, even if they are single-line.

## Code Organization
* Method Length: Methods should be concise and focused on a single task. Aim for a maximum of 20-30 lines of code per method.
* Break down long methods: If a method is too long or complex, break it down into smaller, more manageable pieces. This makes the code easier to read and maintain.
* Single Responsibility Principle: Each method should have a single responsibility and should not be responsible for multiple, unrelated tasks.

## Class and Method Naming Conventions
* Use PascalCase for class and method names.
* Use descriptive names that indicate the purpose of the class or method.

## Other Conventions
* Use `using` statements instead of fully qualified type names.
* Avoid using `static` classes unless necessary.
