using moneygram_api.Models;
using moneygram_api.Services.Interfaces;
using moneygram_api.Settings;
public class BankBranchService : IBankBranchService
{
    private readonly IConfigurations _configurations;
    private readonly string _csvFilePath;
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
    private readonly Lazy<Dictionary<string, BankBranch>> _branches;

    public BankBranchService(IConfigurations configurations)
    {
        _configurations = configurations;
        _csvFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _configurations.IndiaBankBranchesFilePath);
        _branches = new Lazy<Dictionary<string, BankBranch>>(() => LoadBranches(_csvFilePath), LazyThreadSafetyMode.ExecutionAndPublication);
    }

    private static Dictionary<string, BankBranch> LoadBranches(string csvFilePath)
    {
        var branches = new Dictionary<string, BankBranch>(StringComparer.OrdinalIgnoreCase);

        try
        {
            using (var reader = new StreamReader(csvFilePath))
            {
                reader.ReadLine(); // Skip header
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var values = ParseCsvLine(line);
                    if (values.Length < 8) continue;

                    var branch = new BankBranch
                    {
                        IFSCCode = values[0].Trim(),
                        BankName = values[1].Trim(),
                        BranchName = values[2].Trim(),
                        StreetAddress = values[3].Trim(),
                        City = values[4].Trim(),
                        State = values[5].Trim(),
                        ContactInfo = values[6].Trim(),
                        MICRCode = values[7].Trim()
                    };

                    if (!branches.ContainsKey(branch.IFSCCode))
                    {
                        branches[branch.IFSCCode] = branch;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to load bank branches", ex);
        }

        return branches;
    }

    public async Task<List<BankBranch>> GetAllBranchesAsync()
    {
        await _semaphore.WaitAsync();
        try
        {
            return _branches.Value.Values.ToList();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<BankBranch> GetBranchByIFSCAsync(string ifscCode)
    {
        if (string.IsNullOrWhiteSpace(ifscCode))
            return null;

        await _semaphore.WaitAsync();
        try
        {
            _branches.Value.TryGetValue(ifscCode.Trim(), out var branch);
            return branch;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private static string[] ParseCsvLine(string line)
    {
        var result = new List<string>();
        bool inQuotes = false;
        var field = new System.Text.StringBuilder();

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];
            if (c == '"' && (i == 0 || line[i-1] != '\\'))
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                result.Add(field.ToString());
                field.Clear();
            }
            else
            {
                field.Append(c);
            }
        }
        result.Add(field.ToString());
        return result.ToArray();
    }
}