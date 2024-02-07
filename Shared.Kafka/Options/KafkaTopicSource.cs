namespace Shared.Kafka.Options;

public enum KafkaTopicSource
{
    Default,
    CustomersRegistration,
    WorkersRegistration,
    CustomersUpdate,
    UsersDeletion,
    AccountCreation,
    AccountDeletion,
    TransactionsCreation
}