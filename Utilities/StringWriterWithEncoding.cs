namespace moneygram_api.Utilities;

using System.IO;
using System.Text;

public class StringWriterWithEncoding : StringWriter
{
    private readonly Encoding encoding;

    public StringWriterWithEncoding(Encoding encoding)
    {
        this.encoding = encoding;
    }

    public override Encoding Encoding => encoding;
}