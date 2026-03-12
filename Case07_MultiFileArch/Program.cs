using ResidentEngineer.Models;
using ResidentEngineer.Services;

namespace ResidentEngineer;

class Program {
    static void Main() {
        bool isRunning = true;

        List<Subject> subjects = SubjectService.LoadSubjects();

        while(isRunning) {
            SubjectService.ShowMenu();
            if (!int.TryParse(Console.ReadLine(), out int itemPick)) {
                Utils.Write("ERROR: Choose the option by typing a number");
                continue;
            }
            SubjectService.GetMenuItem(itemPick, subjects, ref isRunning);
        }
    }
}
