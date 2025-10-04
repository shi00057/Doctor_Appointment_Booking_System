namespace CST8002.Application.Domain.ValueObjects
{
    public readonly struct EmailAddress
    {
        public string Value { get; }
        public EmailAddress(string value) { Value = value; }
        public override string ToString() => Value;
    }
}