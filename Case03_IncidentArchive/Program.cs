static void Write(string line="") {
    Console.WriteLine(line);
}


int terminalVariant;
bool isRunning = true;
List<string> incidents = new List<string>();

static void AddIncident(List<string> newIncidents) {
    Write("\n=== ADD THE INCIDENT ===");
    Write("Type the incident that is happening right now");
    Write("type 'exit' to stop");
    Write();
    
    do {
        string incident = Console.ReadLine();
        if (incident == "exit") {
            break;
        }
        if (string.IsNullOrWhiteSpace(incident)) {
            Write("ERROR: Empty incident report");
        } else {
            newIncidents.Add(incident);
            Write("\nWrite new incident or type 'exit' to leave the ADD section");
        }
    } while (true);
}

static void RemoveIncident(List<string> newIncidents) {
    if (newIncidents.Count == 0) {
        Write("Archive is empty");
        return;
    }
    Write("\n=== REMOVE THE INCIDENT ===");
    ShowAllIncidents(newIncidents);
    Write();
    Write("Choose the number of an incident from the list");
    
    while (true) {
        if (!int.TryParse(Console.ReadLine(), out int x)) {
            Write("ERROR: INVALID INPUT TYPE, USE NUMBER");
            continue;
        }
        if (x > 0 && x-1 < newIncidents.Count) {
            newIncidents.RemoveAt(x-1);
            Write("Incident removed successfully");
            break;
        } else {
            Write("ERROR: THERE'S NO INCIDENT UNDER THIS NUMBER");
            continue;
        }
    }
}

static void ShowAllIncidents(List<string> newIncidents) {
    Write("\n=== LIST OF ALL INCIDENTS ===");
    int index = 0;

    if (newIncidents.Count == 0) {
        Write("Archive is empty");
    } else {
        foreach(string incident in newIncidents) {
            index++;
            Write($"{index}. {incident}");
        }
    }
    Write($"\nTotal incidents: {newIncidents.Count}");
}

static void ShowMenu() {
    Write("=== INCIDENT ARCHIVE ===");
    Write("1. Add incident");
    Write("2. Show all incidents");
    Write("3. Remove incident");
    Write("0. Exit");
}

while(isRunning) {
    ShowMenu();
    if(!int.TryParse(Console.ReadLine(), out terminalVariant)){
        Write("ERROR: ONLY NUMBERS ALLOWED");
        continue;
    }

    switch (terminalVariant){
        case 1:
            AddIncident(incidents);
            break;
        case 2:
            ShowAllIncidents(incidents);
            Console.Write("Press any button to continue...");
            Write();
            Console.ReadLine();
            break;
        case 3:
            RemoveIncident(incidents);
            break;
        case 0: 
            isRunning = false;
            break;
        default: 
            Write("ERROR: UNKNOWN TERMINAL VARIANT");
            break;
    }

}