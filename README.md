# C# Fundamentals

[![Build & Test](https://github.com/miguelrg14/VS_Console_Examples/actions/workflows/dotnet.yml/badge.svg)](https://github.com/miguelrg14/VS_Console_Examples/actions/workflows/dotnet.yml)

C# console projects from my first year of Computer Science & Video Game Engineering at ESNE (2020–2021),
refactored into a single .NET solution: one menu-driven app with 34 exercises, a reusable library of
generic data structures, three two-player board games and **Battle Tower**, a turn-based RPG.
All of it is covered by unit tests that run on every push.

## Projects

| Project | Type | What it shows |
| --- | --- | --- |
| [`src/BattleTower`](src/BattleTower) | Console game | Turn-based tower climb: inheritance, game rules kept apart from the UI, enemy AI, JSON save/load, high-score table |
| [`src/Games`](src/Games) | Console games | Tic-tac-toe, Connect Four (7×6) and Battleship (hot-seat, ship placement, hidden boards) |
| [`src/DataStructures`](src/DataStructures) | Class library | Generic `LinkedStack<T>`, `LinkedQueue<T>`, `SortedLinkedList<T>`, `DoublyLinkedList<T>`, `CircularLinkedList<T>`, `PriorityLevelQueue<T>` and bubble sort, all written from scratch |
| [`src/Fundamentals`](src/Fundamentals) | Console app | 34 exercises in 7 categories, launched from a menu |
| [`src/ConsoleUtils`](src/ConsoleUtils) | Class library | Validated console input (`ReadInt`, `ReadDouble`, `ReadYesNo`...) and numbered menus |
| [`tests/*`](tests) | xUnit | About 150 tests for the data structures, the exercise logic, the game rules and Battle Tower |

## Getting started

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```bash
git clone https://github.com/miguelrg14/VS_Console_Examples.git
cd VS_Console_Examples

dotnet run --project src/BattleTower    # the RPG
dotnet run --project src/Games          # tic-tac-toe, Connect Four, Battleship
dotnet run --project src/Fundamentals   # the exercise menu

dotnet test                             # run every test
```

Or open `CSharpFundamentals.sln` in Visual Studio / Rider and pick a startup project.

## Battle Tower

Choose how many enemies to face (1–10) and climb the tower fighting them one by one. Every character has
randomly rolled health, strength, defense, magic power, magic barrier and energy.

- **Attack / Magic missile**: damage is the attacker's power minus the target's defense (or magic barrier),
  with a minimum of 1. Every attack costs as much energy as the power it uses.
- **Rest**: recover energy. Enemies choose among the actions they can afford and rest when exhausted.
- **Analyze**: compare your stats with the current enemy's.
- **Save / Continue**: the whole run (player, current enemy, remaining enemies, score) is stored as JSON.
- **Scoring**: damage dealt + 50 per enemy + 500 for reaching the top. Victories go into a high-score table.

Design notes:

- `TowerRun` holds every rule and returns a log of what happened each turn; `Program` only prints it.
  That is what makes the game testable without a console.
- `Player` and `Enemy` share an abstract `Character` base class with the stats and the combat rules.
- Enemies wait in a `LinkedQueue<T>` and high scores are kept in a `SortedLinkedList<T>`, both from the
  `DataStructures` library.
- All randomness goes through `Dice`, which takes a `Random`; tests pass a seeded one so runs are reproducible.

## Exercises

| Category | Exercises |
| --- | --- |
| Basics | Variables and output · Arithmetic operators · Casting and conversion |
| Control flow | Boolean logic · Game menu · Multiplication table · Guess the number · Normalize a duration · Leap year · Number to words · Hollow rectangle · Increasing series · Prize doors · Population statistics · Reverse a two-digit number |
| Methods & recursion | Square and power · Countdown and factorial · Fibonacci · `ref` and `out` parameters |
| Strings & arrays | String toolkit (reverse, replace, Caesar cipher) · Array basics · Random array statistics · Alphabet builder · Palindrome number |
| Files | Text journal · Bubble sort with file I/O |
| Mini apps | Calculator with memory · Address book (export/import) · Video game store (classes, equality, operator overloading) |
| Data structures | Stack playground · Priority queue · Sorted list · Doubly linked list · Circular list of names |

Each exercise is a class implementing `IExercise`, registered in `ExerciseCatalog`. The logic worth
testing lives in public static methods, so the console part of each exercise stays thin.

## Code conventions

- Shared build settings in `Directory.Build.props`: .NET 10, nullable reference types, warnings as errors.
- Package versions managed centrally in `Directory.Packages.props`.
- User input is always validated (`TryParse` instead of `Convert.ToInt32`), files are opened with `using`,
  and missing or corrupt save files are reported instead of crashing.
- GitHub Actions builds the solution and runs the tests on every push and pull request.

## License

[MIT](LICENSE)
