# Talabat E-Commerce API
The Talabat E-Commerce API is a robust and scalable backend solution designed to power an online store. Built with a focus on performance, 
security, and scalability, this API provides a complete set of features to manage various e-commerce operations, including user authentication, 
product catalog management, order processing, payment integration, and more.
_________________________________________________________

## Key Features
- **User Management**: Secure user registration, authentication (JWT-based), and role-based access control.
- **Product Management**: Full CRUD functionality for products with features like product details, pricing, availability, and images.
- **Order Processing**: Manage order creation, order status tracking, and user order history.
- **Shopping Cart**: Users can add products to their basket, update quantities, and securely proceed to checkout.
- **Payment Integration**: Integrated with Stripe for secure payment processing, supporting various methods.
- **Shipping**: Handle shipping addresses and calculate shipping costs.
- **Secure APIs**: Implements best practices for secure API development, including input validation and rate limiting.
- **Error Handling**: Comprehensive error handling and logging for better user experience and debugging.
_________________________________________________________
## Technologies Used
- **ASP.NET Core 6**
- **ASP.NET Core**: Backend API framework.
- **Entity Framework Core**: ORM for database interaction.
- **SQL Server**: Database management system.
- **JWT (JSON Web Token)**: Secure user authentication.
- **Stripe**: Payment gateway integration.
- **AutoMapper**: Object mapping for DTOs and entities.
- **Swagger**: API documentation and testing.
- **Repository Pattern**: Clean architecture for separation of concerns.
_________________________________________________________
## Project Architecture
This project follows a clean architecture pattern with separation of concerns across different layers:

- **API Layer**: Contains the Controlles , DTOs , Extentions , Errors , Helpers And MidelWares 
- **Core Layer**: Contains the Entities , Specifications , Repositories And Services 
- **Repository Layer**: Handles data access, implements repository pattern.
- **Service Layer**: Contains the application services for implementing business logic and domain
_________________________________________________________
## Controllers 
### Controllers Overview
This project includes several key controllers responsible for handling various aspects of the API,
including user authentication, order management,
product catalog, and payment processing. Below is a description of each controller along with the routes and functionality they provide.

### 1. AccountsController
This controller manages user registration, login, and user-related operations.

- ### Routes : 

- **POST /api/Accounts/Register**: Registers a new user.
- **POST /api/Accounts/Login**: Logs in an existing user.
- **GET /api/Accounts/GetCurretUser**: Retrieves the currently logged-in user's information.
- **GET /api/Accounts/Address**: Retrieves the user's saved address.
- **PUT /api/Accounts/Address**: Updates the user's address.
- **GET /api/Accounts/emailExists**: Checks if a given email is already registered.

- ### Key Features :

- Uses UserManager and SignInManager for handling identity operations.
- JWT-based authentication for login and user management.
- Supports role-based access with [Authorize] attributes.


### 2. BasketController
Handles all operations related to a user's shopping basket.

- ### Routes : 

- **GET /api/Basket/{id}**: Retrieves the basket for a given user ID. If no basket exists, it creates a new one.
- **POST /api/Basket**: Updates the basket with new or modified items.
- **DELETE /api/Basket/{id}**: Deletes a user's basket.

- ### Key Features :

- Uses the repository pattern (IBasketRepository) to interact with the basket data.
- AutoMapper is used to map between CustomerBasket and CustomerBasketDto.

### 3. OrdersController
Manages the creation and retrieval of orders for the logged-in user.

- ### Routes : 

- **POST /api/Orders**: Creates a new order based on the basket and shipping details.
- **GET /api/Orders**: Retrieves all orders for the logged-in user.
- **GET /api/Orders/{id}**: Retrieves a specific order by its ID for the logged-in user.
- **GET /api/Orders/DeliveryMethods**: Retrieves all available delivery methods.

- ### Key Features :

- Uses IOrderService to manage order creation and retrieval.
- AutoMapper maps DTOs to and from core entities like Order and Address.
- Provides detailed error handling with custom error responses.

### 4. PaymentsController
Handles payment-related operations, including integrating with Stripe for payment intents.

- ### Routes : 

- **POST /api/Payments/{BasketId}**: Creates or updates a payment intent for the specified basket.
- **POST /api/Payments/webhook**: Handles Stripe webhooks for payment status updates.

- ### Key Features :

- Integrates Stripe's SDK to handle payment processing.
- Uses a Stripe webhook to update payment statuses for success or failure.
- Provides a customer-friendly error response when payment issues arise.

### 5. ProductsController
Provides API endpoints for retrieving product information, brands, and categories.

- ### Routes : 

- **GET /api/Products**: Retrieves all products with support for filtering, sorting, and pagination.
- **GET /api/Products/{id}**: Retrieves a specific product by ID.
- **GET /api/Products/brands**: Retrieves all available product brands.
- **GET /api/Products/categories**: Retrieves all available product categories.

- ### Key Features :

- Uses specifications (such as ProductWithBrandAndCategorySpec) to apply complex filtering logic.
- Provides a caching mechanism for products, brands, and categories to improve performance.
- Pagination and sorting are handled through ProductSpecParams.

### 6. WeatherForecastController
A simple controller that demonstrates returning sample data.

- ### Routes : 

- **GET /api/WeatherForecast**: Returns a list of mock weather data for demonstration purposes.

- ### Key Features :

- Demonstrates basic API functionality and how controllers handle HTTP GET requests.

### 7. ErrorsController
Handles error responses for the API. This controller is useful for generating standardized error responses.

- ### Routes : 

- **GET /errors/{code}**: Returns an appropriate error response based on the status code.

- ### Key Features :

- Uses ApiExplorerSettings(IgnoreApi = true) to prevent Swagger from documenting this controller.
- Provides detailed error messages for common HTTP status codes like 404 and 401.

_________________________________________________________
  
