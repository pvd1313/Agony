using UnityEngine;

namespace Agony.AssetTools
{
    public static class SpriteUtil
    {
        private static readonly Material drawMaterial;

        static SpriteUtil()
        {
            var assets = AssetBundle.LoadFromMemory(Properties.Resources.Shaders);
            var shader = assets.LoadAsset<Shader>("UnlitShader");
            drawMaterial = new Material(shader);
        }

        public static Texture2D Render(Sprite sprite)
        {
            if (sprite == null) throw new System.ArgumentNullException($"{nameof(sprite)} is null");

            var tempRender = RenderTexture.GetTemporary((int)sprite.rect.width, (int)sprite.rect.height, 0, RenderTextureFormat.ARGB32);
            var gameRender = RenderTexture.active;
            RenderTexture.active = tempRender;

            drawMaterial.mainTexture = sprite.texture;
            GL.Clear(true, true, Color.clear);
            GLSprite(sprite, drawMaterial);
            var newTexture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, TextureFormat.ARGB32, true);
            newTexture.ReadPixels(new Rect(0, 0, newTexture.width, newTexture.height), 0, 0);
            newTexture.Apply();

            RenderTexture.active = gameRender;
            RenderTexture.ReleaseTemporary(tempRender);
            return newTexture;
        }

        private static void GLSprite(Sprite sprite, Material material)
        {
            var triangles = sprite.triangles;
            var vertices = sprite.vertices;
            var uv = sprite.uv;
            var extents = sprite.bounds.extents;
            var size = sprite.bounds.size;

            GL.PushMatrix();
            material.SetPass(0);
            GL.LoadOrtho();

            GL.Begin(GL.TRIANGLES);
            foreach (var t in triangles)
            {
                GL.TexCoord2(uv[t].x, uv[t].y);
                GL.Vertex3((vertices[t].x + extents.x) / size.x, (vertices[t].y + extents.y) / size.y, 0);
            }
            GL.End();

            GL.PopMatrix();
        }
    }
}