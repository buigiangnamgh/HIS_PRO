using Inventec.Common.WebApiClient;

namespace His.UC.UCHein.Base
{
	public class ApiConsumerStore
	{
		private static ApiConsumer mosConsumer;

		public static ApiConsumer MosConsumer
		{
			get
			{
				return mosConsumer;
			}
			set
			{
				mosConsumer = value;
			}
		}
	}
}
