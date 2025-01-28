namespace moneygram_api.DTOs;

using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

public class ConsumerLookUpRequestDTO
{
    public required string CustomerPhone { get; set; }
    public required int SendersToReturn { get; set; }
}