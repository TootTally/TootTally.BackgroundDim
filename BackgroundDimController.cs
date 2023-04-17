using BaboonAPI.Hooks.Tracks;
using HarmonyLib;
using TrombLoader.CustomTracks;
using UnityEngine;
using UnityEngine.UI;

namespace TootTally.BackgroundDim
{
    public class BackgroundDimController:MonoBehaviour
    {
        private static readonly Vector2 DEFAULT_RESOLUTION = new Vector2(1920,1080);

        private static GameObject _backgroundDimGameObject;
        private static Image _backgroundDimImage;
        private static CanvasScaler _backgroundDimCanvasScaler;
        private static float _alpha;
        

        [HarmonyPatch(typeof(GameController),nameof(GameController.Start))]
        [HarmonyPostfix]

        public static void BackgroundDim(GameController __instance)
        {
            GameObject bgGameObject = GameObject.Find("BGCameraObj");
            _alpha = Plugin.Instance.option.DimAmount.Value;

            _backgroundDimGameObject = Instantiate(new GameObject(), bgGameObject.transform);

            _backgroundDimGameObject.transform.position = new Vector3(0, 0, 100);
            
            _backgroundDimImage = _backgroundDimGameObject.AddComponent<Image>();
            _backgroundDimImage.color = new Color(0,0,0, _alpha);

            _backgroundDimCanvasScaler = _backgroundDimGameObject.AddComponent<CanvasScaler>();
            _backgroundDimCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            _backgroundDimCanvasScaler.referenceResolution = DEFAULT_RESOLUTION;
            
            RectTransform bgDimRectTransform = _backgroundDimGameObject.GetComponent<RectTransform>();
            bgDimRectTransform.sizeDelta = DEFAULT_RESOLUTION;

        }
    }
}
