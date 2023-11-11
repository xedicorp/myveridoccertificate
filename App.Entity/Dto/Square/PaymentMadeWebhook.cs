namespace App.Entity.Dto.Square
{
    public class PaymentMadeWebhook
    {
        public string? merchant_id { get; set; }
        public string? location_id { get; set; }
        public string? type { get; set; }
        public string? event_id { get; set; }
        public Data? data { get; set; }
    }

    public class Data
    {
        public string id { get; set; }
        public Object? @object { get; set; }
    }

    public class Object
    {
        public Invoice? invoice { get; set; }
    }

    public class Invoice
    {
        public string? created_at { get; set; }
        public string? delivery_method { get; set; }
        public string? id { get; set; }
        public string? invoice_number { get; set; }
        public string? location_id { get; set; }
        public string? order_id { get; set; }
        public PaymentRequests[]? payment_requests { get; set; }
        public PrimaryRecipient? primary_recipient { get; set; }
        public string? public_url { get; set; }
        public string? status { get; set; }
        public bool store_payment_method_enabled { get; set; }
        public string? subscription_id { get; set; }
        public string? timezone { get; set; }
        public string? title { get; set; }
        public string? updated_at { get; set; }
        public int version { get; set; }
    }

    public class PaymentRequests
    {
        public string? automatic_payment_source { get; set; }
        public string? card_id { get; set; }
        public ComputedAmountMoney? computed_amount_money { get; set; }
        public string? due_date { get; set; }
        public string? request_type { get; set; }
        public bool tipping_enabled { get; set; }
        public ComputedAmountMoney? total_completed_amount_money { get; set; }
        public string? uid { get; set; }
    }

    public class PrimaryRecipient
    {
        public Address? address { get; set; }
        public string? company_name { get; set; }
        public string? customer_id { get; set; }
        public string? email_address { get; set; }
        public string? family_name { get; set; }
        public string? given_name { get; set; }
    }

    public class ComputedAmountMoney
    {
        public int amount { get; set; }
        public string? currency { get; set; }
    }

    public class Address
    {
        public string? address_line_1 { get; set; }
        public string? address_line_2 { get; set; }
        public string? address_line_3 { get; set; }
        public string? administrative_district_level_1 { get; set; }
        public string? country { get; set; }
        public string? locality { get; set; }
        public string? postal_code { get; set; }
        public string? sublocality { get; set; }
    }

}
