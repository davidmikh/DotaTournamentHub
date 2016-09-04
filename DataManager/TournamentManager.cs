using CoreDataModels;
using DataAccess;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager
{
    public class TournamentManager
    {
        private DotaAPIAccessor accessor;
        private string[] heroes;
        private string[] items;

        public TournamentManager()
        {
            accessor = new DotaAPIAccessor();
            //Try to get this info from cache first, then get it from API and save to cache if that fails
            heroes = accessor.GetHeroes().ToArray();
            items = accessor.GetItems().ToArray();
        }

        public IEnumerable<Match> GetLiveTournamentGames()
        {
            var jsonMatches = accessor.GetAllLiveTournamentGames();
            //This filters out all unofficial teams in games like FACEIT Leagues
            jsonMatches = jsonMatches.Where(t => t.Radiant.Name != null && t.Dire.Name != null);
            List<LiveMatch> matches = new List<LiveMatch>();
            //Convert the mathes from their Json form to a more useful object
            foreach (var jsonMatch in jsonMatches)
            {
                var radiant = accessor.GetTeamInfo(jsonMatch.Radiant.ID);
                var dire = accessor.GetTeamInfo(jsonMatch.Dire.ID);
                if (radiant == null || dire == null)
                {
                    //At least one of the teams isn't an official valve registered team meaning this can't be a tournament game
                    continue;
                }
                List<ProPlayer> radiantAccounts = new List<ProPlayer>();
                List<ProPlayer> direAccounts = new List<ProPlayer>();
                List<Player> radiantPlayers = new List<Player>();
                List<Player> direPlayers = new List<Player>();
                foreach (var id in radiant.PlayerIDs)
                {
                    radiantAccounts.Add(convertJsonAccountToProPlayer(accessor.GetAccountInfo(id)));
                }
                foreach (var id in dire.PlayerIDs)
                {
                    direAccounts.Add(convertJsonAccountToProPlayer(accessor.GetAccountInfo(id)));
                }
                foreach (var player in jsonMatch.ScoreBoard.Radiant.Players)
                {
                    radiantPlayers.Add(convertJsonPlayerToPlayer(player));
                }
                foreach (var player in jsonMatch.ScoreBoard.Dire.Players)
                {
                    direPlayers.Add(convertJsonPlayerToPlayer(player));
                }
                matches.Add(new LiveMatch
                {
                    ID = jsonMatch.ID,
                    LeagueID = jsonMatch.LeagueID,
                    RadiantSeriesWins = jsonMatch.RadiantSeriesWins,
                    DireSeriesWins = jsonMatch.DireSeriesWins,
                    Duration = new TimeSpan(0, 0, jsonMatch.ScoreBoard.Duration),
                    GameNumber = jsonMatch.GameNumber,
                    SeriesType = jsonMatch.SeriesType,
                    SeriesID = jsonMatch.SeriesID,
                    Radiant = convertJsonTeamToTeam(jsonMatch.ScoreBoard.Radiant, radiant, radiantPlayers, radiantAccounts),
                    Dire = convertJsonTeamToTeam(jsonMatch.ScoreBoard.Dire, dire, direPlayers, direAccounts),
                });
            }

            return matches;
        }

        private ProPlayer convertJsonAccountToProPlayer(JsonAccount account)
        {
            return new ProPlayer
            {
                ID = account.ID,
                Name = account.Name,
                RealName = account.RealName,
                CountryCode = account.CountryCode,
                Image = account.Image,
                URL = account.URL
            };
        }

        private Player convertJsonPlayerToPlayer(JsonPlayer player)
        {
            List<string> playerItems = new List<string>();
            foreach (var item in player.Items)
            {
                playerItems.Add(items[item]);
            }
            return new Player
            {
                AccountID = player.AccountID,
                Assists = player.Assists,
                CurrentGold = player.Gold,
                Deaths = player.Deaths,
                Denies = player.Denies,
                GPM = player.GPM,
                Hero = heroes[player.HeroID],
                Items = playerItems.ToArray(),
                Kills = player.Kills,
                LastHits = player.LastHits,
                Level = player.Level,
                NetWorth = player.NetWorth,
                PositionX = player.PositionX,
                PositionY = player.PositionY,
                RespawnTime = player.RespawnTimer,
                Slot = player.Slot,
                UltimateCD = player.UltimateCD,
                UltimateState = player.UltimateState,
                XPM = player.XPM,
            };
        }

        private Team convertJsonTeamToTeam(JsonTeam team, JsonTeamProfile teamProfile, IEnumerable<Player> players, IEnumerable<ProPlayer> accounts)
        {
            if (team.HeroPicks == null)
            {
                team.HeroPicks = new int[0];
            }
            if (team.HeroBans == null)
            {
                team.HeroBans = new int[0];
            }

            return new Team
            {
                Kills = team.Score,
                //TODO: Convert to actually useful form of barracks state
                BarracksState = team.BarracksState,
                //TODO: Convert to actually useful form of tower state
                TowerState = team.TowerState,
                Picks = team.HeroPicks.Select(t => heroes[t]).ToArray(),
                Bans = team.HeroBans.Select(t => heroes[t]).ToArray(),
                Players = players,
                OfficialTeam = new OfficialTeam
                {
                    ID = teamProfile.ID,
                    Name = teamProfile.Name,
                    LogoURL = accessor.GetImageURL(teamProfile.LogoID),
                    CaptainID = teamProfile.CaptainID,
                    Players = accounts
                }
            };
        }
    }
}
