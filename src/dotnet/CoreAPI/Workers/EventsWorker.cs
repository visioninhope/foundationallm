﻿using FoundationaLLM.Common.Interfaces;

namespace FoundationaLLM.Core.API.Workers
{
    /// <summary>
    /// Background worker used to execute the event service.
    /// </summary>
    /// <param name="eventService">The <see cref="IEventService"/> to run.</param>
    /// <param name="logger">The logger used for logging.</param>
    public class EventsWorker(
        IEventService eventService,
        ILogger<EventsWorker> logger) : BackgroundService
    {
        /// <inheritdoc/>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("The events worker is starting up the event service.");
            await eventService.StartAsync(stoppingToken);

            logger.LogInformation("The events worker is preparing to start the event service.");
            await eventService.ExecuteAsync(stoppingToken);
        }

        /// <inheritdoc/>
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("The events worker is stopping up the event service.");
            await eventService.StopAsync(cancellationToken);
        }
    }
}
