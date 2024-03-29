using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
// Provide a Uri string below. Here guest:guest is the username and password respectively,
// 5672 is the port for sending queue messages to the server, defined in docker container
factory.Uri =new Uri("amqp://guest:guest@localhost:5672");
factory.ClientProvidedName = "Rabbit Sender App"; //Any name that you provide

IConnection cnn = factory.CreateConnection();

// IModel is the interface of RabbitMQ to create a model
IModel channel = cnn.CreateModel();

string exchangeName = "DemoExchange";
string routingKey = "demo-routing-key";
string queueName= "DemoQueue";

channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
channel.QueueDeclare(queueName,false,false,false,null);
channel.QueueBind(queueName,exchangeName,routingKey,null);

// When you send a message through a queue, it will just send as byte
// We have a message encoded as a byte array to be sent
byte[] messageBodyBytes = Encoding.UTF8.GetBytes("Hello Anvay");

//Use the channel to publish in the exchange
channel.BasicPublish(exchangeName,routingKey,null,messageBodyBytes);

channel.Close();
cnn.Close();