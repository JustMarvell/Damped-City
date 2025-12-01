using UnityEditor;
using UnityEngine;
using System;

[InitializeOnLoad]
public static class AssetReviewPopup
{
    private const string FirstOpenKey = "MyAsset_FirstOpenDate";
    private const string HasReviewedKey = "MyAsset_HasReviewed";
    private const string LastPopupDateKey = "MyAsset_LastPopupDate";

    static AssetReviewPopup()
    {
        EditorApplication.update += CheckAndShowPopup;
    }

    private static void CheckAndShowPopup()
    {
        EditorApplication.update -= CheckAndShowPopup;

        if (EditorPrefs.GetBool(HasReviewedKey, false))
            return; // Already reviewed

        DateTime now = DateTime.Now;

        // Store first open date if not already stored
        if (!EditorPrefs.HasKey(FirstOpenKey))
        {
            EditorPrefs.SetString(FirstOpenKey, now.ToString());
            return;
        }

        DateTime firstOpenDate = DateTime.Parse(EditorPrefs.GetString(FirstOpenKey));
        if ((now - firstOpenDate).TotalDays < 1)
            return; // Less than 1 day since first use

        // Don't show again if already shown today
        if (EditorPrefs.HasKey(LastPopupDateKey))
        {
            DateTime lastShownDate = DateTime.Parse(EditorPrefs.GetString(LastPopupDateKey));
            if ((now - lastShownDate).TotalDays < 1)
                return;
        }

        EditorPrefs.SetString(LastPopupDateKey, now.ToString());
        AssetReviewPopupWindow.ShowWindow();
    }
}

public class AssetReviewPopupWindow : EditorWindow
{
    private const string HasReviewedKey = "MyAsset_HasReviewed";
    private const string ReviewUrl = "https://assetstore.unity.com/packages/YOUR_ASSET_URL_HERE"; // 🔁 Replace this!

    public static void ShowWindow()
    {
        AssetReviewPopupWindow window = GetWindow<AssetReviewPopupWindow>(true, "Rate This Asset", true);
        window.position = new Rect(Screen.width / 2f, Screen.height / 2f, 360, 140);
        window.ShowUtility();
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        GUILayout.Label("Enjoying this asset?", EditorStyles.boldLabel);
        GUILayout.Label("If it's helpful, please consider leaving a review on the Asset Store!", EditorStyles.wordWrappedLabel);

        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("👍 Leave a Review"))
        {
            EditorPrefs.SetBool(HasReviewedKey, true);
            Application.OpenURL(ReviewUrl);
            Close();
        }

        if (GUILayout.Button("Later"))
        {
            Close();
        }
        GUILayout.EndHorizontal();
    }
}
