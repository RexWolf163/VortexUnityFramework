using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Vortex.Core.Database.Record;
using Vortex.Extensions;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class DbRecord : ScriptableObject, IDbRecord
{
    [SerializeField, DisplayAsString] private string guid;

    [SerializeField, OnValueChanged("OnNameChanged")]
    private string nameRecord;

    [SerializeField] private string description;
    [PreviewField] private Sprite icon = null;

    public string GetGuid() => guid;

    public string GetName() => nameRecord;
    public string GetDescription() => description;
    public Sprite GetIcon() => icon;

    [Button]
    public string GetCsvTitle()
    {
        var fields = GetType().GetFields();
        var arResult = new List<string>();
        foreach (var field in fields)
            arResult.Add($"\"{field.Name}\"");
        return String.Join(",", arResult);
    }

    [Button]
    public string ToCsvString()
    {
        var fields = GetType().GetFields();
        var arResult = new List<string>();
        foreach (var field in fields)
            arResult.Add($"\"{field.GetValue(this)}\"");
        var result = String.Join(",", arResult);
        return result;
    }

    public bool FromCsvString(string csvString)
    {
        //TODO
        return false;
    }

#if UNITY_EDITOR
    public DbRecord()
    {
        var temp = DateTime.UtcNow.ToFileTimeUtc().ToString() + GetInstanceID();
        guid = Crypto.GetHashSha256(temp);
    }

    [Button]
    public void ResetGuid()
    {
        var temp = DateTime.UtcNow.ToFileTimeUtc().ToString() + GetInstanceID();
        guid = Crypto.GetHashSha256(temp);
    }

    private void OnNameChanged()
    {
        var number = 0;
        while (true)
        {
            var assetPath = AssetDatabase.GetAssetPath(GetInstanceID());
            var result = AssetDatabase.RenameAsset(assetPath, $"{nameRecord}{(number > 0 ? $" ({number})" : "")}");
            if (result != "")
            {
                number++;
                continue;
            }

            AssetDatabase.SaveAssets();
            break;
        }

        if (number > 0)
            Debug.LogError($"[DbRecord] Name {nameRecord} for records already exists!");
    }
#endif
}