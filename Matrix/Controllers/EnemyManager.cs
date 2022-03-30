﻿using Matrix.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NLog;
using System;
using System.Collections.Generic;

namespace Matrix
{
    static class EnemyManager
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        static Texture2D _bulletOrange;
        static Texture2D _BulletBomb;
        static Texture2D _BulletBomb2;
        static Texture2D _enemyButterfly;
        static Texture2D _enemyMidBoss;
        static Texture2D _enemyFinalBoss;
        static Random _random = new Random();
        static List<Spawner> enemiesPhase1 = new List<Spawner>();
        static List<Spawner> enemiesPhase2 = new List<Spawner>();
        static List<Spawner> enemiesPhase3 = new List<Spawner>();
        static List<Spawner> enemiesPhase4 = new List<Spawner>();
        static EnemyFactory enemyFactory = new EnemyFactory();

        public static Bullet Bullet { get; set; }

        static EnemyManager()
        {
           
            _enemyMidBoss = Arts.Boss2;
            _enemyFinalBoss = Arts.Boss;
            _bulletOrange = Arts.BulletOrange;
            _BulletBomb = Arts.Bomb;
            _BulletBomb2 = Arts.Bomb2;
        }

        public static Enemy GetEnemy(Texture2D texture, float x, float y)
        {
            var e = new Enemy(texture);
            string name = texture.Name.ToLower();

            {
                if (name.Contains("boss2"))
                    Bullet = new Bullet(_BulletBomb);
                else if (name.Contains("boss"))
                    Bullet = new Bullet(_BulletBomb2);
            }

            e.Bullet = Bullet;
            e.Position = new Vector2(x, y);
            e.Speed = 2 + (float)_random.NextDouble();
            e.TimerStart = 1.5f + (float)_random.NextDouble();
            e.LifeSpan = 5;

            if (name == "boss")
            {//finalboss
                e.LifeSpan = 5;
                e.Health = 15;
            }
            else if (name == "boss2")
            { //midboss
                e.LifeSpan = 5;
                e.Health = 10;
            }

            _logger.Info("Build enemy: " + e.Name);

            return e;
        }

        public static Enemy GetEnemy(Enemy.Type type)
        {
            Enemy enemy = null;
            Texture2D texture = null;

            float xFactor = 0;
            float yFactor = 0;

            //Set initial starting x,y
            if (type == Enemy.Type.Boss || type == Enemy.Type.FinalBoss)
            {
                xFactor = -40;
                yFactor = 80;
            }
            
            if (type == Enemy.Type.FinalBoss)
            {
                texture = _enemyFinalBoss;
                enemy = GetEnemy(texture, xFactor, yFactor);
            }
            else if (type == Enemy.Type.Boss)
            {
                texture = _enemyMidBoss;
                enemy = GetEnemy(texture, xFactor, yFactor);
            }
            else if (type == Enemy.Type.ButterFlyEnemies)
            {
                enemy = (Enemy)enemyFactory.Create("butterflyenemy", Enemy.Type.ButterFlyEnemies);
            }
            else if (type == Enemy.Type.BasicEnemies)
            {
                enemy = (Enemy)enemyFactory.Create("basicEnemy", Enemy.Type.BasicEnemies);
            }
                       
            return enemy;
        }

