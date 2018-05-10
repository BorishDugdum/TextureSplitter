using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Zobenbralu
{
    public class TextureSplitter
    {
        public struct TexturePieces
        {
            public Texture2D Texture;
            public float GravityWeight;
            public Vector2 Displacement;

            public TexturePieces(Texture2D texture, Vector2 displacement, float gravity)
            {
                this.Texture = texture;
                this.GravityWeight = gravity;
                this.Displacement = displacement;
            }
        }
        public List<TexturePieces> Textures;

        //ContentManager content;
        //public void LoadContent(ContentManager content)
        //{
        //    this.content = content;
        //}


        public void DisassembleTexture(Texture2D texture, string damageType)
        {
            Textures = new List<TexturePieces>();

            //int x = 0, y = 0;
            int width = texture.Width / 4, height = texture.Height / 4;
            

            switch (damageType)
            {
                case "AirDMG":
                    width = texture.Width / 8;
                    height = texture.Height / 8;
                    break;
                case "DissipateBurn":
                    width = texture.Width / 5;
                    height = texture.Height / 15;
                    break;
                case "DissipateBurnUp":
                    width = texture.Width / 4;
                    height = texture.Height / 10;
                    break;
                case "SpineBurn":
                    width = texture.Width / 7;
                    height = texture.Height / 15;
                    break;
                case "HeavyKnightExplosion":
                    width = texture.Width / 10;
                    height = texture.Height / 10;
                    break;
                case "Charred Spirit Explosion":
                    width = texture.Width / 7;
                    height = texture.Height / 7;
                    break;
                case "Shield":
                    width = texture.Width / 3;
                    height = texture.Height / 3;
                    break;
                case "Wall":
                    width = 10;
                    height = 10;
                    break;
                case "Large Dust":
                    width = texture.Width / 15;
                    height = texture.Height / 15;
                    break;
                default:
                    break;
            }

            int xCap = texture.Width / width;
            int yCap = texture.Height / height;

            for (int x = 0; x < xCap; x++)
            {
                for (int y = 0; y < yCap; y++)
                {
                    bool empty = true;
                    //4 Pieces Evenly//
                    TexturePieces tp = new TexturePieces(
                        CropImage(ref empty, texture, damageType, new Rectangle(x * width, y * height, width, height)),
                        new Vector2(-x * width, y * height),
                        6);

                    if(!empty)
                        Textures.Add(tp);
                }
            }

            
        }

        private Texture2D CropImage(ref bool empty, Texture2D tileSheet, string damageType, Rectangle tileArea)
        {
            if (tileArea.Width == 0)
                tileArea.Width = 1;
            else if (tileArea.Width >= tileSheet.Bounds.Width)
                tileArea.Width = tileSheet.Bounds.Width / 2;

            if (tileArea.Height == 0)
                tileArea.Height = 1;
            else if (tileArea.Height >= tileSheet.Bounds.Height)
                tileArea.Height = tileSheet.Bounds.Height / 2;

            Texture2D croppedImage = new Texture2D(tileSheet.GraphicsDevice, tileArea.Width, tileArea.Height);

            Color[] tileSheetData = new Color[tileSheet.Width * tileSheet.Height];
            Color[] croppedImageData = new Color[croppedImage.Width * croppedImage.Height];

            tileSheet.GetData<Color>(tileSheetData);

            int index = 0;

            switch (damageType)
            {
                case "AirDMG":
                    for (int y = tileArea.Y; y < tileArea.Y + tileArea.Height; y++)
                    {
                        for (int x = tileArea.X; x < tileArea.X + tileArea.Width; x++)
                        {
                            croppedImageData[index] = tileSheetData[y * tileSheet.Width + x];
                            if (croppedImageData[index].A != 0)
                            {
                                empty = false;
                                //croppedImageData[index] = new Color(
                                //    croppedImageData[index].R + (byte)25,
                                //    croppedImageData[index].G + (byte)25,
                                //    croppedImageData[index].B + (byte)25);
                            }
                            index++;
                        }
                    }
                    break;
                default:
                    for (int y = tileArea.Y; y < tileArea.Y + tileArea.Height; y++)
                    {
                        for (int x = tileArea.X; x < tileArea.X + tileArea.Width; x++)
                        {
                            croppedImageData[index] = tileSheetData[y * tileSheet.Width + x];
                            if (croppedImageData[index].A != 0)
                                empty = false;
                            index++;
                        }
                    }
                    break;
            }
            
            croppedImage.SetData<Color>(croppedImageData);
            return croppedImage;
        }

    }
}
