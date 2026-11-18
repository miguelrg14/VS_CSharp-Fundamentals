using Fundamentals.Exercises.Basics;
using Fundamentals.Exercises.ControlFlow;
using Fundamentals.Exercises.DataStructureDemos;
using Fundamentals.Exercises.Files;
using Fundamentals.Exercises.Methods;
using Fundamentals.Exercises.MiniApps;
using Fundamentals.Exercises.StringsAndArrays;

namespace Fundamentals;

/// <summary>Every exercise in the app, grouped in the order they were studied.</summary>
public static class ExerciseCatalog
{
    public static IReadOnlyList<ExerciseCategory> Categories { get; } =
    [
        new("Basics",
        [
            new VariablesAndOutput(),
            new Operators(),
            new CastingAndConversion(),
        ]),
        new("Control flow",
        [
            new BooleanLogic(),
            new GameMenu(),
            new MultiplicationTable(),
            new GuessTheNumber(),
            new TimeNormalizer(),
            new LeapYear(),
            new NumberToWords(),
            new HollowRectangle(),
            new IncreasingSeries(),
            new PrizeDoors(),
            new PopulationStats(),
            new ReverseTwoDigits(),
        ]),
        new("Methods & recursion",
        [
            new SquareAndPower(),
            new RecursionBasics(),
            new Fibonacci(),
            new RefAndOutParameters(),
        ]),
        new("Strings & arrays",
        [
            new StringToolkit(),
            new ArrayBasics(),
            new RandomArrayStats(),
            new AlphabetBuilder(),
            new PalindromeNumber(),
        ]),
        new("Files",
        [
            new TextJournal(),
            new BubbleSortWithFile(),
        ]),
        new("Mini apps",
        [
            new Calculator(),
            new AddressBook(),
            new VideoGameStore(),
        ]),
        new("Data structures",
        [
            new StackPlayground(),
            new PriorityQueueDemo(),
            new SortedListDemo(),
            new DoublyLinkedListDemo(),
            new CircularNamesList(),
        ]),
    ];
}
