# StripeBookStore
Simple e-commerce application, that allows a customer to purchase a book utilizing Stripe as our Payments infraestructure. The solution consist of 2 main projects (Backend server and Mobile client apps for iOS and Android):

### About Solution
The solution leverages Stripe’s .NET Library, which is used on the server side to handle creating payment intents for products selected in the client app; thus providing a client secret that allows our users to confirm their payments on the client once they have created and selected a valid card payment method. I used the **Stripe Payments and PaymentMethod API's** for the basic InMemory Data Flow.

You can also enable an **optional flow** using Products stored on your test Stripe account that makes use of the **Products, and Prices API's** to get the most up to date price for the product before the purchase is complete.

The Client Apps users are notified in realtime that a successful charge has been made by using a Webhook that listens to the "charge.succeeded" StripeEvent in conjunction with a SignalR Hub.

**Platforms Supported**

|Platform|Version|
| ------------------- | :------------------: |
|ASP .NET Core| 3.1 |
|Xamarin.Forms| 5.0+|
|Xamarin.iOS|iOS 11+|
|Xamarin.Android|API 21+|

#### Solution Structure
![StripeBookStore Project Structure](https://github.com/Pujolsluis/StripeBookStore/blob/main/Images/ProjectStructureStripeBookStore.png?raw=true)

### Challenges
In order to complete the assignment, I had to **learn all the fundamentals about Stripe** how their payment infraestructure works. The initial challenge I faced was not having worked previously with the product but I was really pleased to have found alot of great documentation, guides and a extensive API reference I could leverage to get an understanding of the product and the common payment use cases it supports.

I decided to use **Xamarin.Forms** as my client app solution and found that **Stripe does not have official up to date Xamarin Bindings** for their iOS and Android Native SDK's, I could not leverage their prebuilt UI's unless I wanted to invest time on creating the Xamarin Bindings and create the abstractions to use it seamlessly from the Shared Xamarin.Forms project, I felt this was out of scope for this project and decided to use the .NET Stripe Library, build custom screens and implement the payment flow accordingly.

### Extending the project into a more robust instance
The current state of the project is not production ready and is more of a POC than a MVP, I would recommend extending it with the following improvements:
- Server:
    - Payments:
        - **Add Endpoints** to manage products, customers and payment methods with Stripe API's instead of In-Memory Data.
    - Data:
        - **Design and Implement Data Layer** - define database model, entities, and persist and sync data with Stripe.
        - **Add Caching**
    - Authentication:
        - **Add Security to REST API**
        - **Create User Authentication Endpoints**
        
- Client Apps: 
    - UI/UX Improvements:
        - **Add Search & Filters for Books Catalog Page** - Allow user to search and filter the list of books.
        - **Add Product Details Page** - Allow the user to get an extended description, specifications, view multiple images and reviews about the product before commiting to purchase.
        - **Extend Payment Flow** in Client Apps from "Select a book > Checkout > Add Payment Method > Purchase Item" instead the app should also have:
            - **Cart Page** - Allows the users to CRUD their own collection of products to purchase.
            - **Add Shipping Method Page** - Allows user to set a shipping Address and get shipping options to complete order.
            - **Persist Payment Methods** - Allow users to save their payment methods and reuse when needed.
            - **Payment Method Validation with Stripe 3D Secure** - For banks that require validation, project should handle it gracefully.
        - **Issue Refunds** - Alows user to issue refunds for orders he has placed under a certain period of time.
        - **Send Email Receipts** - Users receive an email receipt of their purchase.
        - **Localization** - App should be localized for all the markets it wishes to support.
        - **Authentication Flow** - Users should be able to register and sign in to the app before being able to purchase (User must have a stripe customer associated to support features other features of stripe like saving payment methods, shipping method, and associating transactions to the account).


## Useful References for completing project
- [Stripe API Reference](https://stripe.com/docs/api?lang=dotnet)
- [Online Payments guides - Stripe](https://stripe.com/docs/payments/cards/overview)
- [Payment Method guides - Stripe](https://stripe.com/docs/payments/payment-methods/overview)
- [.NET Core Starter - Stripe Office Hours](https://www.youtube.com/watch?v=2-mMOB8MhmE)

## Dev Tools Used
- Stripe CLI
- VS Code Stripe Extension
- VS For Mac 2019