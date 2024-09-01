namespace Super_GameBoy_Border_Renamer;

public class Games
{
    public List<Game> games { get; set; } = [];
}

public class Game
{
    public string game_name { get; set; }

    public List<string> file_names { get; set; } = [];
}
