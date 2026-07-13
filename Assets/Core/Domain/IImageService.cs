using System.Threading.Tasks;
using UnityEngine;

namespace Core.Domain
{
    public interface IImageService
    {
        Texture2D Load(string path);
        Task<string[]> OpenImageFile();
    }
}