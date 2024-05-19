
using Confluent.Kafka;
using OldButGold.Domain.Models;
using System.Text;
using System.Text.Json;

namespace OldButGold.API
{
    public class KafkaConsumer(
        IConsumer<byte[], byte[]> consumer,
        ILogger<KafkaConsumer> logger) : BackgroundService
    {


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

            logger.LogInformation("Subscribing to topic...");
            consumer.Subscribe("obg.DomainEvents");
            do
            {
                var consumeResult = consumer.Consume(stoppingToken);
                if(consumeResult is null || consumeResult.IsPartitionEOF)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                    continue;
                }
                var domainEvent = JsonSerializer.Deserialize<DomainEvent>(consumeResult.Message.Value);

                var contentBlob = Convert.FromBase64String(domainEvent.ContentBlob);
                var topic = JsonSerializer.Deserialize<Topic>(contentBlob);

                logger.LogInformation("Message about topic {TopicId} receiver", topic.Id);

                consumer.Commit(consumeResult);
            } while (!stoppingToken.IsCancellationRequested);

            consumer.Close();

        }

        public class DomainEvent
        {
            public string ContentBlob { get; set; }
        }
    }
}
