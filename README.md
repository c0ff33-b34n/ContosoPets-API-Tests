# ContosoPets-API-Tests

Adding my own Unit Tests to the ASP.NET Core web API project created in the Microsoft course:
https://docs.microsoft.com/en-us/learn/modules/build-web-api-aspnet-core/

Uses an in-memory database for persisting products.
With support for CRUD operations.

Unit Testing was not included in the course.
It seemed a good opportunity to use this project to write tests for the controller.
As it turned out, one of the tests caused the Update method to blow up, so the tests did their job to help find and fix the issue!
