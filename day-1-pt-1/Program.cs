var listOne = new List<int>();
var listTwo = new List<int>();
int lineCount = 0;

try
{
    using StreamReader reader = new("input.txt");

    string text = reader.ReadToEnd();

    string[] lines = text.Split(
      new string[] { Environment.NewLine },
      StringSplitOptions.None
    );
    lineCount = lines.Count();

    foreach (var line in lines)
    {
        string[] words = System.Text.RegularExpressions.Regex.Split( line, @"\s{2,}");
        listOne.Add(Int32.Parse(words[0]));
        listTwo.Add(Int32.Parse(words[1]));
    }
}
catch (IOException e)
{
    Console.WriteLine("The file could not be read:");
    Console.WriteLine(e.Message);
}

listOne.Sort();
listTwo.Sort();

var distances = new List<int>();
for (int i = 0; i < lineCount; i++) 
{
  Console.WriteLine(listOne[i] + " - " + listTwo[i]);
  int distance = Math.Abs(listOne[i]-listTwo[i]);
  Console.WriteLine("Distance: " + distance);
  distances.Add(distance);
}
int totalDistance = distances.Aggregate(0, (acc, x) => acc + x);
Console.WriteLine("Total distance: " + totalDistance);