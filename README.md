# E-Commerce Platform with CQRS and Event Sourcing

## Overview
This project is an e-commerce platform built using CQRS (Command Query Responsibility Segregation) and Event Sourcing. The platform allows users to create accounts, add products to their carts, and place orders. The admin area provides features for managing users, products, orders, and reporting.

## Technologies Used
* C#
* .NET
* React
* .NET Core API

## Features
* User registration and login
* Product catalog management
* Shopping cart management
* Order management
* Payment processing
* Event sourcing for auditing and debugging purposes
* Admin area for managing users, products, orders, and reporting
* Reporting features for sales, customers, products, and orders

## Technical Details
* Built using C# and .NET Core
* Uses CQRS and Event Sourcing for handling business logic and storing events
* Utilizes a SQL Server database for storing data
* Implements a RESTful API for interacting with the platform
* Uses JSON Web Tokens (JWT) for authentication and authorization

## API Endpoints
### Shared API
* POST /Account/Login: Log into app (regardless of role)
* POST /Account/Logout: Log out of app
* POST /Account/Register


### Admin API
* GET /admin/users: Get a list of all users
* GET /admin/users/{id}: Get a user by ID
* POST /admin/users: Create a new user
* PUT /admin/users/{id}: Update a user
* DELETE /admin/users/{id}: Delete a user
* GET /admin/products: Get a list of all products
* GET /admin/products/{id}: Get a product by ID
* POST /admin/products: Create a new product
* PUT /admin/products/{id}: Update a product
* DELETE /admin/products/{id}: Delete a product
* GET /admin/orders: Get a list of all orders
* GET /admin/orders/{id}: Get an order by ID
* POST /admin/orders: Create a new order
* PUT /admin/orders/{id}: Update an order
* DELETE /admin/orders/{id}: Delete an order

### User API
* GET /users/orders: Get a list of all orders for the current user
* GET /users/orders/{id}: Get an order by ID for the current user
* POST /users/orders: Create a new order for the current user
* PUT /users/orders/{id}: Update an order for the current user
* DELETE /users/orders/{id}: Delete an order for the current user

## Reporting API
* GET /reports/sales: Get sales data for a given date range
* GET /reports/customers: Get customer data for a given date range
* GET /reports/products: Get product data for a given date range
* GET /reports/orders: Get order data for a given date range

## Database Schema
### Users Table
* id (primary key)
* username
* email
* password
* role (admin or user)

### Products Table
* id (primary key)
* name
* description
* price
* inventory_level

### Orders Table
* id (primary key)
* user_id (foreign key referencing the Users table)
* order_date
* total
* status (pending, shipped, delivered, etc.)

### Order Items Table
* id (primary key)
* order_id (foreign key referencing the Orders table)
* product_id (foreign key referencing the Products table)
* quantity

## Event Store
* Events Table
* id (primary key)
* event_type (e.g. OrderPlaced, ProductAdded, etc.)
* event_data (JSON data containing event details)
* created_at (timestamp when event was created)