        #region Phases of game - spawing enemies
        public static IEnumerable<Sprite> GetEnemyPhase1(GameTime gameTime)
        {
            if (enemiesPhase1.Count == 0)
            {
                for (int i = 1; i < 15; i++)
                {
                    enemiesPhase1.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = i });
                }
                enemiesPhase1.Add(new Spawner() { EnemyType = Enemy.Type.ButterFlyEnemies, SpawnSeconds = 12 });
                enemiesPhase1.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 15 });
                enemiesPhase1.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 16 });
                enemiesPhase1.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 17 });
                enemiesPhase1.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 18 });
                enemiesPhase1.Add(new Spawner() { EnemyType = Enemy.Type.ButterFlyEnemies, SpawnSeconds = 20 });
                enemiesPhase1.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 22 });
                enemiesPhase1.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 23 });
                enemiesPhase1.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 25 });
                enemiesPhase1.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 26 });
                enemiesPhase1.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 27 });
                enemiesPhase1.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 28 });
                enemiesPhase1.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 29 });
                enemiesPhase1.Add(new Spawner() { EnemyType = Enemy.Type.ButterFlyEnemies, SpawnSeconds = 32 });
                enemiesPhase1.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 34 });
                enemiesPhase1.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 35 });
                enemiesPhase1.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 36 });
            }

            return LoadEnemiesIntoPhase(gameTime, enemiesPhase1);
        }

        public static IEnumerable<Sprite> GetEnemyPhase2(GameTime gameTime)
        {
            if (enemiesPhase2.Count == 0)
            {
                enemiesPhase2.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 40 });
                enemiesPhase2.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 42 });
                enemiesPhase2.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 43 });
                enemiesPhase2.Add(new Spawner() { EnemyType = Enemy.Type.Boss, SpawnSeconds = 48 });
                enemiesPhase2.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 52 });
                enemiesPhase2.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 54 });
                enemiesPhase2.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 55 });
                enemiesPhase2.Add(new Spawner() { EnemyType = Enemy.Type.Boss, SpawnSeconds = 60 });
                enemiesPhase2.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 65 });
                enemiesPhase2.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 66 });
                enemiesPhase2.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 67 });
                enemiesPhase2.Add(new Spawner() { EnemyType = Enemy.Type.Boss, SpawnSeconds = 72 });
                enemiesPhase2.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 76 });
                enemiesPhase2.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 78 });
                enemiesPhase2.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 79 });
            }

            return LoadEnemiesIntoPhase(gameTime, enemiesPhase2);
        }    

        public static IEnumerable<Sprite> GetEnemyPhase3(GameTime gameTime)
        {
            if (enemiesPhase3.Count == 0)
            {
                enemiesPhase3.Add(new Spawner() { EnemyType = Enemy.Type.FinalBoss, SpawnSeconds = 81 });
                enemiesPhase3.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 85 });
                enemiesPhase3.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 86 });
                enemiesPhase3.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 90 });
                enemiesPhase3.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 94 });
                enemiesPhase3.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 97 });
                enemiesPhase3.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 101 });
                enemiesPhase3.Add(new Spawner() { EnemyType = Enemy.Type.FinalBoss, SpawnSeconds = 106 });
                enemiesPhase3.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 109 });
                enemiesPhase3.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 115});
                enemiesPhase3.Add(new Spawner() { EnemyType = Enemy.Type.BasicEnemies, SpawnSeconds = 116});
            }

            return LoadEnemiesIntoPhase(gameTime, enemiesPhase3);
        }

        public static IEnumerable<Sprite> GetEnemyPhase4(GameTime gameTime)
        {
            if (enemiesPhase4.Count == 0)
            {
                enemiesPhase4.Add(new Spawner() { EnemyType = Enemy.Type.FinalBoss, SpawnSeconds = 121 });
                enemiesPhase4.Add(new Spawner() { EnemyType = Enemy.Type.FinalBoss, SpawnSeconds = 131});
                enemiesPhase4.Add(new Spawner() { EnemyType = Enemy.Type.FinalBoss, SpawnSeconds = 141});
                enemiesPhase4.Add(new Spawner() { EnemyType = Enemy.Type.FinalBoss, SpawnSeconds = 151});
                enemiesPhase4.Add(new Spawner() { EnemyType = Enemy.Type.FinalBoss, SpawnSeconds = 161});
            }

            return LoadEnemiesIntoPhase(gameTime, enemiesPhase4);
        }

        private static IEnumerable<Sprite> LoadEnemiesIntoPhase(GameTime gameTime, List<Spawner> spawnedEnemies)
        {
            List<Sprite> enemies = new List<Sprite>();

            //Load enemies each second
            double gameSecond = 0;
            double returnValue = Math.Round(gameTime.TotalGameTime.TotalSeconds, 2) % 1;

            if (returnValue == 0)
            {
                gameSecond = Math.Round(gameTime.TotalGameTime.TotalSeconds, 0);

                //Check if a certain enemy should be sent to view based on time
                Spawner spawner = spawnedEnemies.Find(e => e.SpawnSeconds == gameSecond);
                if (spawner != null)
                    enemies.Add(GetEnemy(spawner.EnemyType));
            }

            return enemies;
        }

        #endregion
    }

}
