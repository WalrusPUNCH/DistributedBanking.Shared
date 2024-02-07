namespace Shared.Redis.Models;

public static class RedisChannelConstants
{
    public const string CustomersRegistrationChannel = "customers:registration";
    public const string WorkersRegistrationChannel = "workers:registration";
    public const string CustomersUpdateChannel = "customers:update";
    public const string UsersDeletionChannel = "users:deletion";
    public const string AccountCreationChannel = "accounts:creation";
    public const string AccountDeletionChannel = "accounts:deletion";
    public const string TransactionsCreationChannel = "transactions:creation";
}
