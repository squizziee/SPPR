namespace WEB_253504_LIANHA.Extensions
{
	public static class HttpRequestExtensions
	{
		private const string XmlHttpRequest = "XMLHttpRequest";

		public static bool IsAjaxRequest(this HttpRequest request)
		{
			if (request == null)
				return false;

			return string.Equals(request.Headers["X-Requested-With"], XmlHttpRequest, StringComparison.OrdinalIgnoreCase);
		}
	}
}
