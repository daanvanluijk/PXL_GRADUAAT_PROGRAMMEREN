using CMProjectDataBase;
using Newtonsoft.Json;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

PagesSQLiteRepository pagesSQLiteRepository = new PagesSQLiteRepository();

var app = builder.Build();

app.MapGet("/Retrieve", delegate (string userName, string password)
{
    if (userName is null || password is null)
        return null;
    User user = new User(userName, password);
    return pagesSQLiteRepository.GetPages(user);
});
app.MapPost("/Store", async delegate (HttpContext context)
{
    using (StreamReader reader = new StreamReader(context.Request.Body, Encoding.UTF8))
    {
        string jsonstring = await reader.ReadToEndAsync();
        User user = JsonConvert.DeserializeObject<User>(jsonstring);
        if (user is null)
        {
            return;
        }
        pagesSQLiteRepository.SavePages(user);
    }
});
app.Run();