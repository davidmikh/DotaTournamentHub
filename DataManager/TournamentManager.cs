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
                if (radiant == null || dire == null)
                {
                    //At least one of the teams isn't an official valve registered team meaning this can't be a tournament game
                    continue;
                }
                List<ProPlayer> radiantAccounts = new List<ProPlayer>();
                List<ProPlayer> direAccounts = new List<ProPlayer>();
                List<Player> radiantPlayers = new List<Player>();
                List<Player> direPlayers = new List<Player>();
                //TODO: Bring this back to before where players & accounts were split. New version isn't as ideal for live games
                //This can be null when the game is live but still in the drafting stage and no hero has been picked
                if (jsonMatch.ScoreBoard.Radiant != null)
                {
                    foreach (var player in jsonMatch.ScoreBoard.Radiant.Players)
                    {
                        radiantAccounts.Add(convertJsonAccountToProPlayer(accessor.GetAccountInfo(player.AccountID)));
                        radiantPlayers.Add(convertJsonPlayerToPlayer(player));
                    }
                }
                if (jsonMatch.ScoreBoard.Dire != null)
                {
                    foreach (var player in jsonMatch.ScoreBoard.Dire.Players)
                    {
                        direAccounts.Add(convertJsonAccountToProPlayer(accessor.GetAccountInfo(player.AccountID)));
                        direPlayers.Add(convertJsonPlayerToPlayer(player));
                    }
                }
                var tournament = tournaments.Where(t => t.ID == jsonMatch.LeagueID).First();
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
            var matchIDs = accessor.GetMatchIDsForTournament(tournamentID);
            List<PastMatch> matches = new List<PastMatch>();
            var tournament = tournaments.Where(t => t.ID == tournamentID).First();
            if (tournament == null)
            {
                return null;
            }
            foreach (var matchID in matchIDs)
            {
                var jsonMatch = accessor.GetMatch(matchID);
                List<ProPlayer> radiantAccounts = new List<ProPlayer>();
                List<ProPlayer> direAccounts = new List<ProPlayer>();
                List<Player> radiantPlayers = new List<Player>();
                List<Player> direPlayers = new List<Player>();
                //The player slot is an 8-bit unsigned integer where the first bit represents the player team - 1 if Dire, 0 if Radiant
                foreach (var player in jsonMatch.Players.Where(t => t.Slot < 128))
                {
                    radiantAccounts.Add(convertJsonAccountToProPlayer(accessor.GetAccountInfo(player.AccountID)));
                    radiantPlayers.Add(convertJsonPlayerToPlayer(player));
                }
                foreach (var player in jsonMatch.Players.Where(t => t.Slot >= 128))
                {
                    direAccounts.Add(convertJsonAccountToProPlayer(accessor.GetAccountInfo(player.AccountID)));
                    radiantPlayers.Add(convertJsonPlayerToPlayer(player));
                }
                string[] radiantPicks = new string[0];
                string[] radiantBans = new string[0];
                string[] direPicks = new string[0];
                string[] direBans = new string[0];
                //This means the match was in captains mode so there are picks/bans
                if (jsonMatch.GameMode == 2)
                {
                    radiantPicks = jsonMatch.RadiantPicks.Select(t => heroes[t]).ToArray();
                    radiantBans = jsonMatch.RadiantBans.Select(t => heroes[t]).ToArray();
                    direPicks = jsonMatch.DirePicks.Select(t => heroes[t]).ToArray();
                    direBans = jsonMatch.DireBans.Select(t => heroes[t]).ToArray();
                }
                //TODO: Exception happens here in adding matches. Its one of the commented out lines
                matches.Add(new PastMatch
                {
                    ID = jsonMatch.ID,
                    Tournament = new Tournament
                    {
                        ID = tournamentID,
                        Name = tournament.Name.Replace("_", " "),
                        TicketItemID = tournament.TicketItemID,
                    },
                    GameType = jsonMatch.GameMode,
                    IsSourceTwo = Convert.ToBoolean(jsonMatch.Engine),
                    ServerClusterNum = jsonMatch.ServerClusterNum,
                    RadiantVictory = jsonMatch.RadiantWin,
                    StartTime = jsonMatch.StartTime,
                    Duration = jsonMatch.Duration,
                    Radiant = new Team
                    {
                        Kills = jsonMatch.RadiantKills,
                        BarracksState = jsonMatch.RadiantBarracksState,
                        TowerState = jsonMatch.RadiantTowerState,
                        Picks = radiantPicks,
                        Bans = radiantBans,
                        Players = radiantPlayers,
                        OfficialTeam = new OfficialTeam
                        {
                            CaptainID = jsonMatch.RadiantCaptainID,
                            ID = jsonMatch.RadiantTeamID,
                            LogoURL = accessor.GetImageURL(jsonMatch.RadiantLogoID),
                            Name = jsonMatch.RadiantName,
                            Players = radiantAccounts,
                        }
                    },
                    Dire = new Team
                    {
                        Kills = jsonMatch.DireKills,
                        BarracksState = jsonMatch.DireBarracksState,
                        TowerState = jsonMatch.DireTowerState,
                        Picks = direPicks,
                        Bans = direBans,
                        Players = direPlayers,
                        OfficialTeam = new OfficialTeam
                        {
                            CaptainID = jsonMatch.DireCaptainID,
                            ID = jsonMatch.DireTeamID,
                            LogoURL = accessor.GetImageURL(jsonMatch.DireLogoID),
                            Name = jsonMatch.DireName,
                            Players = direAccounts,
                        }
                    }
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
            if (team == null)
            {
                //This occurs when the game is in the drafting stage & no heroes have been picked
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
