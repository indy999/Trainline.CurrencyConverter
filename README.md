# Trainline.CurrencyConverter

This is the repository for building a currency exchange converter API for the engineering test for Trainline

# Project information

This is an .NET Core 5 API using xUnit and the in built Visual Studio Test runner.

There is no self signed certificate with the API

## Delivered Artifacts

- The Currency Converter API has one endpoints that can be reached at `https://localhost:44337/currency/`
- A Http GET request will make a call to the currency converter endpoint with more details at 'https://localhost:44337/swagger/index.html'
- I would also add more detailed API documentation if I had more time.

## Considerations/Assumptions

I'm assuming there were only 2 supported currencies as stated here: https://trainlinerecruitment.github.io/exchangerates/

The list of supported currencies should be in a data store/repository rather than as a simple hashset as here if I had more time.  This would allow us to grow and manage the list of locally supported currencies independent of the exchange rates api.

The regex for decimal amount validation does let null values through but they turn up as 0 so is type safe, I would have implemented a custom validation attribute to validate that if 0 isn't allowed but I'm making assumption it is.

For unsupported currencies if I had more time I would return a more suitable status code than bad gateway(possibly 422 or 400).  For time purposes I'm assuming the service will return null for this case and any gateway issues and currency request null validation issues.

When retrieving the exchange rates I'm assuming the Rates are always populated and with correct data, otherwise I would add more null checks if I had more time.

When calculating the conversion I'm round the result to 2 decimal places for readability.

I'm assuming that there will not be a long running request when calling the exchange rates api and assuming the exchange rates API is reliable and stable.  Hence I'm not returning a 206 to indicate this is a long running request and a partial response, instead a simple 200.  If I had more time and for production I would also consider implementing a progressive API Retry policy or circuit breaker pattern to handle the potential unreliability of calls to the exchange rates api as it is a key dependency and varying request load to our API.


I'm also assuming there is no caching of the exchange rates at this moment as we are getting the latest rates each time.  So no checking stale data/ due to a datetime check from the response. This could involve determining any caching policies and regards to cache expiry, TTL etc.

I'm assuming there is no authentication or rate limiting applied for this API as an MVP or security considerations. I would also add logging and application metrics capture.  There has been no mention of the potential hosting infrastructure in the exercise.  This would have to be taken into consideration for production use to handle anticipated load and scale reliably for unanticipated high traffic.

I would also automate the build and deployment of this API with containerization using docker as an option in a CI and deployment pipeline.  Running all tests as part of this would be an option.

For time purposes I have done relatively simple integration tests.  
