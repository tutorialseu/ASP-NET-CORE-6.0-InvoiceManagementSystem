namespace IdentityApp.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }

        public double InvoiceAmount { get; set; }

        public string InvoiceMonth { get; set; }

        public string InvoiceOwner { get; set; }

        public string CreatorId { get; set; }

        public InvoiceStatus Status { get; set; }
    }
}

namespace IdentityApp
{
    public enum InvoiceStatus
    {
        Submitted,
        Approved,
        Rejected
    }
}
