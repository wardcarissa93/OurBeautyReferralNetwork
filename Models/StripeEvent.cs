namespace OurBeautyReferralNetwork.Models
{
    public class StripeEvent
    {
        public string Id { get; set; }
        public string Object { get; set; }
        public string ApiVersion { get; set; }
        public int Created { get; set; }
        public Data Data { get; set; }
        public bool Livemode { get; set; }
        public int PendingWebhooks { get; set; }
        public Request Request { get; set; }
        public string Type { get; set; }
    }

    public class Data
    {
        public ObjectObject Object { get; set; }
    }

    public class ObjectObject
    {
        public string Id { get; set; }
        public string Object { get; set; }
        public int Amount { get; set; }
        // Other properties you may need
    }

    public class Request
    {
        public string Id { get; set; }
        public Guid IdempotencyKey { get; set; }
    }

}
