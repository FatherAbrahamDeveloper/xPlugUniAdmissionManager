using Serilog.Events;

namespace xPlugUniAdmissionManager;

public class SerilogOptions
{
    public const string SectionName = "SerilogServiceSettings";
    public bool EnableConsole { get; set; }
    public bool EnableFile { get; set; }
    public bool EnableSeq { get; set; }
    public LogEventLevel LogEventLevel { get; set; }
    public string FilePath { get; set; } = default!;
    public string FileUrl
    {
        get
        {
            return (!string.IsNullOrEmpty(FilePath) && FilePath.Length > 5) ? $"{AppDomain.CurrentDomain.BaseDirectory}{FilePath}" : "";
        }
    }
    //public string SeqApiKey { get; set; } = default!;
    //public string SeqURL { get; set; } = default!;
}
