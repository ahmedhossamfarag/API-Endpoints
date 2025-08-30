using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace API_Endpoints
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainModel Model { get; set; } = new MainModel();

		public MainWindow()
		{
			InitializeComponent();
			DataContext = this;
		}

		private void Add_EndPoint(object sender, RoutedEventArgs e)
		{
			Model.EndPoints.Add(new EndPoint());
        }

		private void Remove_EndPoint(object sender, RoutedEventArgs e)
		{
			//System.Diagnostics.Debug.WriteLine(button.DataContext);

			if (sender is Button button && button.DataContext is EndPoint endPoint)
			{
				if (MessageBox.Show($" Delete {endPoint.Url}. \n Are You Sure ?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes){
					Model.EndPoints.Remove(endPoint);
				}
			}
		}

		// Replace the Export_Click method with a WPF-compatible file picker implementation.
		// The Windows.Storage.Pickers namespace is not available in standard WPF applications.
		// Use Microsoft.Win32.OpenFileDialog instead.

		private void Export_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new Microsoft.Win32.SaveFileDialog();
			dialog.Filter = "Json Files(*.json)|*.json";
			dialog.DefaultExt = ".json";
			dialog.Title = "Choose destination file";
			if (dialog.ShowDialog() == true)
			{
				try
				{
					var json = JsonSerializer.Serialize(this.Model.EndPoints.ToArray(), Model.SerializerOptions);
					using (var file = System.IO.File.CreateText(dialog.FileName))
					{
						file.Write(json);
					}
					MessageBox.Show("File Saved");
				}
				catch(Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}

		private void Import_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new Microsoft.Win32.OpenFileDialog();
			dialog.Filter = "Json Files(*.json)|*.json";
			dialog.Title = "Select a file to import";
			if (dialog.ShowDialog() == true)
			{
				try
				{
					using (var file = System.IO.File.OpenText(dialog.FileName))
					{
						var json = file.ReadToEnd();
						Model.EndPoints.AddRange(JsonSerializer.Deserialize<EndPoint[]>(json, Model.SerializerOptions));
						MessageBox.Show("File Read");
					}
				}
				catch(Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}

		private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if(sender is TextBox tb)
			{
				if(e.Key == Key.OemOpenBrackets && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
				{
					var caretIndx = tb.CaretIndex;
					tb.Text = tb.Text.Insert(caretIndx, "{}");
					tb.CaretIndex = caretIndx + 1;
					e.Handled = true;
				}
				if(e.Key == Key.Enter)
				{
					var caretIndx = tb.CaretIndex;
					var textBefore = tb.Text.Substring(0, caretIndx);
					var textAfter = tb.Text.Substring(caretIndx, tb.Text.Length - caretIndx);
					if (textBefore.TrimEnd().EndsWith("{") && textAfter.TrimStart().StartsWith("}"))
					{
						var lastLine = textBefore.Split('\n').Last();
						var indentPattern = "    ";
						var indent = "";
						while (lastLine.StartsWith(indentPattern))
						{
							indent += indentPattern;
							lastLine = lastLine.Substring(indentPattern.Length);
						}
						tb.Text = tb.Text.Insert(caretIndx, Environment.NewLine + indent + indentPattern + Environment.NewLine + indent);
						tb.CaretIndex = caretIndx + Environment.NewLine.Length + indent.Length + indentPattern.Length;
						e.Handled = true;
					}

				}
			}
        }
    }
}
