using System;
using System.Threading.Tasks;
using Core.Domain;
using SimpleFileBrowser;
using UnityEngine;

namespace Core.Services
{
    public class ImageService : IImageService
    {
        public ImageService()
        {
            FileBrowser.SetFilters(false);
            RequestPermissionAsynchronously();
        }

        private void RequestPermissionAsynchronously()
        {
#if UNITY_ANDROID || UNITY_IOS
        // if (!FileBrowser.CheckPermission())
        FileBrowser.RequestPermissionAsync((permission) =>
        {
            if (!(permission == FileBrowser.Permission.Granted))
                ServiceLocator.Get<ILogService>().LogError("No permission received to use file browser")
        });
#endif
        }

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

        public Task<string[]> OpenImageFile()
        {
            FileBrowser.SetFilters(false,
                new FileBrowser.Filter("Images", ".png", ".jpg", ".jpeg")
            );
            FileBrowser.SetDefaultFilter(".png");

            var result = new TaskCompletionSource<string[]>();
            FileBrowser.ShowLoadDialog(
                onSuccess: (paths) =>
                {
                    result.TrySetResult(paths);
                },
                onCancel: () =>
                {
                    result.TrySetResult(Array.Empty<string>());
                },
                pickMode: FileBrowser.PickMode.Files,
                allowMultiSelection: true,
                title: "Open Image"
            );

            return result.Task;
        }
    }
}
