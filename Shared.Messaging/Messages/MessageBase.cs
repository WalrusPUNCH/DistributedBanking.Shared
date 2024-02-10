namespace Shared.Messaging.Messages;

public abstract record MessageBase
{
    public abstract string ResponseChannelPattern { get; }
}