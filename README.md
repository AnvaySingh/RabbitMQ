A .NET console application to understand the basics of RabbitMQ with the sender app (RabbitMQDemo) and a receiver app(RabbitReceiver1) to send a message to and fro from the RabbitMQ server.

*For Rabbit MQ server:*

For this a docker container was set up for rabbitMQ using the command below:

docker run -d --hostname rmq --name rabbit-server -p 8080:15672 -p 5672:5672 rabbitmq: 3-management

This would create a docker container running a RabbitMQ server (here management keyword defines that a management package is attached) 

Further details can be found in the article here: https://anvay.hashnode.dev/rabbit-mq-intro-with-net
