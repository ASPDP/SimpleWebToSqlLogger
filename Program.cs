
using Microsoft.EntityFrameworkCore;

namespace WebApplication2;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);
		
		builder.Services.AddDbContextFactory<SovaLoggerContext>(opt =>
			opt.UseSqlServer(builder.Configuration.GetConnectionString("mssqlserver"))	
		);
		// Add services to the container.
		//builder.Services.AddAuthorization();

		var app = builder.Build();

		// Configure the HTTP request pipeline.

		app.MapPost("/log", async (SovaLoggerContext db, HttpRequest request) => { DoSqlMagic(db, request); });

		app.Run();
	}

	static async Task DoSqlMagic(SovaLoggerContext db, HttpRequest request)
	{
		using var reader = new StreamReader(request.Body);
		var text = await reader.ReadToEndAsync();
		var txtArray = text.Split("𩸽ढ");
		//db.Logs.Add(new Log() { Data = text });
		//db.SaveChanges();
		if (txtArray.Length == 3)
			db.Database.ExecuteSql(
				$"INSERT INTO [Logs] (Data, Action, GmailUser) VALUES ({txtArray[0]}, {txtArray[1]}, {txtArray[2]})"); // Exe Update(e=>e.SetProperty(y=>y.Data,text));;
		else if (txtArray.Length == 2)
			db.Database.ExecuteSql(
				$"INSERT INTO [Logs] (Data) VALUES ({text})"); // Exe Update(e=>e.SetProperty(y=>y.Data,text));;
		else if (txtArray.Length == 1)
			db.Database.ExecuteSql(
				$"INSERT INTO [Logs] (Data) VALUES ({text})"); // Exe Update(e=>e.SetProperty(y=>y.Data,text));;
		//db.Logs.ExecuteUpdate(e=>e.SetProperty(y=>y.Data,text));
		//Console.WriteLine(text);
	}
}