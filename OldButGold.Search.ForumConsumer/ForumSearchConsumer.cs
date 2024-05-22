﻿using Confluent.Kafka;
using Microsoft.Extensions.Options;
using OldButGold.Search.API.Grpc;
using OldButGold.Search.ForumConsumer.Monitoring;
using OpenTelemetry.Metrics;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace OldButGold.Search.ForumConsumer
{
    public class ForumSearchConsumer(
        IConsumer<byte[], byte[]> consumer,
        SearchEngine.SearchEngineClient searchEngineClient,
        IOptions<ConsumerConfig> consumerConfig) : BackgroundService
    {

        private readonly ConsumerConfig consumerConfig = consumerConfig.Value;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

            consumer.Subscribe("obg.DomainEvents");
            while (!stoppingToken.IsCancellationRequested)
            {
                var consumeResult = consumer.Consume(stoppingToken);
                if (consumeResult is not { IsPartitionEOF: false })
                {
                    await Task.Delay(300, stoppingToken);
                    continue;
                }

                var activityId = consumeResult.Message.Headers.TryGetLastBytes("activity_id", out var lastBytes)
                ? Encoding.UTF8.GetString(lastBytes)
                : null;

                using var activity = Metrics.ActivitySource.StartActivity("consumer", ActivityKind.Consumer,
                 ActivityContext.TryParse(activityId, null, out var context) ? context : default);
                activity?.AddTag("messaging.system", "kafka");
                activity?.AddTag("messaging.destination.name", "obg.DomainEvents");
                activity?.AddTag("messaging.kafka.consumer_group", consumerConfig.GroupId);
                activity?.AddTag("messaging.kafka.partition", consumeResult.Partition);

                var domainEventWrapper = JsonSerializer.Deserialize<DomainEventWrapper>(consumeResult.Message.Value)!;
                var contentBlob = Convert.FromBase64String(domainEventWrapper.ContentBlob);
                var domainEvent = JsonSerializer.Deserialize<ForumDomainEvent>(contentBlob);

                switch (domainEvent.EventType)
                {
                    case ForumDomainEventType.TopicCreated:
                        await searchEngineClient.IndexAsync(new IndexRequest
                        {
                            Id = domainEvent.TopicId.ToString(),
                            Type = SearchEntityType.ForumTopic,
                            Title = domainEvent.Title,
                        }, cancellationToken: stoppingToken);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            consumer.Close();
        }
    }
}
