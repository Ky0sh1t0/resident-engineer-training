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
    public string Status {get; set;}

    public Subject(string name, int dangerLevel, bool isInfected, string status = "Contained") {
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
            if (Status == "Escaped") {
                Utils.Write("!!! CRITICAL ALERT: HIGH RISK ESCAPED SUBJECT !!!");
            }
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
        Utils.Write("3. Update subject status");
        Utils.Write("4. Exit");
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

    static void DisplaySubjects(List<Subject> subjects, string option) {
        bool isStillWatching = true;
        
        while (isStillWatching) {
            Utils.Write("=== SUBJECTS REGISTRY ===");
            if (subjects.Count == 0) {
                Utils.Write("Registry is empty");
                Utils.Write("Press any button to continue...");
                Console.ReadLine();
                return;
            } 

            int containedSubject = 0;
            int escapedSubject = 0;
            int terminatedSubject = 0;
            for (int i=0; i<subjects.Count;i++) {
                Utils.Write($"{i+1}. {subjects[i].Name}");
                if (subjects[i].Status == "Contained") {
                    containedSubject++;
                    continue;
                } else if (subjects[i].Status == "Escaped") {
                    escapedSubject++;
                    continue;
                } else {
                    terminatedSubject++;
                }
            }
            Utils.Write($"Total: {subjects.Count}");
            Utils.Write($"Contained: {containedSubject}");
            Utils.Write($"Escaped: {escapedSubject}");
            Utils.Write($"Terminated: {terminatedSubject}");
            string title;
            if (option == "Add") {
                title = "If you want to see the details, type a number of a subject or leave by typing '0'";
                // ShowDetailedInfo(subjects, ref isStillWatching);
                Validation(title,ShowDetailedInfo, subjects, ref isStillWatching);
            } else {
                title = "If you want to change the subject status type a number of a subject or leave by typing '0'";
                // UpdateStatus(subjects, ref isStillWatching);
                Validation(title, UpdateStatus, subjects, ref isStillWatching);
            }

        }
    }

    static void Validation(string title,Action<List<Subject>, int> action, List<Subject> subjects, ref bool isStillWatching) {
        Utils.Write($"{title}");
        int subjectIndexDetail;
        if (!int.TryParse(Console.ReadLine(), out subjectIndexDetail) || subjectIndexDetail > subjects.Count || subjectIndexDetail <= 0) {
            Utils.Write("Error, not a number or out of range of the subjects try again or type '0' in field");
        } else if (subjectIndexDetail == 0) {
            isStillWatching = false;
        } else {
            action(subjects, subjectIndexDetail);
        }
    }

    // validation creation
    static void ShowDetailedInfo(List<Subject> subjects, int index) {
        subjects[index-1].ShowInfo();
    }

    static void UpdateStatus(List<Subject> subjects, int index ) {
        List<string> statusList = ["Contained", "Escaped", "Terminated"];

        int subjectStatus;
        while (true) {
            Utils.Write("\nChoose new status");
            for (int i=0; i<statusList.Count;i++) {
                Utils.Write($"{i+1}.{statusList[i]}");
            }
            if (!int.TryParse(Console.ReadLine(), out subjectStatus) || subjectStatus > statusList.Count || subjectStatus < 0) {
                Utils.Write("Error, not a number or out of range of the subjects try again or type '0' in field");
                continue;
            }
            
            subjects[index - 1].Status = statusList[subjectStatus-1];
            break;
        }
        Utils.Write("Subject status updated successfully")
    }

    static void GetMenuItem(int selection,List<Subject> subjects, ref bool isRunning) {
        switch (selection) {
            case 1:
                AddSubject(subjects);
                break;
            case 2: 
                DisplaySubjects(subjects, "Add");
                break;
            case 3:
                DisplaySubjects(subjects, "UpdateStatus");
                break;
            case 4:
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
