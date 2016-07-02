namespace WebSiteForIntegration.Controllers
{
	using System;
	using System.IO;
	using System.Net;
	using System.Net.Http;
	using System.Threading.Tasks;
	using System.Web;
	using Castle.MonoRail;

	public partial class RootController
	{
		public ActionResult Index()
		{
			return new TextWriterResult(WriteMsg);
		}

		public Task<ActionResult> LongRunning2()
		{
			return Task.FromResult<ActionResult>(new TextWriterResult(WriteMsg));
		}
		public Task<ActionResult> LongRunning3()
		{
			var tcs = new TaskCompletionSource<ActionResult>();

			Task.Run(async () =>
			{
				await Task.Delay(1000);

				tcs.SetResult(new TextWriterResult(WriteMsg2));
			});

			return tcs.Task;
		}

		public async Task<ActionResult> LongRunning4()
		{
			var req = new System.Net.Http.HttpClient();
			var result = await req.GetStringAsync(new Uri("https://github.com/clearctvm/RabbitMqNext/blob/master/src/RabbitMqNext/MConsole/RestConsole.cs"));

			return new TextWriterResult(writer => { writer.Write(result); });
		}


		public ActionResult LongRunning()
		{
			return new TextWriterResult(WriteMsg);
		}

		public ActionResult ReplyWith304()
		{
			return new HttpResult(HttpStatusCode.NotModified);
		}

		public ActionResult ActionWithRedirect()
		{
			return new RedirectResult(Urls.Index.Get());
		}

		public ActionResult ActionWithRedirect2()
		{
			return new RedirectResult(Urls.ReplyWith304.Get());
		}

		public ActionResult ActionWithRedirectPerm()
		{
			return new PermRedirectResult(Urls.Index.Get());
		}

		public ActionResult ActionWithRedirectPerm2()
		{
			return new PermRedirectResult(Urls.ReplyWith304.Get());
		}


		private void WriteMsg(TextWriter writer)
		{
			writer.Write("Howdy");
			writer.Flush();
		}

		private void WriteMsg2(TextWriter writer)
		{
			writer.Write("Howdy " + HttpContext.Current);
			writer.Flush();
		}
	}
}