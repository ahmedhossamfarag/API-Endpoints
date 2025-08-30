using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.Json;

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
			dialog.Filter = "Json Files(*.json)|*.json|Markdown Files(*.md)|*.md";
			dialog.DefaultExt = ".json";
			dialog.AddExtension = true;
			dialog.Title = "Choose destination file";
			if (dialog.ShowDialog() == true)
			{
				try
				{
					string text;
					if (dialog.FileName.EndsWith(".md"))
					{
						text = string.Join("\n", Array.ConvertAll(Model.EndPoints.ToArray(), ep => ep.ToMarkdown()));
					}
					else
					{
						text = JsonSerializer.Serialize(this.Model.EndPoints.ToArray(), Model.SerializerOptions);
					}
					using (var file = System.IO.File.CreateText(dialog.FileName))
					{
						file.Write(text);
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
			const string indentPattern = "    ";
			if(sender is TextBox tb)
			{
				if(e.Key == Key.OemOpenBrackets && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
				{
					var caretIndx = tb.CaretIndex;
					tb.Text = tb.Text.Insert(caretIndx, "{}");
					tb.CaretIndex = caretIndx + 1;
					e.Handled = true;
				}
				else if(e.Key == Key.Tab)
				{
					var caretIndx = tb.CaretIndex;
					tb.Text = tb.Text.Insert(caretIndx, indentPattern);
					tb.CaretIndex = caretIndx + indentPattern.Length;
					e.Handled = true;
				}
				else if(e.Key == Key.Enter)
				{
					var caretIndx = tb.CaretIndex;
					var textBefore = tb.Text.Substring(0, caretIndx);
					var textAfter = tb.Text.Substring(caretIndx, tb.Text.Length - caretIndx);
					var lastLine = textBefore.Split('\n').Last();
					var indent = "";
					while (lastLine.StartsWith(indentPattern))
					{
						indent += indentPattern;
						lastLine = lastLine.Substring(indentPattern.Length);
					}
					if (textBefore.TrimEnd().EndsWith("{") && textAfter.TrimStart().StartsWith("}"))
					{
						tb.Text = tb.Text.Insert(caretIndx, Environment.NewLine + indent + indentPattern + Environment.NewLine + indent);
						tb.CaretIndex = caretIndx + Environment.NewLine.Length + indent.Length + indentPattern.Length;
					}
					else
					{
						tb.Text = tb.Text.Insert(caretIndx, Environment.NewLine + indent);
						tb.CaretIndex = caretIndx + Environment.NewLine.Length + indent.Length;
					}
					e.Handled = true;
				}
			}
        }
    }
}
