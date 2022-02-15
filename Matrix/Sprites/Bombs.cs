﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Matrix
{
    class Bombs : Sprite
    {
        private static Bombs _instance;

        /// <summary>
        /// Provides an instance of the Bombs class
        /// </summary>
        public static Bombs Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Bombs();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Creates an instance of <see cref="Bombs" class./>
        /// </summary>
        private Bombs()
        {
            image = Arts.Bomb;
            Position.X = 0;
            Position.Y = 0;
        }

        public override void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>        
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, Position, Color.White);
        }
    }
}