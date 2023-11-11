using Newtonsoft.Json;

namespace App.Entity.Dto.Square
{
    public class PaymentUpdateWebhook
    {
        [JsonProperty("merchant_id")]
        public string? MerchantId { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("event_id")]
        public string? EventId { get; set; }

        [JsonProperty("created_at")]
        public string? CreateAt { get; set; }

        [JsonProperty("data")]
        public PaymentData? PaymentData { get; set; }
    }

    public class PaymentData
    {
        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("object")]
        public PaymentObject? PaymentObject { get; set; }
    }

    public class PaymentObject
    {
        [JsonProperty("payment")]
        public Payment? Payment { get; set; }

    }

    public class Payment
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("created_at")]
        public string? CreateAt { get; set; }

        [JsonProperty("updated_at")]
        public string? UpdateAt { get; set; }

        [JsonProperty("status")]
        public string? Status { get; set; }

        [JsonProperty("card_details")]
        public CardDetials? CardDetials { get; set; }

        [JsonProperty("location_id")]
        public string? LocationId { get; set; }

        [JsonProperty("order_id")]
        public string? OrderId { get; set; }

        [JsonProperty("receipt_number")]
        public string? ReceiptNumber { get; set; }

        [JsonProperty("receipt_url")]
        public string? ReceiptUrl { get; set; }

        [JsonProperty("reference_id")]
        public string? ReferenceId { get; set; }

    }

    public class CardDetials
    {
        [JsonProperty("status")]
        public string? Status { get; set; }

        [JsonProperty("card")]
        public Card? Card { get; set; }
    }

    public class Card
    {
        [JsonProperty("card_brand")]
        public string? CardBrand { get; set; }

        [JsonProperty("last_4")]
        public string? Last4 { get; set; }

        [JsonProperty("card_type")]
        public string? CardType { get; set; }
    }
}