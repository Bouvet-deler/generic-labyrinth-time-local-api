public static class Controllers
{
	public static WebApplication MapRoutes(this WebApplication app)
	{
		app.MapGet("/GetCurrentTopListName", string (Application application) =>
		{
			return application.CurrentTopListFileName;
		}).Produces<string>(StatusCodes.Status200OK)
		  .Produces<string>(StatusCodes.Status400BadRequest);

		app.MapGet("/GetCurrentTopList", IResult (Application application) =>
		{
			return Results.Ok(application.CurrentToplist.OrderBy(x => x.Time).ToList());
		}).Produces<List<User>>(StatusCodes.Status200OK)
		  .Produces<string>(StatusCodes.Status400BadRequest);

		app.MapGet("/GetAllUsersList", IResult (Application application) =>
		{
			return Results.Ok(application.CurrentToplist.ToList());
		}).Produces<List<User>>(StatusCodes.Status200OK)
		  .Produces<string>(StatusCodes.Status400BadRequest);

		//app.MapGet("/GetNewUsers", IResult (Application application) =>
		//{
		//    return Results.Ok(application.CurrentToplistFileName.ToList());
		//}).Produces<List<User>>(StatusCodes.Status200OK)
		//  .Produces<string>(StatusCodes.Status400BadRequest);

		app.MapGet("/ListAllTopLists", IResult (Application application) =>
		{
			return application.ListAllTopLists();
		}).Produces<List<string>>(StatusCodes.Status200OK)
		  .Produces<string>(StatusCodes.Status400BadRequest);

		app.MapPost("/CreateNewTopList", (string topListName, Application application) =>
		{
			return application.CreateNewTopList(topListName);
		}).Produces<string>(StatusCodes.Status200OK)
		  .Produces<string>(StatusCodes.Status400BadRequest);

		app.MapPost("/LoadTopList", async (string topListName, Application application) =>
		{
			return await application.LoadTopListAsync(topListName);
		}).Produces<string>(StatusCodes.Status200OK)
		  .Produces<string>(StatusCodes.Status400BadRequest);

		app.MapPost("/NewTimeEntry", async (string username, string email, int phoneNumber, string username2, string email2, int phoneNumber2, Application application) =>
		{
			return await application.NewTimeEntry(username, email, phoneNumber, username2, email2, phoneNumber2);
		}).Produces<string>(StatusCodes.Status200OK)
		  .Produces<string>(StatusCodes.Status400BadRequest);

		app.MapGet("/StartTime", bool (Application application) =>
		{
			return application.StartTime();
		}).Produces<string>(StatusCodes.Status200OK)
		  .Produces<string>(StatusCodes.Status400BadRequest);

		app.MapGet("/EndTime", bool (Application application) =>
		{
			return application.EndTime();
		}).Produces<string>(StatusCodes.Status200OK)
		  .Produces<string>(StatusCodes.Status400BadRequest);

		app.MapGet("/EndTime2", bool (Application application) =>
		{
			return application.EndTime2();
		}).Produces<string>(StatusCodes.Status200OK)
		  .Produces<string>(StatusCodes.Status400BadRequest);

		app.MapGet("/simulateStartTime", void (Application application) =>
		{
			application.simulateStartTime();
		}).Produces<string>(StatusCodes.Status200OK)
		  .Produces<string>(StatusCodes.Status400BadRequest);

		//app.MapGet("/simulateEndTime", void (Application application) =>
		//{
		//    application.simulateEndTime();
		//}).Produces<string>(StatusCodes.Status200OK)
		//  .Produces<string>(StatusCodes.Status400BadRequest);

		//app.MapGet("/simulateEndTime2", void (Application application) =>
		//{
		//    application.simulateEndTime2();
		//}).Produces<string>(StatusCodes.Status200OK)
		//  .Produces<string>(StatusCodes.Status400BadRequest);

		app.MapGet("/sendTimePlayer1", string (Application application) =>
		{
			return application.sendTimePlayer1();
		}).Produces<string>(StatusCodes.Status200OK)
		  .Produces<string>(StatusCodes.Status400BadRequest);

		app.MapGet("/sendTimePlayer2", string (Application application) =>
		{
			return application.sendTimePlayer2();
		}).Produces<string>(StatusCodes.Status200OK)
		  .Produces<string>(StatusCodes.Status400BadRequest);

		app.MapGet("/ResetTime", void (Application application) =>
		{
			application.resetTime();
		}).Produces<string>(StatusCodes.Status200OK)
		  .Produces<string>(StatusCodes.Status400BadRequest);

		app.MapGet("/PickRandomWinnersFromParticipants", IResult (int numberOfWinners, Application application) =>
		{
			return application.pickRandomWinnersFromParticipants(numberOfWinners);
		}).Produces<string>(StatusCodes.Status200OK)
		  .Produces<string>(StatusCodes.Status400BadRequest);

		return app;
	}
}
