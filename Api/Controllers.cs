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

        app.MapPost("/NewTimeEntry", async (string username, double time, Application application) =>
        {
            return await application.NewTimeEntry(username, time);
        }).Produces<string>(StatusCodes.Status200OK)
          .Produces<string>(StatusCodes.Status400BadRequest);

        return app;
    }
}
