# How to run
### Visual studio
- Either run the solution by clicking on the green arrow or use the command "dotnet run" in the terminal
- Run the frontend as well, and click the **Start** button. This will make the webpage wait for a start signal from the server, which is triggered by pressing the yellow button in the room.

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
- The method **simulateStartSignal** will send a start signal which will cause the fronted application to start taking the time.
- The method **simulateEndTime** and **simulateEndTime2** will send the end signal, which will cause the run to end. Currently, the end times are hard coded which means that the finished run times of both players will be the the same between the runs, regardless of time elapsed.
- Both simulateEndTime methods needs to be called to stop the frontend application from counting time.
