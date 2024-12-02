﻿using System.Linq;

string[] lines = [];
int lineCount = 0;

try
{
  using StreamReader reader = new("input.txt");
  string text = reader.ReadToEnd();

  lines = text.Split(
    new string[] { Environment.NewLine },
    StringSplitOptions.None
  );
}
catch (IOException e)
{
  Console.WriteLine("The file could not be read:");
  Console.WriteLine(e.Message);
}

lineCount = lines.Count();

List<List<int>> reports = new List<List<int>>();
foreach (var line in lines)
{
  string[] levelStrings = line.Split(' ');
  List<int> levels = new List<int>();
  foreach (var item in levelStrings)
  {
    levels.Add(Convert.ToInt32(item));
  }
  reports.Add(levels);
}

static List<int> Difference(List<int> inputList)
{
  return inputList.Skip(1).Select((n, i) => n - inputList[i]).ToList();
}

static bool IsAllPositiveOrNegative(List<int> inputList)
{
  bool allPositive = inputList.All(n => n >= 0);
  bool allNegative = inputList.All(n => n <= 0);
  return allPositive || allNegative;
}

static bool IsAbsoluteValueBetweenOneAndThree(List<int> inputList)
{
  return inputList.All(n => Math.Abs(n) >= 1 && Math.Abs(n) <= 3);
}

static List<int> RemoveItemByLocation(List<int> inputList, int itemLocation)
{
  return inputList.Where((n, i) => i != itemLocation).ToList();
}

static bool CheckIfListIsSafe(List<int> inputList)
{
  List<int> levelAnalysis = Difference(inputList);
  //Console.WriteLine("Differences: " + string.Join(",", levelAnalysis.Select(n => n.ToString()).ToArray()));
  bool allInOrDecreasing = IsAllPositiveOrNegative(levelAnalysis);
  bool allWithinRange = IsAbsoluteValueBetweenOneAndThree(levelAnalysis);
  return allInOrDecreasing && allWithinRange;
}

int safeReportsCount = 0;
foreach (var report in reports)
{
  //Console.WriteLine("\nReport: " + string.Join(",", report.Select(n => n.ToString()).ToArray()));
  int currentLevel = 0;
  bool reportIsSafe = false;
  do
  {
    var subList = RemoveItemByLocation(report, currentLevel);
    //Console.WriteLine("Attempt " + currentLevel + ": " + string.Join(",", subList.Select(n => n.ToString()).ToArray()));
    reportIsSafe = CheckIfListIsSafe(subList);
    currentLevel++;
  } while (currentLevel <= report.Count() && !reportIsSafe);

  if (reportIsSafe) {
    safeReportsCount++;
  }
}
Console.WriteLine("Safe reports: " + safeReportsCount);