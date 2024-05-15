
using DevFreela.Payments.API.Interfaces;
using DevFreela.Payments.API.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace DevFreela.Payments.API.Consumer
{
    public class PaymentConsumer : BackgroundService
    {
        private readonly IConnectionFactory _factory;
        private readonly IModel _model;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private string PAYMENT_PROCESSING_QUEUE;
        private string PAYMENT_FINISHED_QUEUE;

        public PaymentConsumer(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;

            _factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
            PAYMENT_PROCESSING_QUEUE = _configuration["RabbitQueues:PAYMENT_PROCESSING"]!;
            PAYMENT_FINISHED_QUEUE = _configuration["RabbitQueues:PAYMENT_FINISHED"]!;

            var connection = _factory.CreateConnection();
            _model = connection.CreateModel();

            _model.QueueDeclare(
                       queue: PAYMENT_FINISHED_QUEUE,
                       durable: false,
                       autoDelete: false,
                       exclusive: false,
                       arguments: null
                       );
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_model);

            consumer.Received += (sender, EventArgs) =>
            {
                var contentArray = EventArgs.Body.ToArray();
                var contentString = Encoding.UTF8.GetString(contentArray);
                var message = JsonSerializer.Deserialize<PaymentInput>(contentString);

                ProcessPendingPayment(message!);

                PaymentFinishedInfoPublish(message!);

                _model.BasicAck(EventArgs.DeliveryTag, false);

            };

            _model.BasicConsume(PAYMENT_PROCESSING_QUEUE, false, consumer);
            return Task.CompletedTask;

        }

        private void ProcessPendingPayment(PaymentInput message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentsService>();
                paymentService.CheckPaymentIsCompletedAsync(message);
            };
        }

        private void PaymentFinishedInfoPublish(PaymentInput paymentInput)
        {
            var eventInput = new PaymentProjectEventApproved(paymentInput.IdProject);
            var messageJson = JsonSerializer.Serialize(eventInput);
            var contentMessage = Encoding.UTF8.GetBytes(messageJson);

            _model.BasicPublish("", PAYMENT_FINISHED_QUEUE, null, contentMessage);
        }
    }
}
