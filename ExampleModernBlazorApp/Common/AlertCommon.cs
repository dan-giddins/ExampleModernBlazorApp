using Microsoft.AspNetCore.Components;

namespace ExampleModernBlazorApp.Common
{
	public static class AlertCommon
	{
		private static RenderFragment SetAlert(string message, string alertType) =>
			builder =>
			{
				builder.OpenElement(1, "div");
				builder.AddAttribute(1, "class", $"alert {alertType}");
				builder.AddAttribute(1, "role", "alert");
				builder.AddContent(2, message);
				builder.CloseElement();
			};

		public static RenderFragment AlertDanger(string message) =>
			SetAlert(message, "alert-danger");

		public static RenderFragment AlertSuccess(string message) =>
			SetAlert(message, "alert-success");

		public static RenderFragment AlertInfo(string message) =>
			SetAlert(message, "alert-info");

		public static RenderFragment AlertWarning(string message) =>
			SetAlert(message, "alert-warning");
	}
}
