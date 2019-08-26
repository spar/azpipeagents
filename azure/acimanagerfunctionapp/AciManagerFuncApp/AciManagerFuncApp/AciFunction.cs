using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Management.Fluent;

namespace AciManagerFuncApp
{
	public static class AciFunction
	{
		private static readonly string AzureAuthFile = Environment.GetEnvironmentVariable("AzureAuthFileName");
		private static readonly string SubscriptionName = Environment.GetEnvironmentVariable("SubscriptionName");
		private static readonly string ResourceGroupName = Environment.GetEnvironmentVariable("ResourceGroupName");
		private static readonly string ContainerGroupNames = Environment.GetEnvironmentVariable("ContainerGroupNames");

		[FunctionName("AciStartTimer")]
		public static async Task AciStartTimer([TimerTrigger("0 0 8 * * *")]TimerInfo myTimer, ILogger log, ExecutionContext context)
		{
			log.LogInformation($"C# Timer trigger function AciStartTimer executed at: {DateTime.Now}");
			try
			{
				await Start(GetAzureContext(context), log);
			}
			catch (Exception ex)
			{
				log.LogError(ex, ex.Message);
			}
		}

		[FunctionName("AciStopTimer")]
		public static async Task AciStopTimer([TimerTrigger("0 0 18 * * *")]TimerInfo myTimer, ILogger log, ExecutionContext context)
		{
			log.LogInformation($"C# Timer trigger function AciStopTimer executed at: {DateTime.Now}");
			try
			{

				await Stop(GetAzureContext(context), log);
			}
			catch (Exception ex)
			{
				log.LogError(ex, ex.Message);
			}
		}

		private static IAzure GetAzureContext(ExecutionContext context)
		{
			var azureAuthFile = Path.Combine(context.FunctionAppDirectory, AzureAuthFile);
			return Azure
				.Authenticate(azureAuthFile)
				.WithSubscription(SubscriptionName);
		}

		private static async Task Start(IAzure azure, ILogger log)
		{

			var acis = ContainerGroupNames.Split(",");
			foreach (var aci in acis)
			{
				await azure
					.ContainerGroups
					.StartAsync(ResourceGroupName, aci);
				log.LogInformation($"${aci} has been started");
			}
		}

		private static async Task Stop(IAzure azure, ILogger log)
		{

			var containers = azure
				.ContainerGroups
				.List()
				.ToList();

			var acis = ContainerGroupNames.Split(",");
			foreach (var aci in acis)
			{
				await azure
					.ContainerGroups
					.GetById(containers.FirstOrDefault(x => x.Name == aci)?.Id)
					.StopAsync();
				log.LogInformation($"${aci} has been stopped");
			}
		}
	}
}
