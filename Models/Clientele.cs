using System;

namespace moneygram_api.Models
{
    public class Clientele
    {
        public long ID { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public string NationalID { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Suburb { get; set; }
        public byte[] NatID_Image { get; set; }
        public string Img_Format { get; set; }
        public string ContentType { get; set; }
        public DateTime? AddDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? Modification_Reason { get; set; }
        public string? ModifiedBy { get; set; }
    }
}