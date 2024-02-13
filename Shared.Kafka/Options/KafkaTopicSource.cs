namespace Shared.Kafka.Options;

public enum KafkaTopicSource
{
    Default,
    RoleCreation,
    CustomersRegistration,
    WorkersRegistration,
    CustomersUpdate,
    UsersDeletion,
    AccountCreation,
    AccountDeletion,
    TransactionsCreation
}