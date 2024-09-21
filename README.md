# Car Rental Library

This repository contains a partial implementation of a system for handling car rentals.

## Features

The library handles three steps of a car rental process.
1. Start rental - records initial details of the rental
2. End rental - records data about rental duration and mileage
3. Calculate rental price
 
Rental cars can belong to one of three categories. Each category has different pricing rules.

## Implementation

The system has been implemented as a class library, making it platform-agnostic.
It could be used with a REST API in front of it, or a desktop GUI, or a console application.

The system also makes no assumptions regarding persistence technology for storing the data about rentals. Client code 
needs to provide an implementation of IRentalRepository, which could be implemented using any suitable database provider.

An integration test using a simple in-memory implementation of the persistence layer demonstrates usage of the class
library in a sample scenario.

## Extensibility and configurability

**New categories**:
More categories can easily be added by adding new enum values and corresponding implementations of IPricingStrategy.

**Pricing parameters:**
Pricing parameters are configurable through the PricingConfiguration class, allowing each client to apply their own 
prices. Prices could be configured in a config file or a database.

## Assumptions regarding business rules

- Rental fee is paid per each started day
- There is no rounding of mileage
- Price calculation rules do not vary by client