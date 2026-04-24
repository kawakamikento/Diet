using System.Text.Json;
using DietApp.Models;

namespace DietApp.Services;

public class DataService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    private readonly string _filePath =
        Path.Combine(FileSystem.AppDataDirectory, "data.json");

    private DietData? _cache;

    public async Task<DietData> LoadAsync()
    {
        if (_cache is not null)
            return _cache;

        if (!File.Exists(_filePath))
        {
            _cache = new DietData();
            return _cache;
        }

        await using var stream = File.OpenRead(_filePath);
        _cache = await JsonSerializer.DeserializeAsync<DietData>(stream, JsonOptions)
                 ?? new DietData();
        return _cache;
    }

    public async Task SaveAsync(DietData data)
    {
        _cache = data;
        await using var stream = File.Create(_filePath);
        await JsonSerializer.SerializeAsync(stream, data, JsonOptions);
    }
}
