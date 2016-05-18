## Some issues and comments on design

* The model is stored in-memory, in a Dictionary<label,InvItem>. This needs to be converted to database code.

* The key of the dictionary is just the label.

 * Should the type be part of the inventory identifier? (Should the key be label & type)

* Delete currently decrements the count, if greater than 1, or deletes the item if less than or equal to 1. This behavior is not specified.  I probably should have left the count out of the data-model, since it wasn't specified and it wasn't clear what to do with the count in all situations. (Follow KISS in the future, or if volunteering to add a feature, it needs to be properly fleshed-out.)

* Startup.inv is not right. The inventory object should be part of a model class file, not the Startup class. 

* Home page should be moved to a static page or template, not inline in the HomeModule.cs; it makes the code harder to read

* No attempt has been made to manage concurrency in this version

* Expired items are not yet logged properly; I need a notification log and notification subscribers to do this right...
