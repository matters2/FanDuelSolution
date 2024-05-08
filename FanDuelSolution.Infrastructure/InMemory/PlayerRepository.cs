using FanDuelSolution.Application.Interfaces;
using FanDuelSolution.Application.NFL.Models;
using System.Text;

namespace FanDuelSolution.Infrastructure.InMemory;

public class PlayerRepository : IPlayerRepository
{
    private readonly Dictionary<string, List<Player>> _depthChartNFL;

    public PlayerRepository()
    {
        _depthChartNFL = new();
    }

    public IReadOnlyDictionary<string, List<Player>> DepthChartNFL => _depthChartNFL;

    public string GetFullDepthChart()
    {
        StringBuilder sb = new StringBuilder();

        var allPlayersAllPositions = _depthChartNFL.Values?.SelectMany(p => p).ToList();

        if (allPlayersAllPositions != null && allPlayersAllPositions.Count == 0)
        {
            var emptyResultMessage = "No results included in NFL Depth Chart";
            
            //For demo purposes only
            Console.WriteLine(emptyResultMessage);

            sb.Append(emptyResultMessage);

            return sb.ToString();
        }

        sb.Append("Position\t");

        //Get the largest list of players, for a given position
        var maxListSize = 0;

        foreach (var listPlayers in _depthChartNFL.Values)
        {
            var listSize = listPlayers.Count;

            if (listSize > maxListSize)
            {
                maxListSize = listSize;
            }
        }

        //Add depth chart headers
        for (int i = 0; i < maxListSize; i++)
        {
            sb.Append($"No.\tPlayer {i + 1}\t");
        }

        sb.Append("\n");

        //Add player details for each position 
        foreach (var kvp in _depthChartNFL)
        {
            sb.Append($"{kvp.Key}\t\t");
            
            foreach(var player in kvp.Value)
            {
                sb.Append($"{player.Number}\t{player.Name}\t");
            };

            sb.Append("\n");
        }

        //For demo purposes only, returned by the API
        Console.WriteLine(sb.ToString());
        
        return sb.ToString();
    }

    public List<Player> GetBackups(Player player)
    {
        var position = player.Position;

        if (string.IsNullOrEmpty(position) || !_depthChartNFL.ContainsKey(position))
        {
            throw new Exception($"Position backsups do not exist for given Position: {position}");
        }

        var playersList = _depthChartNFL[player.Position];
          
        var playerForBackups = playersList.FirstOrDefault(p => p.Number == player.Number);

        //scenario 1 - no back ups available
        if (playerForBackups == null)
        {
            return new();
        }

        var playerPositionIndex = playersList.FindIndex(p => p.Number == player.Number);

        var playerBackupsList = new List<Player>();

        //scenario 2 - players with lower depth exist 
        if (playersList.Count > playerPositionIndex + 1)
        {
            playerBackupsList = playersList.GetRange(playerPositionIndex + 1, playersList.Count - (playerPositionIndex + 1));
        };

        return playerBackupsList;
    }

    public void AddPlayerToDepthChart(Player player, int? positionDepth)
    {
        var position = player.Position;

        //scenario 1 - position does not exist in dict, add
        if (!string.IsNullOrEmpty(position) && !_depthChartNFL.ContainsKey(position))
        {
            _depthChartNFL.Add(position, new List<Player>() { player });

            return;
        }

        var playersForPosition = _depthChartNFL[position];
        var playersForPositionDepthCount = playersForPosition.Count;

        //scenario 2 - player being added already exists for given position, primary key player number
        var playerExistsInPosition = playersForPosition.Any(p => p.Number == player.Number);

        if (playerExistsInPosition)
        {
            throw new Exception($"Player Number: {player.Number}, Player Name: {player.Name} already exists in Depth Chart For Position {position}");
        }

        //scenario 3 - position exists, no position depth provided or position depth less than 1 or position depth last, goes to the end
        if (!positionDepth.HasValue ||
            positionDepth.HasValue && positionDepth <= 0 ||
            positionDepth.HasValue && positionDepth > playersForPositionDepthCount + 1 ||
            positionDepth.HasValue && positionDepth >= playersForPositionDepthCount)
        {
            playersForPosition.Add(player);
        }
        else if (positionDepth <= playersForPositionDepthCount) //scenario 4 - position depth provided, insert 
        {
            playersForPosition.Insert(positionDepth.Value - 1, player);
        }
        else // scenario 5 - invalid
        {
            throw new Exception($"Unable to add Player Number: {player.Number}, Player Name: {player.Name}");
        }

        _depthChartNFL[position] = playersForPosition;

        return;
    }

    public List<Player> RemovePlayerFromDepthChart(Player player)
    {
        var position = player.Position;

        if (string.IsNullOrEmpty(position) || !_depthChartNFL.ContainsKey(position))
        {
            throw new Exception($"Position: {position} for player removal does not exist in the depth chart");
        }

        var playersList = _depthChartNFL[position];

        //Per question, player can be identified from player number, primary key
        var playerForRemoval = playersList.FirstOrDefault(p => p.Number == player.Number);

        if (playerForRemoval == null)
        {
            return new();
        }

        playersList.Remove(playerForRemoval);

        _depthChartNFL[position] = playersList;

        return new List<Player> { player };
    }
}