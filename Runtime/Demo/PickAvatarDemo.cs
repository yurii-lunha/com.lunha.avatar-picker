using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lunha.AvatarPicker
{
    public class PickAvatarDemo : MonoBehaviour
    {
        [SerializeField] Image _avatarRenderer;

        [Space] [SerializeField] Button _pickFromGalleryButton;
        [SerializeField] Button _takeFromCameraButton;

        [Header("Cropper parameters")] [SerializeField]
        Color _backgroundColor = Color.clear;

        [SerializeField] Toggle _ovalSelectionInput;
        [SerializeField] Toggle _autoZoomInput;
        [SerializeField] InputField _minAspectRatioInput;
        [SerializeField] InputField _maxAspectRatioInput;

        void OnEnable()
        {
            _pickFromGalleryButton.onClick.AddListener(OnPickFromGallery);
            _takeFromCameraButton.onClick.AddListener(OnTakeFromCamera);
        }

        void OnDisable()
        {
            _pickFromGalleryButton.onClick.RemoveListener(OnPickFromGallery);
            _takeFromCameraButton.onClick.RemoveListener(OnTakeFromCamera);
        }

        void OnPickFromGallery()
        {
            PickImageFromGallery.TryPick(CropNRenderAvatar);
        }

        void OnTakeFromCamera()
        {
            TakeImageFromCamera.TryPick(CropNRenderAvatar);
        }

        void CropNRenderAvatar(Texture2D texture2D)
        {
            if (texture2D == null)
            {
                Debug.LogWarning($"No image has been selected");
                return;
            }

            CropAvatar(texture2D, croppedTexture2D =>
            {
                if (croppedTexture2D == null)
                {
                    Debug.LogWarning($"Image has not been cropped");
                    return;
                }

                var sprite = Sprite.Create(croppedTexture2D,
                    new Rect(0, 0, croppedTexture2D.width, croppedTexture2D.height),
                    new Vector2(0.5f, 0.5f));
                _avatarRenderer.sprite = sprite;
            });
        }

        void CropAvatar(Texture2D texture2D, Action<Texture2D> croppedTexture2D)
        {
            ImageCropper.Instance.Show(texture2D,
                (bool result, Texture originalImage, Texture2D croppedImage) =>
                {
                    croppedTexture2D?.Invoke(result ? croppedImage : null);
                },
                settings: new ImageCropper.Settings()
                {
                    ovalSelection = _ovalSelectionInput.isOn,
                    autoZoomEnabled = _autoZoomInput.isOn,
                    imageBackground = _backgroundColor,
                    selectionMinAspectRatio = int.Parse(_minAspectRatioInput.text),
                    selectionMaxAspectRatio = int.Parse(_maxAspectRatioInput.text)
                },
                croppedImageResizePolicy: (ref int width, ref int height) =>
                {
                    // uncomment lines below to save cropped image at half resolution
                    //width /= 2;
                    //height /= 2;
                });
        }
    }
}