using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAlgoProject
{
    class ScalableGameTime
    {
        public static float TimeScale { get; set; }
        public static float DeltaTime { get; private set; }
        public static float RealTime { get; private set; }
        public static float UnscaledRealTime { get; private set; }
        public static float UnscaledDeltaTime { get; private set; }

        public static GameTime GameTime { get; private set; }

        public static GameTime UnscaledGameTime { get; private set; }

        static ScalableGameTime()
        {
            TimeScale = 1f;
        }

        public static void Process(GameTime gameTime)
        {
            UnscaledDeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            DeltaTime = UnscaledDeltaTime * TimeScale;

            UnscaledRealTime = (float)gameTime.TotalGameTime.TotalSeconds;

            RealTime = UnscaledRealTime + DeltaTime;

            UnscaledGameTime = gameTime;

            GameTime = gameTime;
            GameTime.ElapsedGameTime = TimeSpan.FromSeconds(DeltaTime);
            GameTime.TotalGameTime = TimeSpan.FromSeconds(RealTime);
        }
    }
}
