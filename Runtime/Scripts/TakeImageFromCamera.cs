using System;
using UnityEngine;

namespace Lunha.AvatarPicker
{
    public static class TakeImageFromCamera
    {
        public static void TryPick(Action<Texture2D> onComplete, int maxSize = -1)
        {
            if (!NativeCamera.DeviceHasCamera())
            {
                onComplete.Invoke(null);
                return;
            }

            if (NativeCamera.CheckPermission(true) == NativeCamera.Permission.Denied)
            {
                if (NativeCamera.RequestPermission(true) != NativeCamera.Permission.Granted)
                {
                    onComplete.Invoke(null);
                    return;
                }
            }

            NativeCamera.TakePicture(path =>
            {
                Texture2D texture = null;
                if (path != null && !string.IsNullOrEmpty(path))
                {
                    texture = NativeGallery.LoadImageAtPath(path, maxSize, false);
                }

                onComplete.Invoke(texture);
            }, maxSize);
        }

        public static bool HasPermission()
        {
            return NativeCamera.CheckPermission(true) == NativeCamera.Permission.Granted;
        }

        public static void RequestPermission(Action<bool> onComplete)
        {
            bool permissionGranted = NativeCamera.RequestPermission(true) == NativeCamera.Permission.Granted;
            onComplete.Invoke(permissionGranted);
        }
    }
}