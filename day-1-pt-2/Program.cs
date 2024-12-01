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

var frequencyDict = listTwo.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
/*foreach (KeyValuePair<int, int> author in frequencyDict)
{
    Console.WriteLine("Key: {0}, Value: {1}",author.Key, author.Value);
}*/

var similarityScores = new List<int>();
foreach (var number in listOne)
{
  int frequency;
  try 
  {
    frequency = frequencyDict[number];
  }
  catch (Exception e)
  {
    frequency = 0;
  }
  //Console.WriteLine("Num: " + number + " - " + frequency);
  int similarityScore = number * frequency;
  //Console.WriteLine("Num: " + number + " - " + similarityScore);
  similarityScores.Add(similarityScore);
}

int totalsimilarityScore = similarityScores.Aggregate(0, (acc, x) => acc + x);
Console.WriteLine("Total Similarity Score: " + totalsimilarityScore);
