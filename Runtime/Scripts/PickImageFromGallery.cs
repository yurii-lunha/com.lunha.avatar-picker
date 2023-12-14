using System;
using UnityEngine;

namespace Lunha.AvatarPicker
{
    public static class PickImageFromGallery
    {
        public static void TryPick(Action<Texture2D> onComplete, int maxSize = -1)
        {
            var permission = NativeGallery.RequestPermission(
                NativeGallery.PermissionType.Read,
                NativeGallery.MediaType.Image
            );

            if (permission != NativeGallery.Permission.Granted)
            {
                onComplete.Invoke(null);
                return;
            }

            NativeGallery.GetImageFromGallery(path =>
            {
                Texture2D texture = null;

                if (path != null)
                {
                    texture = NativeGallery.LoadImageAtPath(path, maxSize, false);
                }

                onComplete.Invoke(texture);
            });
        }
    }
}