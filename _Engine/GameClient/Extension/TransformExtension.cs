using UnityEngine;
using DG.Tweening;
public static class TransformExtension {
    private static Vector2 tempVec2 = new Vector2();
    private static Vector3 tempVec3 = new Vector3();
//    private static Color32 tempColor = new Color32();
//    private static Color tempColorF = new Color();

    #region transform
    public static void SetGlobalPosition(this Transform t, float x, float y, float z)
    {
        tempVec3.Set(x, y, z);
        t.position = tempVec3;
    }

    public static void GetGlobalPosition(this Transform t, out float x, out float y, out float z)
    {
        x = t.position.x;
        y = t.position.y;
        z = t.position.z;
    }

    public static void SetPosition(this Transform t, float x, float y, float z)
    {
        t.localPosition = new Vector3(x, y, z);
    }

    public static void SetPositionX(this Transform t, float val)
    {
        tempVec3 = t.localPosition;
        tempVec3.x = val;
        t.localPosition = tempVec3;
    }

    public static void SetPositionY(this Transform t, float val)
    {
        tempVec3 = t.localPosition;
        tempVec3.y = val;
        t.localPosition = tempVec3;
    }

    public static void SetPositionZ(this Transform t, float val)
    {
        tempVec3 = t.localPosition;
        tempVec3.z = val;
        t.localPosition = tempVec3;
    }

    public static void GetPosition(this Transform t, out float x, out float y, out float z)
    {
        x = t.localPosition.x;
        y = t.localPosition.y;
        z = t.localPosition.z;
    }

    public static void SetScale(this Transform t, float x, float y, float z)
    {
        t.localScale = new Vector3(x, y, z);
    }

    public static void GetScale(this Transform t, out float x, out float y, out float z)
    {
        x = t.localScale.x;
        y = t.localScale.y;
        z = t.localScale.z;
    }

    public static void SetGlobalRotation(this Transform t, float x, float y, float z)
    {
        t.rotation = Quaternion.Euler(x, y, z);
    }

    public static void GetGlobalRotation(this Transform t, out float x, out float y, out float z)
    {
        Vector3 v = t.rotation.eulerAngles;
        x = v.x;
        y = v.y;
        z = v.z;
    }

    public static void SetRotation(this Transform t, float x, float y, float z)
    {
        t.localRotation = Quaternion.Euler(x, y, z);
    }

    public static void GetRotation(this Transform t, out float x, out float y, out float z)
    {
        Vector3 v = t.localRotation.eulerAngles;
        x = v.x;
        y = v.y;
        z = v.z;
    }

    public static void SetLookDir(this Transform t, float x, float y, float z)
    {
        tempVec3.Set(x, y, z);
        t.localRotation = Quaternion.LookRotation(tempVec3);
    }

    public static void SetLookAt(this Transform t, float x, float y, float z)
    {
        tempVec3.Set(x, y, z);
        t.LookAt(tempVec3);
    }

    public static void SetVisible(this Transform t, bool visible)
    {
        t.gameObject.SetActive(visible);
    }

    public static bool AddChild(this Transform t, Transform obj)
    {
        obj.SetParent(t, false);
        return true;
    }

    public static bool AddChild(this Transform t, Transform obj, string name)
    {
        Transform child = t.Find(name);
        if (child)
        {
            obj.SetParent(child, false);
            return true;
        }
        return false;
    }

    public static int SetLayer(this Transform t, int layerName)
    {
        int oldLayer = t.gameObject.layer;
        t.gameObject.layer = layerName;
        return oldLayer;
    }

    public static int SetLayer(this Transform t, int layerName, bool changeChildren)
    {
        int oldLayer = t.gameObject.layer;
        t.gameObject.layer = layerName;
        if (changeChildren)
        {
            foreach (Transform child in t)
            {
                SetLayer(child, layerName, changeChildren);
            }
        }
        return oldLayer;
    }

    public static void ResetPRS(this Transform t)
    {
        t.localPosition = Vector3.zero;
        t.localEulerAngles = Vector3.zero;
        t.localScale = Vector3.one;
    }

    public static void ClearAllChild(this Transform t)
    {
        for (int i = 0; i < t.childCount; i++)
        {
            GameObject.Destroy(t.GetChild(i).gameObject);
        }
    }
    #endregion

    #region NGUi

    public static void SetText(this Transform t,string text)
    {
        t.GetComponent<UILabel>().text = text;
    }

    public static void SetSprite(this Transform t, UIAtlas atlas, string sprite)
    {
        if (atlas)
        {
            t.GetComponent<UISprite>().atlas = atlas;
            t.GetComponent<UISprite>().spriteName = sprite;
            return;
        }
        t.GetComponent<UISprite>().spriteName = sprite;
    }
    #endregion


    #region Touch Event

    public static void AddClickEventListener(this Transform t, UIEventListener.VoidDelegate action)
    {
        
        Debug.Log(t.GetType());
        if (t.GetComponent<BoxCollider>() == null)
        {
            NGUITools.AddWidgetCollider(t.gameObject,false);
//            NGUITools.UpdateWidgetCollider(t.gameObject);
//           var obj = t.gameObject.AddComponent<BoxCollider>();
//            obj.isTrigger = true;
        }
        UIEventListener ev = UIEventListener.Get(t.gameObject);
        if (ev != null)
        {
            ev.onClick = action;
        }
    }

    #endregion

    #region DOTween
    public static Tweener DOMove(this Transform target, float x, float y, float z, float duration, bool snapping = false)
    {
        tempVec3.Set(x, y, z);
        return DOTween.To(() => { return target.localPosition; },
            v => { target.localPosition = v; }, tempVec3, duration);
    }

