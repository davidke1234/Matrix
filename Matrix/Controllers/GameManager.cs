﻿using Matrix.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Matrix.Controllers
{
    public static class GameManager
    {
        public static int GamePhase = 1;

        public static bool EnabledKillEnemiesCheat { get; internal set; }

        public static void LoadObservers()
        {
            Subject subject = new Subject("PlayerScore");
            PlayerManager.ObserverScoreSubject = subject;
            new Observer(subject);

            subject = new Subject("PlayerHealth");
            PlayerManager.ObserverHealthSubject = subject;
            new Observer(subject);
        }

        public static bool GoToNextPhase(double currentTotalGameSeconds, int phase)
        {
            if (phase == 2 && currentTotalGameSeconds >= 40)
            {
                GamePhase = 2;
                return true;
            }
            if (phase == 3 && currentTotalGameSeconds >= 80)
            {
                GamePhase = 3;
                return true;
            }
            if (phase == 4 && currentTotalGameSeconds >= 120)
            {
                GamePhase = 4;
                return true;
            }

            return false;
        }
    }
}
