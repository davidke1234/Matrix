﻿using Matrix.Models.Factories;
using Matrix.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Matrix.Models
{
    public class Enemy : Sprite, ICollidable
    {
        public Bullet Bullet;
        private float _shootingTimer;
        public float TimerStart = 1.25f;
        public float Speed = 2f;
        private static ProjectileFactory projectileFactory = new ProjectileFactory();

        public enum Type { 
            BasicEnemies, //small grunts
            ButterFlyEnemies, //larger
            Boss, //midboss
            FinalBoss  //finalboss
        }
        public int Health { get; set; }

        public Enemy(Texture2D texture)
      : base(texture)
        { }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            _shootingTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            string name = _texture.Name.ToLower();

            if (_shootingTimer >= LifeSpan)
                this.IsRemoved = true;

            if (_shootingTimer >= TimerStart)
            {
                if (name == "boss")
                {
                    DropBullet(sprites, new Vector2(15, 15));
                    DropBullet(sprites, new Vector2(-20, -20));
                    DropBullet(sprites, new Vector2(0, 0));
                }
                else if (name == "boss2")
                {
                    DropBullet(sprites, new Vector2(5, 5));
                    DropBullet(sprites, new Vector2(0, 0));
                }
                else
                    DropBullet(sprites, new Vector2(0, 0));

                _shootingTimer = 0;
            }

            Direction.X = 10f;
            Direction.Y = 25f;

            //If off screen, remove enemy
            if (Position.Y < -10)
                IsRemoved = true;

            //B,C,D Enemies
            if (name == "grumpbird" || name == "boss" || name == "boss2")
            {
                Position.X += 1f;
            }
            else
            {
                //A enemies.  Check for pivot point to go up and out
                if (Position.X > 670)
                {
                    //Move them up and off screen
                    Position.Y -= 1f;
                }
                else if (Position.X > 150)
                {
                    //stop Y and go horizantal
                    Position.X += 1f;
                }
                else
                {
                    Position.X += 1f;
                    Position.Y += 1f;
                }
            }
        }

        public void DropBullet(List<Sprite> sprites, Vector2 extraDirection)
        {
            var bullet = Bullet.Clone() as Bullet;
            bullet.Direction = Direction + extraDirection;
            bullet.Position = Position;
            bullet.LinearVelocity = 0.05f;
            bullet.LifeSpan = 6f;
            bullet.Parent = this;

            sprites.Add(bullet);
        }

        public void DropBomb(List<Sprite> sprites, Vector2 extraDirection)
        {
            var bomb = projectileFactory.Create("bomb", Enemy.Type.Boss);
            bomb.Direction = Direction + extraDirection;
            bomb.Position = Position;
            bomb.LinearVelocity = 0.05f;
            bomb.LifeSpan = 6f;
            bomb.Parent = this;

            sprites.Add(bomb);
        }

        public void OnCollide(Sprite sprite)
        {
            //If the player hits an enemy, remove enemy, but score
            if (sprite is Player && !((Player)sprite).Die)
            {
                GetScoreValue(sprite, 1);

                // We want to remove the enemy
                IsRemoved = true;
            }

            // Hit an enemy.  Deduct 1 health point     
            if (sprite is Bullet && ((Bullet)sprite).Parent is Player)
            {
                Health--;

                if (Health <= 0)
                {
                    int scoreValue;

                    switch(Name)
                    {
                        case "boss":
                            scoreValue = 15;
                            break;
                        case "boss2":
                            scoreValue = 10;
                            break;
                        case "grumpbird":
                            scoreValue = 5;
                            break;
                        default:
                            scoreValue = 1;
                            break;                            
                    }

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
