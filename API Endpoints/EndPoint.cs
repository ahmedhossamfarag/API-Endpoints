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
		public RequestType Type {  get; set; } = RequestType.GET;

		public string Url { get; set; } = string.Empty;

		public string Params { get; set; } = string.Empty;

		public string Body { get; set; } = string.Empty;

		public string Succes { get; set; } = string.Empty;

		public string Error { get; set; } = string.Empty;

		internal object ToMarkdown()
		{
			var b = new StringBuilder();
			b.AppendLine($"## {Type} [  {Url}  ]\n");
			if (!string.IsNullOrWhiteSpace(Params) || !string.IsNullOrWhiteSpace(Body))
			{
				b.AppendLine("### Request\n");
				if(!string.IsNullOrWhiteSpace(Params))
				{
					b.AppendLine("#### Params\n");
					b.AppendLine($"```\n{Params}\n```\n");
				}
				if(!string.IsNullOrWhiteSpace(Body))
				{
					b.AppendLine("#### Body\n");
					b.AppendLine($"```\n{Body}\n```\n");
				}
			}
			if(!string.IsNullOrWhiteSpace(Succes) || !string.IsNullOrWhiteSpace(Error))
			{
				b.AppendLine("### Response\n");
				if(!string.IsNullOrWhiteSpace(Succes))
				{
					b.AppendLine("#### Success\n");
					b.AppendLine($"```\n{Succes}\n```\n");
				}
				if(!string.IsNullOrWhiteSpace(Error))
				{
					b.AppendLine("#### Error\n");
					b.AppendLine($"```\n{Error}```\n");
				}
			}
			b.AppendLine();
			return b.ToString();
		}
	}
}
