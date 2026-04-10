using ResidentEngineer.Models;
using ResidentEngineer.Enums;

namespace ResidentEngineer.Services;

public static class SubjectService {
    static Random rnd = new Random();

    static SubjectStatus[] GetStatus() {
        return Enum.GetValues<SubjectStatus>();
    }

    public static void ShowMenu() {
        Utils.Write("\n=== SUBJECT REGISTRY ===");
        Utils.Write("1. Add subject");
        Utils.Write("2. Show all subjects");
        Utils.Write("3. Update subject status");
        Utils.Write("4. Search subject");
        Utils.Write("5. Filter by status");
        Utils.Write("6. Delete Subject");
        Utils.Write("7. Exit");
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
                Utils.Write("ERROR: Enter a number from 0 to 5");
                continue;
            }
        }
        Utils.WriteInLine("Is Subject Infected? 'Yes' or 'No': ");
        bool isInfected = false;
        while (true) {
            string tmp = Console.ReadLine().ToLowerInvariant();
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
        StorageService.SaveSubjects(subjects);
    }

    static void DisplaySubjects(List<Subject> subjects, DisplayMode mode) {
        bool isStillWatching = true;
        
        while (isStillWatching) {
            Utils.Write("\n=== SUBJECT REGISTRY ===");
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
                Utils.Write($"{i+1}. {subjects[i].Name} - {subjects[i].Status}");
                if (subjects[i].Status == SubjectStatus.Contained) {
                    containedSubject++;
                    continue;
                } else if (subjects[i].Status == SubjectStatus.Escaped) {
                    escapedSubject++;
                    continue;
                } else {
                    terminatedSubject++;
                }
            }
            Utils.Write($"\nTotal: {subjects.Count}");
            Utils.Write($"Contained: {containedSubject}");
            Utils.Write($"Escaped: {escapedSubject}");
            Utils.Write($"Terminated: {terminatedSubject}");
            string title;
            if (mode == DisplayMode.DisplayDetails) {
                title = "If you want to see the details, enter a number of a subject or leave by typing '0'";
                Validation(title,ShowDetailedInfo, subjects, ref isStillWatching);
            } else if (mode == DisplayMode.UpdateStatus) {
                title = "If you want to change the subject status enter a number of a subject or leave by typing '0'";
                Validation(title, UpdateStatus, subjects, ref isStillWatching);
            } else if (mode == DisplayMode.DeleteSubject) {
                title = "If you want to delete subject enter a number or leave by typing '0'";
                Validation(title, DeleteSubject,subjects, ref isStillWatching);
            }

        }
    }

    static void Validation(string title,Action<List<Subject>, int> action, List<Subject> subjects, ref bool isStillWatching) {
        
        Utils.Write($"{title}");
        int subjectIndexDetail;
        if (!int.TryParse(Console.ReadLine(), out subjectIndexDetail) 
            || subjectIndexDetail > subjects.Count 
            || subjectIndexDetail < 0) 
        {
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

    static void ShowStatusDialog(string title) {
        Utils.Write($"{title}");
        int iStatus = 1;
        var statuses = GetStatus();
        foreach (var s in statuses) {
            Utils.Write($"{iStatus}. {s}");
            iStatus++;
        }
    }

    static void UpdateStatus(List<Subject> subjects, int index ) {
        
        int subjectStatus;
        var statuses = GetStatus();
        
        while (true) {
            ShowStatusDialog("\nChoose new status");
            if (!int.TryParse(Console.ReadLine(), out subjectStatus) || subjectStatus <= 0 || subjectStatus > statuses.Length) {
                Utils.Write("Error, not a number or out of range of the subjects try again or type '0' in field");
                continue;
            }
            
            subjects[index - 1].Status = statuses[subjectStatus-1];
            StorageService.SaveSubjects(subjects);
            break;
        }
        
        Utils.Write("Subject status updated successfully");
    }

    public static void GetMenuItem(int selection,List<Subject> subjects, ref bool isRunning) {
        switch (selection) {
            case 1:
                AddSubject(subjects);
                EscapeEvent(subjects);
                break;
            case 2: 
                DisplaySubjects(subjects, DisplayMode.DisplayDetails);
                break;
            case 3:
                DisplaySubjects(subjects, DisplayMode.UpdateStatus);
                EscapeEvent(subjects);
                break;
            case 4:
                SearchSubjectDisplay(subjects);
                break;
            case 5:
                FilterStatusDisplay(subjects);
                break;
            case 6:
                DisplaySubjects(subjects, DisplayMode.DeleteSubject);
                break;
            case 7:
                StorageService.SaveSubjects(subjects);
                isRunning = false;
                break;
            default:
                Utils.Write("ERROR: NOT AN OPTION");
                break;                
        }
    }   

    static void EscapeEvent(List<Subject> subjects) {
        List<Subject> containedSubjects = subjects.Where(subject => subject.Status == SubjectStatus.Contained).ToList();
        if (containedSubjects.Count == 0) {
            return;
        } 
        if (rnd.Next(100) < 10) {
            int tmpIndex = rnd.Next(containedSubjects.Count);
            containedSubjects[tmpIndex].Status = SubjectStatus.Escaped;
            Utils.Write($"!!! ALERT: SUBJECT {containedSubjects[tmpIndex].Name} ESCAPED");
            StorageService.SaveSubjects(subjects);
        }
    }


    // Search 
    static void SearchSubjectDisplay(List<Subject> subjects) {
        List<Subject> filteredSubjects = new List<Subject>(); 
        string filterText;
        while(true) {
            Utils.WriteInLine("Enter a word to find a subject: ");
            filterText = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(filterText)) {
                Utils.Write("ERROR: Field is empty");
                continue;
            }
            filteredSubjects = SearchSubject(subjects, filterText);
            break;
        }
        
        if (filteredSubjects.Count > 0) {
            Utils.Write("All found subjects:");
            foreach (Subject subject in filteredSubjects) {
                Utils.Write($"{subject.Name} - {subject.Status}");
            }
        } else {
            Utils.Write($"Such subjects not found");
        }
    }

    static List<Subject> SearchSubject(List<Subject> subjects, string filterText) {
        return subjects.Where(subject => subject.Name.Contains(filterText, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    // Filter

    static void FilterStatusDisplay(List<Subject> subjects) {
        var statuses = GetStatus();

        while(true) {
            ShowStatusDialog("\nChoose one to see all subjects under this status");
            if(!int.TryParse(Console.ReadLine(), out int indexStatus) || indexStatus > statuses.Length || indexStatus <= 0) {
                Utils.Write("ERROR: Wrong choice from the options");
                continue;
            }
            List<Subject> filteredSubjects = FilterByStatus(subjects, statuses[indexStatus-1]);
            if (filteredSubjects.Count == 0) {
                Utils.Write($"\n0 Subjects found under status {statuses[indexStatus-1]}");
                break;
            }
            Utils.Write($"\nAll subjects with {statuses[indexStatus-1]} status:");
            foreach (Subject subject in filteredSubjects) {
                subject.ShowInfo();
            }
            break;
            
        }
    }

    static List<Subject> FilterByStatus(List<Subject> subjects, SubjectStatus status) {
        return subjects.Where(subject => subject.Status == status).ToList();
    }

    // Delete

    static void DeleteSubject(List<Subject> subjects, int indexDelete) {
        Subject deletedSubject = subjects[indexDelete-1];
        Utils.Write($"Are you sure that you want to delete the subject: {deletedSubject.Name}? y/n");
        string agreement;
        while (true) {
            agreement = Console.ReadLine().ToLowerInvariant();
            if (agreement == "y" || agreement == "yes") {
                subjects.RemoveAt(indexDelete-1);
                Utils.Write($"Subject - {deletedSubject.Name} was successfully deleted");
                StorageService.SaveSubjects(subjects);
                break;
            } else if (agreement == "n" || agreement == "no") {
                break;
            } else {
                Utils.Write("ERROR: Enter 'y'/'yes' to delete or 'n'/'no' to cancel");
            }
        }
    }
}