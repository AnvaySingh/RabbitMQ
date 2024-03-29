using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
// Provide a Uri string below. Here guest:guest is the username and password respectively,
// 5672 is the port for sending queue messages to the server, defined in docker container
factory.Uri =new Uri("amqp://guest:guest@localhost:5672");
factory.ClientProvidedName = "Rabbit Receiver1 App"; //Any name that you provide

//Connection
IConnection cnn = factory.CreateConnection();

// IModel is the interface of RabbitMQ to create a model
IModel channel = cnn.CreateModel();

string exchangeName = "DemoExchange";
string routingKey = "demo-routing-key";
string queueName= "DemoQueue";

channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
channel.QueueDeclare(queueName,false,false,false,null);
channel.QueueBind(queueName,exchangeName,routingKey,null);

//prefetchSize:0 - means not limiting the size of the message
//prefetchCount:1 - 1 message to be sent at once. Defining the number of messages to process at a time.
//globa:false - means to apply this setting to the current instance only
channel.BasicQos(0,1,false);

//Consumer setup
var consumer=new EventingBasicConsumer(channel);
consumer.Received +=(sender, args) => 
{
    var body= args.Body.ToArray();

    //Decode the message back to string which was passed earlier in RabbitMQDemo
    string message=Encoding.UTF8.GetString(body);

    Console.WriteLine($"Message Received: {message}");

    // Acknowledgement of the message delivered
    channel.BasicAck(args.DeliveryTag, false);
};

//Tag of the overall consume system
string ConsumerTag =  channel.BasicConsume(queueName,false,consumer);

Console.ReadLine();

// Handle the consumer system and close it
channel.BasicCancel(ConsumerTag);

channel.Close();
cnn.Close();