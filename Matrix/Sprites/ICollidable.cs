﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrix
{
  public interface ICollidable
  {
    void OnCollide(SpriteNew sprite);
  }
}