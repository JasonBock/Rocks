namespace Rocks.Templates
{
	public static class EventTemplates
	{
		public static string GetNonPublicEvent(string visibility, string eventType, string eventName) => $"{visibility} override event {eventType} {eventName};";

		public static string GetEvent(string @override, string eventType, string eventName) => $"public {@override} event {eventType} {eventName};";
	}
}
