﻿using CoreDataModels;
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
        private string[] heroes;

        public TournamentManager()
        {
            accessor = new DotaAPIAccessor();
            //Try to get this info from cache first, then get it from API and save to cache if that fails
            heroes = accessor.GetHeroes().ToArray();
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
                        Picks = jsonMatch.ScoreBoard.Radiant.HeroPicks.Select(t => heroes[t]).ToArray(),
                        Bans = jsonMatch.ScoreBoard.Radiant.HeroBans.Select(t => heroes[t]).ToArray(),
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
                        Picks = jsonMatch.ScoreBoard.Dire.HeroPicks.Select(t => heroes[t]).ToArray(),
                        Bans = jsonMatch.ScoreBoard.Dire.HeroBans.Select(t => heroes[t]).ToArray(),
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
