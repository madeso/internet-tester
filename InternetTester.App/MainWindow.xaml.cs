﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using InternetTester.Lib;

namespace InternetTester.App
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			var app = new InternetTester.Lib.App(this.UpdateStatus);
			this.DataContext = app.Data;

			this.Language = XmlLanguage.GetLanguage(Thread.CurrentThread.CurrentCulture.Name);
		}

		private void UpdateStatus(AppStatus appStatus)
		{
			switch (appStatus)
			{
				case AppStatus.Up:
					SetIcon(Properties.Resources.StatusOk, "StatusOk.ico");
					break;
				case AppStatus.Down:
					SetIcon(Properties.Resources.StatusError, "StatusError.ico");
					break;
				case AppStatus.Unsure:
					SetIcon(Properties.Resources.StatusUnsure, "StatusUnsure.ico");
					break;
				case AppStatus.Shutdown:
					// todo(Gustav): update this icon?
					SetIcon(Properties.Resources.StatusUnsure, "StatusUnsure.ico");
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(appStatus), appStatus, null);
			}
		}

		private void SetIcon(Icon icon, string path)
		{
			// todo(Gustav): update the icon

			Uri iconUri = new Uri("pack://application:,,,/Res/" + path, UriKind.RelativeOrAbsolute);
			this.Icon = BitmapFrame.Create(iconUri);

			// var ibd = new IconBitmapDecoder(
			// 	new Uri(@"pack://application:,,,/Resources/" + path, UriKind.RelativeOrAbsolute),
			// 	BitmapCreateOptions.None,
			// 	BitmapCacheOption.Default);
			// Icon = ibd.Frames[0];

			// var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Resources/" + path);
			// this.Icon = BitmapFrame.Create(stream);
		}
	}
}
