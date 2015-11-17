using System;
using System.IO;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Automate.UITests
{
	public class AppInitializer
	{
		public static IApp StartApp(Platform platform)
		{
			if(platform == Platform.Android)
			{
				var apkPath = Directory.EnumerateFiles(Path.Combine("..", "..", "testapps"), "*.apk").First();
				return ConfigureApp.Android.ApkFile(apkPath).StartApp();
			}
				
			var appPath = Directory.EnumerateDirectories(Path.Combine("..", "..", "testapps"), "*.app").First();
			return ConfigureApp.iOS.AppBundle(appPath).StartApp();
		}
	}
}

