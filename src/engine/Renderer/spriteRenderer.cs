using SDL3;

class SpriteRenderer
{
    
    nint renderer;

    public SpriteRenderer(nint rendererParam)
    {
        renderer = rendererParam;
    }


    //draw the sprite at a position other than its default
    unsafe public void drawSpriteWithPos(Sprite sprite, int spriteX, int spriteY, float scale = 1)
    {
        SDL.FRect dst = new()
        {
            X=spriteX,
            Y=spriteY,
            W=sprite.width*scale,
            H=sprite.height*scale,
        };

        renderTexture(sprite.GetTexture(), dst);
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

        renderTexture(sprite.GetTexture(), dst);
    }

    //takes a sprite and
    unsafe void renderTexture(nint texture, SDL.FRect dst)
    {
        SDL.RenderTexture(
            renderer,
            texture,
            nint.Zero,
            (nint)(&dst)
        );
    }




}