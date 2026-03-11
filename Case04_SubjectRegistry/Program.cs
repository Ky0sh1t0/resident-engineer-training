public static class Utils {
    public static void Write(string line) {
        Console.WriteLine(line);
    }

    public static void WriteInLine(string line) {
        Console.Write(line);
    }
}

class Subject {
    public string Name {get; set;}
    public int DangerLevel {get; set;}
    public bool IsInfected {get; set;}

    public Subject(string name, int dangerLevel, bool isInfected) {
        Name = name;
        DangerLevel = dangerLevel;
        IsInfected = isInfected;
    }

    public void ShowInfo() {
        Utils.Write($"Name: {Name}");
        Utils.Write($"Danger Level: {DangerLevel}");
        Utils.WriteInLine("Is Infected: ");
        Utils.Write(IsInfected ? "YES" : "NO");
        if (DangerLevel >= 4) {
            Utils.Write("!!!WARNING!!!: HIGH THREAT SUBJECT");
        }
        if (IsInfected) {
            Utils.Write("Status: Quarantine required!!!");
        }
        Utils.Write("");
    }
}


class Program {

    static void ShowMenu() {
        Utils.Write("\n=== SUBJECT REGISTRY ===");
        Utils.Write("1. Add subject");
        Utils.Write("2. Show all subjects");
        Utils.Write("3. Exit");
    }

    static void AddSubject(List<Subject> subjects) {
        Utils.WriteInLine("Subject name: ");
        string name = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(name)) {
            Utils.Write("ERROR: Subject name cannot be empty");
            Utils.WriteInLine("Subject name: ");
            name = Console.ReadLine();
        }
        int dangerLevel;
        while(true) {
            Utils.WriteInLine("Subject Danger Level from 0 to 5: ");
            if (int.TryParse(Console.ReadLine(), out dangerLevel) && 0 <= dangerLevel && dangerLevel <= 5) {
                break;
            } else {
                Utils.Write("Error: try number instead of text or is under 0 or over 5");
                continue;
            }
        }
        Utils.WriteInLine("Is Subject Infected? 'Yes' or 'No': ");
        bool isInfected = false;
        while (true) {
            string tmp = Console.ReadLine().ToLower();
            if (tmp == "yes") {
                isInfected = true;
                break;
            } else if (tmp == "no") {
                isInfected = false;
                break;
            } else {
                Utils.Write("ERROR: wrong text, try again");
                continue;
            }
        }

        Subject subject = new Subject(name, dangerLevel, isInfected);
        subjects.Add(subject);
    }

    static void DisplaySubjects(List<Subject> subjects) {
        bool isStillWatching = true;
        
        while (isStillWatching) {
            Utils.Write("=== Subjects Registry ===");
            if (subjects.Count == 0) {
                Utils.Write("Registry is empty");
                Utils.Write("Press any button to continue...");
                Console.ReadLine();
                return;
            } 

            for (int i=0; i<subjects.Count;i++) {
                Utils.Write($"{i+1}. {subjects[i].Name}");
            }

            Utils.Write("If you want to see the details, type a number of a subject");
            int subjectIndexDetail;
            if (!int.TryParse(Console.ReadLine(), out subjectIndexDetail) || subjectIndexDetail > subjects.Count || subjectIndexDetail < 0) {
                Utils.Write("Error, not a number or out of range of the subjects try again or type '0' in field or leave it empty");
            } else if (subjectIndexDetail == 0) {
                isStillWatching = false;
            } else {
                subjects[subjectIndexDetail-1].ShowInfo();
                continue;
            }

        }
    }

    static void GetMenuItem(int selection,List<Subject> subjects, ref bool isRunning) {
        switch (selection) {
            case 1:
                AddSubject(subjects);
                break;
            case 2: 
                DisplaySubjects(subjects);
                break;
            case 3:
                isRunning = false;
                break;
            default: 
                Utils.Write("ERROR: NOT AN OPTION");
                break;                
        }
    }

    static void Main() {
        bool isRunning = true;
        List<Subject> subjects = new List<Subject>();

        while(isRunning) {
            ShowMenu();
            int itemPick;
            if (!int.TryParse(Console.ReadLine(), out itemPick)) {
                Utils.Write("ERROR: Choose the option by typing a number");
                continue;
            }
            GetMenuItem(itemPick, subjects, ref isRunning);
        }
    }
}
