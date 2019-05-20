#r "Newtonsoft.Json"

using System;

using System.Text;

using System.Threading.Tasks;

using Microsoft.Azure.EventHubs;

using Newtonsoft.Json;


 


public class Order

{

    public string CustId;

    public DateTime OrderTime;

    public string OrderNumber;

	public string Location;

    public int TotalAmount;

}

public static void Run(TimerInfo myTimer, ILogger log)

{	
		 Random random = new Random();
		 string[] Alllocation = { "USA", "JPN", "GBR", "FRA","TUR" };
		
		
        log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

        EventHubClient eventHubClient;

        string EventHubConnectionString = "test";

        string EventHubName = "eventhub2";

        var connectionStringBuilder = new EventHubsConnectionStringBuilder(EventHubConnectionString)

        {

            EntityPath = EventHubName

        };


 


        eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

        for (var i = 0; i < 100; i++)

        {

            try

            {
				
                log.LogInformation($"Sending message: ");

                Order neworder = new Order(); 

                neworder.CustId = String.Concat("GAPCustomer",i);     

                neworder.OrderTime = DateTime.Now;
				int index = random.Next(Alllocation.Length);
				neworder.Location = Alllocation[index];

                neworder.OrderNumber=String.Concat("GAPOrder",i);  
				int randomNumber = random.Next(0, 1000);
                neworder.TotalAmount=randomNumber;

                var message = JsonConvert.SerializeObject(neworder);

                log.LogInformation($"Sending message: {message}");

                eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));

            }

            catch (Exception exception)

            {

                Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");

            }

        }

}