using System.IO;
using System.Text.Json;
using ResidentEngineer.Models;

namespace ResidentEngineer.Services;

public static class StorageService{
    public static List<Subject> LoadSubjects() {
        if (File.Exists("subjects.json")) {
            var json = File.ReadAllText("subjects.json");
            return JsonSerializer.Deserialize<List<Subject>>(json) ?? new List<Subject>();
        }

        return new List<Subject>();
    }

    public static void SaveSubjects(List<Subject> subjects) {
        var json = JsonSerializer.Serialize(subjects, new JsonSerializerOptions{
            WriteIndented = true
        });
        File.WriteAllText("subjects.json", json);
    }
}