using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StarControlMelee.Graphics
{
    public static class TextureFactory
    {
        static float ClampF(float v, float a, float b) => (v < a) ? a : (v > b) ? b : v;

        public static Texture2D CreatePlanetTexture(GraphicsDevice gd, int size = 256)
        {
            var tex = new Texture2D(gd, size, size);
            var data = new Color[size * size];
            var center = new Vector2(size / 2f, size / 2f);
            float radius = size / 2f;

            uint Seed(uint s) { s ^= s << 13; s ^= s >> 17; s ^= s << 5; return s; }

            uint rnd = 123456789;
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    int idx = y * size + x;
                    var p = new Vector2(x + 0.5f, y + 0.5f);
                    float d = Vector2.Distance(p, center) / radius;

                    if (d > 1f)
                    {
                        data[idx] = Color.Transparent;
                        continue;
                    }

                    var col = new Vector4(0.0f, 0.5f, 0.95f, 1f);

                    float landMask = 0f;
                    for (int b = 0; b < 6; b++)
                    {
                        rnd = Seed(rnd + (uint)b * 7919u);
                        float bx = (int)(rnd % (uint)size);
                        rnd = Seed(rnd);
                        float by = (int)(rnd % (uint)size);
                        rnd = Seed(rnd);
                        float br = 20f + (rnd % 80);

                        float dist = Vector2.Distance(p, new Vector2(bx + 0.5f, by + 0.5f));
                        float v = MathF.Max(0f, 1f - (dist / br));
                        landMask += v * v;
                    }

                    landMask = ClampF(landMask, 0f, 1f);

                    if (landMask > 0.12f)
                    {
                        var land = new Vector4(0.18f, 0.7f, 0.25f, 1f);
                        var rock = new Vector4(0.6f, 0.45f, 0.25f, 1f);
                        float t = (landMask - 0.12f) / 0.6f;
                        t = ClampF(t, 0f, 1f);
                        var mixed = Vector4.Lerp(land, rock, t);
                        col = mixed;
                    }

                    rnd = Seed(rnd + (uint)(x * 73856093 ^ y * 19349663));
                    float cloud = ((rnd % 100u) / 100f);
                    float cloudMask = (cloud > 0.92f) ? (cloud - 0.92f) * 12f : 0f;
                    cloudMask = ClampF(cloudMask, 0f, 1f);

                    float rim = 1f - MathF.Pow(d, 1.2f) * 0.4f;
                    var final = new Vector4(col.X * rim, col.Y * rim, col.Z * rim, 1f);
                    final = Vector4.Lerp(final, new Vector4(1f, 1f, 1f, 1f), cloudMask * 0.9f);

                    float fx = Math.Min(final.X * 1.05f, 1f);
                    float fy = Math.Min(final.Y * 1.05f, 1f);
                    float fz = Math.Min(final.Z * 1.12f, 1f);
                    final = new Vector4(fx, fy, fz, final.W);

                    data[idx] = new Color(new Vector4(final.X, final.Y, final.Z, final.W));
                }
            }

            tex.SetData(data);
            return tex;
        }

        public static Texture2D CreateShipTexture(GraphicsDevice gd, int shipW = 128, int shipH = 64)
        {
            var tex = new Texture2D(gd, shipW, shipH);
            var shipData = new Color[shipW * shipH];
            for (int i = 0; i < shipData.Length; i++) shipData[i] = Color.Transparent;

            var sCenter = new Vector2(shipW * 0.5f, shipH / 2f);
            var tip = new Vector2(shipW - 8, shipH / 2f);
            float bodyRadius = shipH * 0.45f;

            for (int y = 0; y < shipH; y++)
            {
                for (int x = 0; x < shipW; x++)
                {
                    var px = x + 0.5f;
                    var py = y + 0.5f;
                    float dCenter = Vector2.Distance(new Vector2(px, py), sCenter);
                    float rawt = (tip.X - px) / (shipW - sCenter.X);
                    float t = ClampF(rawt, -1f, 1f);

                    float alpha = (1f - (dCenter / bodyRadius)) * (1f - MathF.Max(0f, t));
                    alpha = ClampF(alpha, 0f, 1f);

                    float aaRaw = (alpha - 0.02f) / 0.98f;
                    float a = ClampF(aaRaw, 0f, 1f);

                    if (a > 0f)
                    {
                        float shade = 0.65f + 0.35f * (1f - (dCenter / bodyRadius));
                        var col = new Vector4(shade, shade, shade, a);
                        shipData[y * shipW + x] = new Color(col);
                    }
                }
            }

            int wx = shipW * 3 / 8;
            int wy = shipH / 2;
            int ww = shipW / 8;
            int wh = shipH / 6;
            for (int yy = -wh; yy <= wh; yy++)
            {
                for (int xx = -ww; xx <= ww; xx++)
                {
                    int sx = wx + xx;
                    int sy = wy + yy;
                    if (sx >= 0 && sx < shipW && sy >= 0 && sy < shipH)
                    {
                        float dist = MathF.Sqrt(xx * xx + yy * yy) / (MathF.Max(ww, wh));
                        float ca = ClampF(1f - dist, 0f, 1f) * 0.9f;
                        var prev = shipData[sy * shipW + sx].ToVector4();
                        var win = new Vector4(0.2f, 0.5f, 1f, ca);
                        var blended = Vector4.Lerp(prev, win, win.W);
                        shipData[sy * shipW + sx] = new Color(blended);
                    }
                }
            }

            tex.SetData(shipData);
            return tex;
        }

        public static Texture2D CreateProjectileTexture(GraphicsDevice gd, int pd = 24)
        {
            var tex = new Texture2D(gd, pd, pd);
            var pData = new Color[pd * pd];
            var pc = new Vector2(pd / 2f, pd / 2f);
            float pr = pd / 2f;
            for (int y = 0; y < pd; y++)
            {
                for (int x = 0; x < pd; x++)
                {
                    int idx = y * pd + x;
                    var p = new Vector2(x + 0.5f, y + 0.5f);
                    float dist = Vector2.Distance(p, pc) / pr;
                    float a = 1f - ClampF(dist, 0f, 1f);
                    a = MathF.Pow(a, 1.8f);
                    var col = new Vector4(1f, 0.9f, 0.4f, a);
                    pData[idx] = new Color(col);
                }
            }

            tex.SetData(pData);
            return tex;
        }
    }
}
