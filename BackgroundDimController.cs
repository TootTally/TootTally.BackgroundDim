using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace TootTally.BackgroundDim
{
    public class BackgroundDimController:MonoBehaviour
    {
        private static readonly Vector2 DEFAULT_RESOLUTION = new Vector2(1000,300);

        private static GameObject _backgroundDimGameObject;
        private static Image _backgroundDimImage;
        private static CanvasScaler _backgroundDimCanvasScaler;
        private static float _alpha;
        

        [HarmonyPatch(typeof(GameController),nameof(GameController.Start))]
        [HarmonyPostfix]

        public static void BackgroundDim(GameController __instance)
        {
            GameObject bgGameObject = GameObject.Find("BGCameraObj");

            _backgroundDimGameObject = Instantiate(new GameObject(), bgGameObject.transform);
            _backgroundDimGameObject.transform.SetAsFirstSibling();
            _backgroundDimGameObject.transform.position = Vector2.zero;
            
            _backgroundDimImage = _backgroundDimGameObject.AddComponent<Image>();
            _backgroundDimImage.color = new Color(0,0,0,0.75f);

            _backgroundDimCanvasScaler = _backgroundDimGameObject.AddComponent<CanvasScaler>();
            _backgroundDimCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            _backgroundDimCanvasScaler.referenceResolution = DEFAULT_RESOLUTION;
            
            RectTransform bgDimRectTransform = _backgroundDimGameObject.GetComponent<RectTransform>();
            //bgDimRectTransform.anchoredPosition = Vector2.zero;
            bgDimRectTransform.sizeDelta = DEFAULT_RESOLUTION;

        }
    }
}
