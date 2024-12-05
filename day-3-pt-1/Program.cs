using System;
using System.Text.RegularExpressions;

string text = "";

try
{
  using StreamReader reader = new("input.txt");
  text = reader.ReadToEnd();
}
catch (IOException e)
{
  Console.WriteLine("The file could not be read:");
  Console.WriteLine(e.Message);
}

Console.WriteLine(text);

string pattern = @"mul\(\d+,\d+\)";
Regex rg = new Regex(pattern);
MatchCollection matchedFunctions = rg.Matches(text);
string[] funcArray = matchedFunctions
  .Cast<Match>()
  .Select(m => m.Value)
  .ToArray();

int totalSum = 0;
foreach (string fun in funcArray) 
{
  Console.WriteLine(fun);
  Regex rgDigits = new Regex(@"\d+");
  int[] digits = rgDigits.Matches(fun)
    .Cast<Match>()
    .Select(m => m.Value)
    .Select(m => Convert.ToInt32(m))
    .ToArray();

  int sum = digits[0] * digits[1];
  totalSum = totalSum + sum;
}

Console.WriteLine(totalSum);
