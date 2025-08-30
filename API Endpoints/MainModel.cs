using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API_Endpoints
{
	public class MainModel
	{
		public ObservableCollection<EndPoint> EndPoints { get; } = new ObservableCollection<EndPoint>();

		public JsonSerializerOptions SerializerOptions { get; } = new JsonSerializerOptions { WriteIndented = true, Converters = { new JsonStringEnumConverter() } };


		public MainModel() {}
	}
}
