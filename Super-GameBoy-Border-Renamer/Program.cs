
using Newtonsoft.Json;
using Super_GameBoy_Border_Renamer;

const string extension       = "sgb";
const string jsonFileName    = "Super-GameBoy-Border-Names.json";
const string renameLocation  = "/Users/michael/Projects/Super-GameBoy-Border-Renamer/Super-GameBoy-Borders/SGB";
const string renamedLocation = "/Users/michael/Projects/Super-GameBoy-Border-Renamer/Super-GameBoy-Borders/SGB-Renamed";

string jsonString = File.ReadAllText(jsonFileName);
Games list = JsonConvert.DeserializeObject<Games>(jsonString);
Dictionary<string, string> lookup = new Dictionary<string, string>();

foreach (Game game in list.games)
{
    foreach (string fileName in game.file_names)
    {
        lookup.Add(Path.GetFileNameWithoutExtension(fileName), game.game_name);
    }
}

string[] filesToRename = Directory.GetFiles(renameLocation, $"*.{extension}", SearchOption.AllDirectories)
                                  .OrderBy(f => f).ToArray();

foreach (string file in filesToRename)
{
    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);

    if (lookup.TryGetValue(fileNameWithoutExtension, out string gameName))
    {
        string[] gameNameParts = gameName.Split('/', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        string[] fileNameParts = fileNameWithoutExtension.Split('-');
        string newFileName;

        if (fileNameParts.Length > 1)
        {
            newFileName = gameNameParts[0] + $" ({fileNameParts[1]}).{extension}";
        }
        else
        {
            newFileName = gameNameParts[0] + $".{extension}";
        }

        if (newFileName.Contains(':'))
        {
            newFileName = newFileName.Replace(":", " -");
        }

        string destination = Path.Combine(renamedLocation, newFileName);

        Directory.CreateDirectory(renamedLocation);
        File.Copy(file, destination, true);
    }
    else
    {
        Console.WriteLine($"Not Found '{Path.GetFileName(file)}'");
    }
}
