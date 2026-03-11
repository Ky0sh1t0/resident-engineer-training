// See https://aka.ms/new-console-template for more information

static void Write(string line) {
    Console.WriteLine(line);
}


Write("What is your name?");
string name = Console.ReadLine();
Write("What is your age?");
int age;
int.TryParse(Console.ReadLine(), out age);
Write("What game you like the most?");
string favGame = Console.ReadLine();
Write("How long do you play a week?");
int hoursPerWeek;
int.TryParse(Console.ReadLine(), out hoursPerWeek);

Write("=== PASSPORT OF THE UNKINDLED ===");
Write($"Name: {name}");
Write($"Age: {age}");
Write($"Favorite game: {favGame}");
Write($"Hours per week: {hoursPerWeek}");
if (age >= 18) {
    Write("Rank: Adult Initiate");
} else {
    Write("Rank: Young Ash");
}

if (hoursPerWeek < 5) {
    Write("Playstyle: Casual Wanderer");
} else if (hoursPerWeek <= 15) {
    Write("Playstyle: Steady Fighter");
} else {
    Write("Playstyle: Relentless Grinder");
}

Write("");
Write("Status: Ready for the Engineer`s Path.");