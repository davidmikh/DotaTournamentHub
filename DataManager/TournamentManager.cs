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
        private IEnumerable<JsonTournament> tournaments;

        public TournamentManager()
        {
            accessor = new DotaAPIAccessor();
            //Try to get all this info from cache first, then get it from API and save to cache if that fails
            heroes = accessor.GetHeroes().ToArray();
            items = accessor.GetItems().ToArray();
            tournaments = accessor.GetAllTournaments();
        }

        public IEnumerable<LiveMatch> GetLiveTournamentGames()
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
                var tournament = tournaments.Where(t => t.ID == jsonMatch.LeagueID).SingleOrDefault();
                if (radiant == null || dire == null || tournament == null)
                {
                    //At least one of the teams isn't an official valve registered team meaning this can't be a tournament game
                    continue;
                }
                List<ProAccount> radiantAccounts = new List<ProAccount>();
                List<ProAccount> direAccounts = new List<ProAccount>();
                List<Player> radiantPlayers = new List<Player>();
                List<Player> direPlayers = new List<Player>();
                //This can be null when the game is live but still in the drafting stage and no hero has been picked
                if (jsonMatch.ScoreBoard.Radiant != null)
                {
                    foreach (var player in jsonMatch.ScoreBoard.Radiant.Players)
                    {
                        radiantAccounts.Add(convertJsonAccountToProAccount(accessor.GetAccountInfo(player.AccountID)));
                        radiantPlayers.Add(convertJsonPlayerToPlayer(player));
                    }
                }
                if (jsonMatch.ScoreBoard.Dire != null)
                {
                    foreach (var player in jsonMatch.ScoreBoard.Dire.Players)
                    {
                        direAccounts.Add(convertJsonAccountToProAccount(accessor.GetAccountInfo(player.AccountID)));
                        direPlayers.Add(convertJsonPlayerToPlayer(player));
                    }
                }
                matches.Add(new LiveMatch
                {
                    ID = jsonMatch.ID,
                    Tournament = new Tournament
                    {
                        ID = tournament.ID,
                        Name = tournament.Name.Replace("_", " "),
                        TicketItemID = tournament.TicketItemID,
                    },
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

        public IEnumerable<PastMatch> GetMatchesForTournament(long tournamentID)
        {
            var jsonMatches = accessor.GetMatchesForTournament(tournamentID, 50);
            List<PastMatch> matches = new List<PastMatch>();
            var tournament = tournaments.Where(t => t.ID == tournamentID).First();
            if (tournament == null)
            {
                return null;
            }
            foreach (var match in jsonMatches)
            {
                //TODO: Hopefully these teams will also be stored in cache so don't need to call API so many times since teams don't change by the minute. Look into it
                var jsonRadiant = accessor.GetTeamInfo(match.RadiantTeamID);
                var jsonDire = accessor.GetTeamInfo(match.DireTeamID);
                matches.Add(new PastMatch
                {
                    ID = match.ID,
                    StartTime = match.StartTime,
                    Tournament = new Tournament
                    {
                        ID = tournament.ID,
                        Name = tournament.Name,
                        TicketItemID = tournament.TicketItemID,
                    },
                    Radiant = convertJsonTeamToTeam(null, jsonRadiant, null, null),
                    Dire = convertJsonTeamToTeam(null, jsonDire, null, null),
                });
            }
            return matches;
        }

        private ProAccount convertJsonAccountToProAccount(JsonAccount account)
        {
            return new ProAccount
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
                //TODO: There is currently a bug with items where the ID can be too high. Gets as high as 265
                if (item <= items.Length)
                {
                    playerItems.Add(items[item]);
                }
            }
            if (player.HeroID >= heroes.Length)
            {
                //TODO: THere is currently a bug with heros where the ID can be too high. Gets as high as 114
                player.HeroID = 0;
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

        private Team convertJsonTeamToTeam(JsonTeam team, JsonTeamProfile teamProfile, IEnumerable<Player> players, IEnumerable<ProAccount> accounts)
        {
            //This occurs when the game is in the drafting stage & no heroes have been picked
            if (team == null)
            {
                if (teamProfile != null)
                {
                    return new Team
                    {
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
                else
                {
                    //This happens when a team has an official ID but for some reason doesn't have a Team Profile
                    return new Team
                    {
                        Players = players,
                        OfficialTeam = new OfficialTeam
                        {
                            ID = 0,
                            Name = "Unofficial Team",
                            LogoURL = accessor.GetImageURL(0),
                            CaptainID = 0,
                            Players = accounts
                        }
                    };
                }
            }
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
