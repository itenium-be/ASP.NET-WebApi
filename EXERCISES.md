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
