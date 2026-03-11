// See https://aka.ms/new-console-template for more information

static void Write(string line) {
    Console.WriteLine(line);
}

Write("What`s your name?");
string name = Console.ReadLine();
if (string.isNullOrWhiteSpace(name)) {
    Write("WARNING: Operator name missing");
}
Write("How old are you?");
int age;
int.TryParse(Console.ReadLine(), out age);
Write("What`s your access level");
int accessLevel;
int.TryParse(Console.ReadLine(), out accessLevel);

Write("=== ACCESS REPORT ===");
Write($"Operator: {name}");
Write($"Age: {age}");
Write($"Clearance Level: {accessLevel}");
Write("");
if (age < 16) {
    Write("ACCESS DENIED: Age restriction");
    Write("Zone: Entrance");
} else {
    switch (accessLevel) {
        case 1: 
            Write("ACCESS Granted");
            Write("Zone: Public Zone");
            break;
        case 2: 
            Write("ACCESS Granted");
            Write("Zone: Public Zone");
            break;
        case 3:
            Write("ACCESS Granted");
            Write("Zone: Research Zone");
            break;
        case 4:
            Write("ACCESS Granted");
            Write("Zone: Research Zone");
            break;
        case 5:
            Write("ACCESS Granted");
            Write("Zone: Biohazard Core");
            break;
        default: 
            Write("ACCESS DENIED: Invalid clearance level");
            break;
    }
}




