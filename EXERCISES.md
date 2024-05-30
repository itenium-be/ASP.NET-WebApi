WebApi
======

Add `[AllowAnonymous]` for easier manual testing.


## ModelBinding

1. BrandsController :: DeleteMany

From our frontend new functionality has been added to delete many Brands in one call.
Right now, an array of Guid ids is submitted in the body.  

This is unfortunately a problem for our frontend team, as they are using a package
that automatically calls the backend like this:

```
DELETE /api/v1/brands?ids=1,2,3
```

See `QueryStringIdsModelBinder` for implementing this.

Our junior developer tried the following:

```c#
public Task<Guid[]> DeleteMany([FromQuery] Guid[] ids)
```

But this expects a call like: `/api/v1/brands?ids=1&ids=2&ids=3`
which is not compatible with the frontend package.


2. All Guids are passed like that

Imagine that ALL `Guid[]` are always passed like `?ids=1,2,3`
How to add this to the IOC container?

See `QueryStringIdsBinderProvider`.



3. BrandsController :: Create

There are a few special cases for which some additional logic
needs to be performed when creating a new brand.

We've added a `Type` to the `CreateBrandRequest`

```c#
public enum BrandType
{
  OurBrand,
  ThirdPartyBrand,
}
```

The request body looks like this:

```json
{
  "name": "Socks-n-Smells",
  "description": "This famous socks brand has, due its enormous success, been split off itenium",
  "type": 0
}
```

But who can ever remember what `0` or `1` means?  
We want to pass `OurBrand` or `ThirdPartyBrand` as a string instead.



4. DateTime vs DatePicker

Our frontend has a `DatePicker` control which sends dates in a very specific format:

```
ddMMyyyy
```

For eaxmple our BrandsController::Create has an `ActiveFrom` field that needs to accept standard
DateTimes and also this specific format.


5. Damn Americans!

Our American users submitted a ticket that this format isn't working for them.
After some analysis it turns out that their format is `MMddyyyy`.

We need to check the `Accept-Language` header and parse as `MMddyyyy` when the
value is `en-US`.

Because this is getting rather complicated, we need 100% test coverage for this!

6. Our junior developer added the BrandsController::Update action. However pen-tests have revealed a security vulnerability

We need to validate that the Update request body contains the same Brand Id in the query as in the request body.

We can use FluentValidations to add a Request Validator. To check with the querypath the http context needs to be injected by the DI container. Make sure the validator is registered.

Tip: use FluentValidationAutoValidation from the `SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions` package.

7. Too slow...

Recently a lot of new brands were added and our frontend devs have set the default pagesize to high so now the product list is loading to slow. Maybe some compression can help?

7.1. Measure

We've received complaints that some of our endpoints are (still!?) too slow.
Because the user(s) didn't specify exactly what actions are lagging,
we want to add middleware to log the timing of all requests.

Log the URL, headers, body & querystring parameters.

Bonus:  
We want to avoid logging sensitive information:
- Do not log the Authorization header
- Do not log body properties that are decorated with a newly created [SensitiveAttribute]
