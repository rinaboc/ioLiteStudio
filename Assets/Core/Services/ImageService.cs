using System;
using System.Threading.Tasks;
using Core.Domain;
using SimpleFileBrowser;
using UnityEngine;

namespace Core.Services
{
    public interface IImageService
    {
        Texture2D Load(string path);
    }

    public class ImageService : IImageService
    {
        public Texture2D Load(string path)
        {
            byte[] bytes;
            try
            {
                bytes = FileBrowserHelpers.ReadBytesFromFile(path);
            }
            catch (Exception e)
            {
                ServiceLocator.Get<ILogService>()?.LogError("[IMAGE_SERVICE] Couldn't load image async.", e);
                return null;
            }

            if (bytes == null || bytes.Length == 0)
                return null;

            var texture = new Texture2D(1, 1, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Trilinear
            };

            texture.LoadImage(bytes);

            return texture;
        }
    }
}
