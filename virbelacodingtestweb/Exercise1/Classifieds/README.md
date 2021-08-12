# Exercise 1 #

For this exercise you will create a REST API that provides data to support a classifieds application.

As you progress through the steps, feel free to add comments to the code about *why* you choose to do things a certain way. Add comments if you felt like there's a better, but more time intensive way to implement specific functionality. It's OK to be more verbose in your comments than typical, to give us a better idea of your thoughts when writing the code.

### What you need ###

* IDE of your choice
* Git
* Some chosen backend language / framework
* Some chosen local data store

## Instructions ##

### Phase 1 - Setup ###

 1. Clone this repository to your local machine
 1. Create the basic structure needed for your API with your chosen framework
 1. Add a README.md in this exercise folder with the basic requirements and steps to run the project locally

### Phase 2 - Main Implementation ###

Implement a RESTful API to support a classifieds application that satisfies the following requirements:

 * Ability to create (essentially 'register') a User object using POST call. User must have email/password.
 * Ability to 'login' using email/password combo in POST call. Should return some kind of authorization token to be re-used on subsequent calls.
 * Ability to perform all CRUD operations for a Listing object. The Listing object represents a 'for sale' classified ad. Include minimum of Title, Description, Price fields.
 	* A valid authorization token must be provided for all Listing operations
 	* A User can create many Listings
 	* Only the User who created a Listing can update or delete a Listing
 	* An authenticated User can retrieve all Listings

### Phase 3 - Stretch Goals ###

Please implement any of the following stretch goals. They are in no particular order.

 * Allow paging and/or filtering of Listings
 * Add some type of self-documenting UI such as Swagger
 * Create Unit Tests (note and include in the commit with your tests any bugs/improvements you make due to Unit Test development)

### Phase 4 - SUPER STRETCH GOAL - Add Region based listings ###

We want to alter our very general classifieds API to limit Listings to Users based on an associated Region. Please make changes to satisfy the following requirements:

 * Each User is associated with a single Region. A Region has many Users.
 * When a User requests all Listings, they only receive Listings created by Users in the same Region as themselves.

## Questions ##

 1. How can your implementation be optimized?
    * Goodness, so many ways. This was designed as a demo, for very low volume and small datasets. Use an ORM like EF or even Dapper and a real DB like MySQL, Oracle, SQL Server...
    * Use async calls to support greater scalability, but at the cost of clarity and much more difficult to debug. I went for quick to code and good enough. Perfect being the emeny of good.
    * First build out a true PDD and understand not just the "What" but the "Why" for the project, so that the architecture an implementation best align to the goals.
    * Follow RESTful design for most things, but be willing to deviate where it supports effective UI/UX.
    * Write a true UI Client to demonstrate the use.
    * Write a full suite of test cases at the Service layer to assure that all edge cases are covered and to support running tests at build time as part of the CI/CD pipeline.
    * Implement some form of logging to track usage.  
    * It really does go on and on.. this is a demo, a proof, not a piece of production software. 

 2. How much time did you spend on your implementation?
    * Difficult to ascertain as this was not done uninterrupted, but with a great deal small segments of time spent where time allowed. I do have a very consuming full-time job and I have a family.
    * I started Monday after work, and spent the first evening just refreshing my memory. It has been about 2.5 years from the last API I wrote. I've written a lot of them.
    * I have been serving as am Architect and team lead over the RPA CoE, with no real hands on coding. I don't even fancy myself an RPA developer, but I know .Net well enough to guide the teams well.
    * I spent three evenings on this. I'd estimate about 15 hours overall, but if I was honestly charging, I'd bill for 8 hours.
    * I enjoyed the return to C#, Swagger and API creation. Truth is, I needed a good refresher and this was fun. I only wish I had been able to do it with greater focus and uninterrupted.
 
 3. What was most challenging for you?
    * Cutting corners knowing it was utility level code as a demo, and selecting what to show and what to skip.
    * Not knowing the true goal or the grading standard, I lacked clear vision of the goal, so a lot of assumptions were made. I only hope I choose wisely. 
    * I'm looking at a lot of different possible next steps in my career, but I want this job. I want to join the Virbela mission. I can feel my passions stir and am confident that my skills will be tested and my capabilities will grow.
    * I maybe wanted the job too much and that influenced my choices, so I cut only enough to deliver an MVP based on the available time.

## Next Steps ##

* Confirm you've addressed the functional goals - I believe I have
* Answer the questions above by adding them to this file - Done
* Make sure your README.md is up to date with setup and run instructions - See Below
* Ensure you've followed the sharing instructions in the main [README](../README.md)

### How to use this solution ###

