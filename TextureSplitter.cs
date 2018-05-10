// Created by Warren Smith
// Used in Dark Flame (www.DarkFlameGame.com)
// Class used to break an image into multiple pieces

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nobody_Cares
{
    public class TextureSplitter
    {
        //This will be used for the list of new 'Image Pieces' later
        public struct TexturePieces
        {
            public Texture2D Texture;
            public Vector2 Displacement;

            public TexturePieces(Texture2D texture, Vector2 displacement)
            {
                this.Texture = texture;
                this.Displacement = displacement;
            }
        }
        
        //Public usage of new TexturePieces
        public List<TexturePieces> Textures;

        /// <summary>
        /// Method used to "Break Image" to multiple pieces
        /// </summary>
        /// <param name="texture">The texture used</param>
        /// <param name="widthBreaks">Number of times the image will be divided into horizontally</param>
        /// <param name="heightBreaks">Number of times the image will be divided into vertically</param>
        public void DisassembleTexture(Texture2D texture, int widthBreaks, int heightBreaks)
        {
            Textures = new List<TexturePieces>();

            int width = texture.Width / widthBreaks, 
                height = texture.Height / heightBreaks;

            int xCap = texture.Width / width;
            int yCap = texture.Height / height;

            for (int x = 0; x < xCap; x++)
            {
                for (int y = 0; y < yCap; y++)
                {
                    bool empty = true;
                    
                    TexturePieces tp = new TexturePieces(
                        CropImage(ref empty, texture, new Rectangle(x * width, y * height, width, height)),
                        new Vector2(-x * width, y * height));

                    if(!empty)
                        Textures.Add(tp);
                }
            }

            
        }

        /// <summary>
        /// Method to return a 'Section' of the texture passed in
        /// </summary>
        /// <param name="empty">Used to check if the whole image is empty (Alpha value == 0f)</param>
        /// <param name="tileSheet">Texture used</param>
        /// <param name="tileArea">Size of new 'Image Piece'</param>
        /// <returns>New 'Image Piece'</returns>
        private Texture2D CropImage(ref bool empty, Texture2D tileSheet, Rectangle tileArea)
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
            
            croppedImage.SetData<Color>(croppedImageData);
            return croppedImage;
        }

    }
}
