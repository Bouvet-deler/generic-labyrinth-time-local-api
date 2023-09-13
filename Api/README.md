# How to run
- Either run the solution by clicking on the green arrow in visual studio or use the command **"dotnet run"** in the terminal
- Run the frontend as well, and click the **Start** button. This will make the webpage wait for a start signal from the server, which is triggered by pressing the yellow button in the room.
- The stopwatch will automatically reset itself after a finished run(the sensor registers two entities passing it), and begin waiting for the next start signal\
***Note the stopwatch time on the webpageis purely visual, it is the time taken on the backend that is the actual time.***
  
# How to test without the Arduino
## Setting up
- Comment out this line in the file **Program.cs**
```
  builder.Services.AddHostedService<HardwereBackgroundService>();
```

- Uncomment these methods in **Controller.cs** if they are commented out
```
  	app.MapGet("/simulateStartTime", void (Application application) =>
	{
		application.simulateStartTime();
	}).Produces<string>(StatusCodes.Status200OK)
	  .Produces<string>(StatusCodes.Status400BadRequest);

	app.MapGet("/simulateEndTime", void (Application application) =>
	{
	    application.simulateEndTime();
	}).Produces<string>(StatusCodes.Status200OK)
	  .Produces<string>(StatusCodes.Status400BadRequest);

	app.MapGet("/simulateEndTime2", void (Application application) =>
	{
	    application.simulateEndTime2();
	}).Produces<string>(StatusCodes.Status200OK)
	  .Produces<string>(StatusCodes.Status400BadRequest);
```
- Uncomment these methods in **Application.cs** if they are commented out
```
    public void simulateEndTime()
    {
        time_span1 = "01:12:000";
        runStop = true;
    }



    public void simulateEndTime2()
    {
        time_span2 = "00:09:110";
        runStop2 = true;
    }
  ```

## Testing
- Click the **Start** button on the localhost:3000 to start the frontend application
```
https://localhost:3000
```
- Use the SwaggerUI on localhost:5050 to send http requests to frontend
```
https://localhost:5050/index.html
```
- The method **simulateStartSignal** will send a start signal which will cause the fronted application to start the stopwatch.
- The method **simulateEndTime** and **simulateEndTime2** will send the end signal, which will cause the run to end. Currently, the end times are hard coded which means that the finished run times of both players will be the the same between the runs, regardless of time elapsed.
- Both simulateEndTime methods needs to be called to stop the frontend application from counting time.

# How it works
- When receiving the start signal, the stopwatch will begin running.
- The stopwatch will run until the sensor registers two entities passing it.
- When two entities has passed the sensor, the frontend application will receive the signal that the run is over, and then it will request time taken by both participants.\
***Note the stopwatch time on the webpage is purely visual, it is the time taken on the backend that is the actual time.***
- The backend will return the time that it has registerd for each participant, where the fastest one is considered *Player1* and the other is *Player2*.
- Frontend will automatically fill the "time" boxes in the submit form with the times received.
- After the form with name, time, email and phone number is filled out and submitted it is sent to the backend where a new **User** object is created if the user wasn't registered before. **To determine if a user has played before, there is a check if the email is unique**.
