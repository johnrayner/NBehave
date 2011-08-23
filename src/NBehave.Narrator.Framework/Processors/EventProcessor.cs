using System;
using NBehave.Narrator.Framework.Messages;
using NBehave.Narrator.Framework.Tiny;

namespace NBehave.Narrator.Framework.Processors
{
    public class EventProcessor : IMessageProcessor
    {
        public EventProcessor(ITinyMessengerHub hub, NBehaveConfiguration configuration)
        {
            if (AppDomain.CurrentDomain.FriendlyName == RunnerFactory.AppDomainName)
                return;

            hub.Subscribe<RunStartedEvent>(_ => configuration.EventListener.RunStarted());
            hub.Subscribe<RunFinishedEvent>(_ => configuration.EventListener.RunFinished());
            hub.Subscribe<FeatureStartedEvent>(_ => configuration.EventListener.FeatureStarted(_.Content));
            hub.Subscribe<FeatureNarrativeEvent>(_ => configuration.EventListener.FeatureNarrative(_.Content));
            hub.Subscribe<FeatureResultEvent>(_ => configuration.EventListener.FeatureFinished(_.Content));
            hub.Subscribe<ScenarioStartedEvent>(_ => configuration.EventListener.ScenarioStarted(_.Content.Title));
            hub.Subscribe<ScenarioResultEvent>(_ => configuration.EventListener.ScenarioFinished(_.Content));
        }
    }
}