using CoreDataModels;
using DataAccess;
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

        public TournamentManager()
        {
            accessor = new DotaAPIAccessor();
        }

        public IEnumerable<Match> GetLiveTournamentGames()
        {
            var jsonMatches = accessor.GetAllLiveTournamentGames();
            //This filters out all unofficial teams in games like FACEIT Leagues
            jsonMatches = jsonMatches.Where(t => t.Radiant != null && t.Dire != null);
            List<LiveMatch> matches = new List<LiveMatch>();
            //Convert the mathes from their Json form to a more useful object
            foreach (var jsonMatch in jsonMatches)
            {
                var radiant = accessor.GetTeamInfo(jsonMatch.Radiant.ID);
                var dire = accessor.GetTeamInfo(jsonMatch.Dire.ID);
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
                    Radiant = new Team
                    {
                        Kills = jsonMatch.ScoreBoard.Radiant.Score,
                        BarracksState = jsonMatch.ScoreBoard.Radiant.BarracksState,
                        TowerState = jsonMatch.ScoreBoard.Radiant.TowerState,
                        //TODO: Picks
                        //TODO: Bans
                        OfficialTeam = new OfficialTeam
                        {
                            ID = radiant.ID,
                            Name = radiant.Name,
                            LogoURL = accessor.GetImageURL(radiant.LogoID),
                            CaptainID = radiant.CaptainID,
                            //TODO: Players
                        }
                    },
                    Dire = new Team
                    {
                        Kills = jsonMatch.ScoreBoard.Dire.Score,
                        BarracksState = jsonMatch.ScoreBoard.Dire.BarracksState,
                        TowerState = jsonMatch.ScoreBoard.Dire.TowerState,
                        //TODO: Picks
                        //TODO: Bans
                        OfficialTeam = new OfficialTeam
                        {
                            ID = dire.ID,
                            Name = dire.Name,
                            LogoURL = accessor.GetImageURL(dire.LogoID),
                            CaptainID = dire.CaptainID,
                            //TODO: Players
                        }
                    },
                });
            }

            return matches;
        }
    }
}
