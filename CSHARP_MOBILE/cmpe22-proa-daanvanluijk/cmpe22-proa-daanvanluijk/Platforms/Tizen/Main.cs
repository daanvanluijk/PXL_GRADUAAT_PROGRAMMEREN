using System;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace cmpe22_proa_daanvanluijk;

class Program : MauiApplication
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

	static void Main(string[] args)
	{
		var app = new Program();
		app.Run(args);
	}
}
