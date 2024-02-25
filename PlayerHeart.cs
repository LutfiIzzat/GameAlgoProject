using GameAlgoProject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class PlayerHeart : GameObject
{
    // Attributes
    public Texture2D Texture;
    public const int MaxHearts = 3;
    public int RemainingHearts;
    public float scale;

    public PlayerHeart() : base()
    {
    }

    public override void Initialize()
    {
        LoadContent();
        scale = 1.75f;
        RemainingHearts = MaxHearts;
    }

    protected override void LoadContent()
    {
        Texture = _game.Content.Load<Texture2D>("lifebar_16x16");
    }

    public override void Start()
    {
        // Position the hearts vertically aligned
        //Position = new Vector2(750, 50);
    }

    public override void Update()
    {
    }

    public override void Draw()
    {
        // Define the source rectangle for the first quarter of the texture
        Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width / 2, Texture.Height / 2);

        // Define the vertical spacing between hearts
        int verticalSpacing = 11;

        // Draw the hearts
        Vector2 position = Position;
        _game.SpriteBatch.Begin();
        for (int i = 0; i < RemainingHearts; i++)
        {
            _game.SpriteBatch.Draw(Texture, position, sourceRectangle, Color.White, Orientation, Origin, scale, SpriteEffects.None, 0f);
            position.Y += Texture.Height / 2 + verticalSpacing;
        }
        _game.SpriteBatch.End();
    }

    public void DeductHeart()
    {
        if (RemainingHearts > 0)
        {
            RemainingHearts--;
        }
    }
}