    public static Tweener DOMoveDelta(this Transform target, float x, float y, float z, float duration, bool snapping = false)
    {
        tempVec3.Set(target.localPosition.x + x, target.localPosition.y + y, target.localPosition.z + z);
        return DOTween.To(() => { return target.localPosition; },
            v => { target.localPosition = v; }, tempVec3, duration);
    }

    public static Tweener DOMoveUI(this Transform target, float x, float y, float duration, bool snapping = false)
    {
        tempVec2.Set(x, y);

        RectTransform rt = target.GetComponent<RectTransform>();
        Tweener tw =  DOTween.To(() => { return rt.anchoredPosition; },
            v => { rt.anchoredPosition = v; }, tempVec2, duration);

        if (snapping == true) {
            tw.SetEase(Ease.Linear);
        }
        return tw;
    }

    public static Tweener DOMoveDeltaUI(this Transform target, float x, float y, float duration, bool snapping = false)
    {
        RectTransform rt = target.GetComponent<RectTransform>();
        tempVec2.Set(rt.anchoredPosition.x + x, rt.anchoredPosition.y + y);
        return DOTween.To(() => { return rt.anchoredPosition; },
            v => { rt.anchoredPosition = v; }, tempVec2, duration);
    }

    public static Tweener DORotateUI(this Transform target, float val, float duration)
    {
        tempVec3.Set(0, 0, val);
        return target.DOLocalRotate(tempVec3, duration, RotateMode.FastBeyond360);
    }

    public static Tweener DORotate(this Transform target, float x, float y, float z, float duration)
    {
        tempVec3.Set(x, y, z);
        return target.DOLocalRotate(tempVec3, duration, RotateMode.Fast);
    }

    public static Tweener DOLookAt(this Transform target, float x, float y, float z, float duration)
    {
        tempVec3.Set(x, y, z);
        return target.DOLookAt(tempVec3, duration);
    }

    public static Tweener DOScaleUI(this Transform target, float x, float y, float duration, bool snapping = false)
    {
        RectTransform rt = target.GetComponent<RectTransform>();

        tempVec2.Set(x, y);
        return DOTween.To(() => { return (Vector2)rt.localScale; },
            v => { rt.localScale = v; }, tempVec2, duration);
    }

//    public static Tweener DOColor(this Transform target, byte r, byte g, byte b, float duration, bool snapping = false)
//    {
//        tempColor.r = r;
//        tempColor.g = g;
//        tempColor.b = b;
//
//        Graphic img = target.GetComponent<Graphic>();
//        return DOTween.To(
//            () => { return img.color; },
//            v =>
//            {
//                tempColor = v;
//                tempColor.a = (byte)(img.color.a * 255);
//                img.color = tempColor;
//            }, tempColor, duration);
//    }
//
//    public static Tweener DOAlphaF(this Transform target, float a, float duration, bool snapping = false)
//    {
//        tempColorF.a = a;
//
//        var cg = target.GetComponent<CanvasGroup>();
//        if (cg != null)
//        {
//            cg = target.GetComponent<CanvasGroup>();
//            return DOTween.To(() => cg.alpha, x => cg.alpha = x, a, duration);
//        }
//        else
//        {
//            var img = target.GetComponent<Graphic>();
//            return DOTween.To(
//                () => { return img.color; },
//                v =>
//                {
//                    tempColorF = img.color;
//                    tempColorF.a = v.a;
//                    img.color = tempColorF;
//                }, tempColorF, duration);
//        }
//    }
//
//    public static Tweener DOSpriteAnimation(this Transform t, string bundleName, string aniName, float speed, int times)
//    {
//        Image img = t.GetComponent<Image>();
//        float tempFloat = (float)0.0;
//        if (!img)
//        {
//            return DOTween.To(() => tempFloat, x => tempFloat = x, 1.0f, 1.0f).SetEase(Ease.Linear).SetLoops(times);
//        }
//        UIAtlasDesc atlasDesc = AssetManager.Instance.GetUIAtlasDesc(bundleName, "ui_atlas_desc");
//        if (!atlasDesc)
//        {
//            return DOTween.To(() => tempFloat, x => tempFloat = x, 1.0f, 1.0f).SetEase(Ease.Linear).SetLoops(times);
//        }
//        var sprites = atlasDesc.GetAnimationSprites(aniName);
//        if (sprites == null || sprites.Count == 0)
//        {
//            return DOTween.To(() => tempFloat, x => tempFloat = x, 1.0f, 1.0f).SetEase(Ease.Linear).SetLoops(times);
//        }
//        ImageEtc imgEtc = img as ImageEtc;
//        if(imgEtc != null)
//        {
//            Texture alphaTexture = atlasDesc.GetAlphaTexture(sprites[0].texture.name);
//            imgEtc.SetAlphaTexture(alphaTexture);
//        }
//        return DOTween.To(
//            () => tempFloat,
//            v =>
//            {
//                tempFloat = v;
//                int index = (int)(v / (1.0f / sprites.Count));
//                if (img.sprite != sprites[index])
//                {
//                    img.sprite = sprites[index];
//                    img.SetNativeSize();
//                }
//
//            }, 1.0f, (1.0f / 8) * sprites.Count / speed).SetEase(Ease.Linear).SetLoops(times);
//    }

//    public static Tweener DOSpriteAnimation(this Transform t, string bundleName, string aniName)
//    {
//        return t.DOSpriteAnimation(bundleName, aniName, 1.0f, 1);
//    }

    public static void KillTweener(this Transform t, Tweener tweener, bool is_complate = false)
    {
        tweener.Kill(is_complate);
    }

    #endregion


}