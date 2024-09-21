---
theme: gaia
paginate: true
backgroundColor: #fff

  <!---
  See https://marp.app/
  -->
---

# Car Rental Library

- What you asked for
- What we have created
- Areas for further discussion

---

# What you asked for 

- A prototype system for handling car rentals
- Features: start rental, end rental, and calculate rental price
- Adaptable to the needs of different clients
- Extensible with new types of cars

---

# What we have created for you

- A platform-agnostic class library
- Use it with a REST API, a desktop GUI, a console application...
- Persist data using any database technology 
- Find it here: [GitHub repository](https://github.com/htoomik/car-rental)
- Includes integration test demonstrating usage

---

# Extensibility and configurability

Easy to extend with:
- New categories
  - Requires code changes
- Client-specific pricing parameters
  - Configurable without code changes

---

# Business rules

- Assumptions made regarding pricing
  - Rental fee is paid per each started day
  - There is no rounding of mileage
  - Price calculation rules do not vary by client
- To be discussed
  - More detailed data validation rules

---

# Maintainability

- CQRS pattern, for growth, flexibility and performance
- A minimum of libraries
  - FluentValidation
  - Microsoft.Extensions.Logging
  - Libraries for unit tests
- To be discussed
  - Data structure for errors: error codes? translations?

---

# Questions?