## Load the Swagger UI ##
1.  Load the solution file "Classifieds.sln" file into Visual Studio. I used VS 2019 Enterprise.
2.  Run the solution by pressing F5. A browser will open and the default Web API home page will load.
3.  The menu near the top has three menu items. "Home", "Default API UI" and "Swagger UI". 
        * You can use either API UI, but for this example, we will use Swagger.
4.  Click "Swagger UI" in the menu near the top. The Swagger UI will load. 

## Crate a new user ##
1.  Locate the section titled "User" and within that, find the action for "POST /api/User" and click it to open the details.
2.  Enter a "userName", an "emailAddress" and a "userPassword". 
        * Note or remember these values as you will need them later.
3.  Click "Try it out!" and you will see that you get a 401 (Forbidden) error. 
        * This set of APIs are protected by an API Key. We will need to set that now.
4.  At the top right of the page, there is a text box that says "api_key". type "1234567" without the quotes. 
        * "1234567" is the hard coded API key. 
        * Once this is set, we can leave it there for all the subsequent API calls we will be making. 
5.  Now that we have the API Key in place, return to the "Try it out!" button that we used before and click it again. 
        * This time we will get a 200 response and a user id will be returned. Please note the id.

## Create a session ##
1.  Locate the section titled "Session" and within that, find the action "POST /api/Session" and click it to open the details.
        * Every API with the exception of creating a new user requires the use of a session.
2.  Enter the "userName" and "Password" for the use we created above.
3.  Click "Try it out!" and if you got the userName and userPassword right, you will get back a session Id.
        * Note this Id, you will need it for every API we call. It is valid for an hour. 
        * If it expires, go back to step 1 in this section and generate a new one. 
        * Each user can only have one valid session Id at a time. 
        * The session Id is between the quotes. Do not use the quotes when entering it in subsequent API calls.

## Create a listing ##
1.  Locate the section titled "Listing" and within that, find the action "POST /api/Listing" and click it to open the details.
2.  Enter the sessionId we generated in the last step
3.  The listing data uses a Json format, and the easiest way to assure you use the correct format is to click the "Model Example Value" on the right.
        * This copies the shape of the entry into the text field.
4.  Edit the Json and fill in each of the values.
        * You will replace the word "string" with the actual value you wish to use. Keep the quotes, and only replace the word string.
5.  Click "Try it out!" and if you got the sessionId and shape of the Json right, you will get back a Listing Id.

## Reading all listings ##
1.  Still in the section titled "Listing", find the action "GET /api/Listing" and click it to open the details.
        * Make sure you use the correct action. Do not use the "GET /api/Listing/{id}" action.
2.  Enter the sessionId and click "Try it out!". If you got the session Id right, you will get back a Json blob containing every listing that exists... which at this point is just one.

## Expanding the number of listings ##
1.  Use the two sections above to create any number of listings and then look at them.
        * As you add more listings, the Json gets harder and harder to read by eye.
2.  Copy the Json blob, but don't include the quotes around it. 
3.  In a browser, navigate to "https://codebeautify.org/json-decode-online" and paste the Json blob on the left and you will see it represented on the right.
        * Much easier to read this way.
        * Now you can see the text along with the Listing Id, the created and updated timestamps and the Id of the user that created the listing... that's you!
4.  Do this until you have three or more listings.

## Updating a listing ##
1.  While still in the "Listing" section, find the "PUT /api/Listing/{id}" action and click it to open it.
2.  Enter the Listing Id for the listing to change in the id field.
        * You are going to need a Listing Id, which you were given when you created the listing, or you can get it from the Json blob returned when you read all the listings.
3.  Enter the session Id and the click the Model Example Value like we did above, and fill in the new listing data.
        * Make sure you fill in all of the fields. Whatever you enter here will replace what is currently in the listing referenced by the id.
4.  Click "Try it out!" and if you got the info right, you will get back a 204 (No Content) result code, meaning the original no longer exists, it has been replaced.
5.  Go back and read all listings again, and use the site to read the Json and notice the change you just made.
        * Notice the ListingUpdated value changed to when you made the update (Universal Time) and if different from the CreatedTime when the listing was first created.

## Delete a listing ##
1.  While still in the "Listing" section, find the "DELETE /api/Listing/{id}" action and click it to open it.
2.  Enter the Listing Id for the listing to delete in the id field and enter the session Id as well
        * Delete is permanent, there is no way to retrieve a deleted listing.
3. Click "Try it out!" and if you got the info right, you will get back a 204 (No Content) result code, meaning the listing no longer exists.

## Playing around ## 
Now that you have the basics down, you can create more users, and for each user create a session and create listings for different users. There are also additional API actions that we didn't cover. Check them out!

## The most important step ##
1.  Hire Mike Reed! :-) WOOT!
