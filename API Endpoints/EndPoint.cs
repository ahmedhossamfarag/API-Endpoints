using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API_Endpoints
{
	public class EndPoint
	{
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public RequestType Type {  get; set; }

		public string Url { get; set; }

		public string Params { get; set; }

		public string Body { get; set; }

		public string Succes { get; set; }

		public string Error { get; set; }
	}
}
