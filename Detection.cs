using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using OpenMetaverse;
using OpenMetaverse.Assets;

namespace FirstBot
{
    static class Detection
    {
        private static TextureRequestState resultState;
        private static AssetTexture asset = null;
        private static AutoResetEvent DownloadHandle = new AutoResetEvent(false);

        private static List<Primitive> foundPrims;

        public static void FindObjects(GridClient Client)
        {
            Vector3 location = Client.Self.SimPosition;

            foundPrims = Client.Network.CurrentSim.ObjectsPrimitives.FindAll(
                delegate(Primitive prim)
                {
                    Vector3 pos = prim.Position;
                    return (prim.ParentID == 0 && pos != Vector3.Zero);
                }
            );
        }

        public static void DetectSteg(GridClient Client)
        {
            int count = 1;
            Console.WriteLine("Found: " + foundPrims.Count + " objects");

            foreach (Primitive p in foundPrims)
            {
                Console.Write(count + ". Analyzing texture...");
                UUID textureID = p.Textures.DefaultTexture.TextureID;
                DownloadHandle.Reset();

                Client.Assets.RequestImage(textureID, ImageType.Normal, ImageReceived);
                if (DownloadHandle.WaitOne(1000, false))
                {
                    if (resultState == TextureRequestState.Finished)
                    {
                        if (asset != null && asset.Decode())
                        {
                            string s = "";
                            var file = asset.AssetData;
                            for (int i = 0; i < file.Length; i++)
                            {
                                s += String.Format("{0:X2}", file[i]);
                            }

                            if (s.IndexOf("504B0304") != -1)
                            {
                                Extraction.DownloadSteg(asset.AssetData, asset.AssetID, Client);
                            }
                            else
                                Console.WriteLine("Nothing found.");
                        }
                    }
                }
                count++;

            }
        }

        private static void ImageReceived(TextureRequestState state, AssetTexture assetID)
        {
            resultState = state;
            asset = assetID;

            DownloadHandle.Set();
        }
    }
}
