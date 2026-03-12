using ResidentEngineer.Enums;

namespace ResidentEngineer.Models;


public class Subject {
    public string Name {get; set;}
    public int DangerLevel {get; set;}
    public bool IsInfected {get; set;}
    public SubjectStatus Status {get; set;}

    public Subject(string name, int dangerLevel, bool isInfected, SubjectStatus status = SubjectStatus.Contained) {
        Name = name;
        DangerLevel = dangerLevel;
        IsInfected = isInfected;
        Status = status;
    }

    public void ShowInfo() {
        Utils.Write($"Name: {Name}");
        Utils.Write($"Danger Level: {DangerLevel}");
        Utils.WriteInLine("Is Infected: ");
        Utils.Write(IsInfected ? "YES" : "NO");
        Utils.Write($"Status: {Status}");
        if (DangerLevel >= 4) {
            Utils.Write("!!! WARNING: HIGH THREAT SUBJECT !!!");
            if (Status == SubjectStatus.Escaped) {
                Utils.Write("!!! CRITICAL ALERT: HIGH RISK ESCAPED SUBJECT !!!");
            }
        }
        if (IsInfected) {
            Utils.Write("Status: Quarantine required!!!");
        }
        Utils.Write("");
    }
}