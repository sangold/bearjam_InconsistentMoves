using UnityEngine;

namespace Chesslitaire.Utils
{
    public static class Utils
    {
        public static TextMesh CreateText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
        {
            GameObject go = new GameObject("Text", typeof(TextMesh));
            Transform t = go.transform;
            t.SetParent(parent, false);
            t.localPosition = localPosition;
            TextMesh textMesh = go.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.fontSize = fontSize;
            textMesh.text = text;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }

        public static Vector3 GetMouseWorldPosition(Vector3 screenPosition, Camera worldCamera)
        {
            return worldCamera.ScreenToWorldPoint(screenPosition);
        }

        public static Vector3 GetMouseWorldPosition()
        {
            Vector3 vec = GetMouseWorldPosition(Input.mousePosition, Camera.main);
            vec.z = 0;
            return vec;
        }
    }
}
