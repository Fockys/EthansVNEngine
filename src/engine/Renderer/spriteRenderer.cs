using SDL3;

class SpriteRenderer
{
    
    nint renderer;

    public SpriteRenderer(nint rendererParam)
    {
        renderer = rendererParam;
    }
    
    unsafe public void drawSprite(Sprite sprite, float scale = 1)
    {
        SDL.FRect dst = new()
        {
            X = sprite.x,
            Y = sprite.y,
            W = sprite.width * scale,
            H = sprite.height * scale
        };

        SDL.RenderTexture(
            renderer,
            sprite.GetTexture(),
            nint.Zero,
            (nint)(&dst)
        );
    }

}