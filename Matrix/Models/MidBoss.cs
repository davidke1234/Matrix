﻿using Matrix;
using Matrix.Models.Enums;
using Matrix.Models.Factories;
using Matrix.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Matrix.Models
{
    /// <summary>
    /// The Midboss class
    /// </summary>
    public class MidBoss : Enemy
    {
        private float _changePositionTimer = 5;
        private float _timerStart = 1.25f; // 5;
        private float _shootingTimer;
        public Bomb bomb;
        public List<Sprite> bombs = new List<Sprite>();
        //public new int Health;
        private static MidBoss MidBossInstance = null;
        private static ProjectileFactory _projectileFactory = new ProjectileFactory();
        private double _lifeSpanTimer;

        /// <summary>
        /// Returns a singleton instance of midboss
        /// </summary>
        /// <returns>An instance of the midboss</returns>
        public static MidBoss GetInstance
        {
            get
            {
                if (MidBossInstance == null)
                {
                    MidBossInstance = new MidBoss(Arts.MidBoss);
                }
                return MidBossInstance;
            }
        }

        /// <summary>
        /// Initializes an instance of <see cref="MidBoss"/> class.
        /// </summary>
        /// <param name="texture"></param>
        public MidBoss(Texture2D texture): base(texture)
        {
            //Position = new Vector2(Game1.Viewport.Width / 2, 50);
            Health = 40;
            Position.X = 70;
            Position.Y = 60;
            Bomb = (Bomb)_projectileFactory.Create(typeof(Bomb).Name, Arts.Bomb);
            //Bullet = (Bullet)_projectileFactory.Create("bullet");
            bombs.Add(bomb);

            LifeSpan = 38;
            Speed = 2.65f;
            Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);

            Colour = Color.White;
            TextureData = new Color[_texture.Width * _texture.Height];
            _texture.GetData(TextureData);
        }

        public void UpdateBombs()
        {
            if (bombs.Count() < 2)
            {
                bombs.Add((Bomb)bomb);
            }

            foreach (Bomb bomb in bombs.ToList())
            {
                bomb.Position += bomb.Velocity;
                if (bomb.Position.X < 0)
                {
                    bomb.IsRemoved = true;
                }

                for (int i = 0; i < bombs.Count; i++)
                {
                    if (!bombs[i].IsRemoved)
                    {
                        bombs.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        //public void ShootBombs(List<Sprite> sprites)
        //{            
        //    bomb.Velocity.Y = this.Velocity.Y + 6f;
        //    bomb.Position = this.Position;
        //    bomb.Direction = this.Direction;
        //    bomb.Parent = this;

        //    if (bombs.Count() < 3)
        //    {
        //        bombs.Add((Bomb)bomb);
        //    }
        //    if(!sprites.Contains(bomb))
        //    {
        //        sprites.Add(bomb);
        //    }
        //}        

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            if (_lifeSpanTimer == 0)
                _lifeSpanTimer = gameTime.TotalGameTime.TotalSeconds;

            _shootingTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Move randomly
            randomMovement.Movement(gameTime, this);

            if (_shootingTimer >= LifeSpan)
                this.IsRemoved = true;

            if (gameTime.TotalGameTime.TotalSeconds - _lifeSpanTimer >= LifeSpan)
                IsRemoved = true;

            if (Position.X > Game1.Viewport.Width)
            {
                Position.X = 0;
            }

            //shoot += elasped;
            //        if (shoot > 1)
            if (_shootingTimer >= TimerStart)
            {
                //shoot = 0;
                //ShootBombs(sprites);
                DropBomb(sprites, new Vector2(0, 0), "bomb", EnemyType.MidBoss);
                //DropBullet(sprites, new Vector2(-1, -1));
                //Position += Velocity;
                _shootingTimer = 0;
            }

            // Direction.X = 10f;
            // Direction.Y = 25f;

        }

        public void DropBomb(List<Sprite> sprites, Vector2 extraDirection, string bombName, EnemyType enemyType)
        {
            var bomb = _projectileFactory.Create(typeof(Bomb).Name, Arts.Bomb2);
            bomb.Direction = Direction + extraDirection;
            bomb.Position = Position;
            bomb.LinearVelocity = 0.07f;
            bomb.LifeSpan = 4f;
            bomb.Parent = this;
            bomb.Velocity = new Vector2(Speed, 0f);

            sprites.Add(bomb);
        }

        public new void DropBullet(List<Sprite> sprites, Vector2 extraDirection)
        {
            var bullet = Bullet.Clone() as Bullet;
            bullet.Direction = Direction + extraDirection;
            bullet.Position = Position;
            bullet.LinearVelocity = 0.07f;
            bullet.LifeSpan = 5f;
            bullet.Parent = this;

            sprites.Add(bullet);
        }

        // Boss Movement Patterns
        IEnumerable<int> FollowPlayer(float acceleration)
        {
            while (true)
            {
               
            }
        }

        public new void OnCollide(Sprite sprite)
        {
            if (sprite is Bullet && ((Bullet)sprite).Parent is Player)
            {
                Health--;

                if (Health <= 0)
                {
                    int scoreValue = 10;                                                   

                    IsRemoved = true;
                    GetScoreValue(sprite.Parent, scoreValue);
                }
            }
        }

        private static void GetScoreValue(Sprite sprite, int scoreValue)
        {
            ((Player)sprite).Score.Value += scoreValue;
        }
    }
}
