using System;
using Features.GameShop;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScriptableGameShopData))]
public class ScriptableGameShopDataEditor : Editor
{
    private SerializedProperty _bundlesProperty;
    private SerializedProperty _dataProperty;
    private string[] _skuDisplayNames;
    private string[] _skuIds;
    private SkuDefinitionProvider.SkuInfo[] _skuInfos;

    private void OnEnable()
    {
        _dataProperty = serializedObject.FindProperty("Data");
        _bundlesProperty = _dataProperty.FindPropertyRelative("Bundles");

        RefreshSkuData();
    }

    private void RefreshSkuData()
    {
        _skuInfos = SkuDefinitionProvider.GetSkuInfos();
        _skuIds = new string[_skuInfos.Length];
        _skuDisplayNames = new string[_skuInfos.Length];

        for (var i = 0; i < _skuInfos.Length; i++)
        {
            _skuIds[i] = _skuInfos[i].SkuId;
            _skuDisplayNames[i] = _skuInfos[i].DisplayName;
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Game Shop Data", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        if (GUILayout.Button("Add Bundle"))
        {
            _bundlesProperty.arraySize++;
            var newBundle = _bundlesProperty.GetArrayElementAtIndex(_bundlesProperty.arraySize - 1);
            var purchaseData = newBundle.FindPropertyRelative("PurchaseData");
            var rewardData = newBundle.FindPropertyRelative("RewardData");
            purchaseData.arraySize = 0;
            rewardData.arraySize = 0;
        }

        EditorGUILayout.Space();

        for (var i = 0; i < _bundlesProperty.arraySize; i++)
        {
            DrawBundle(i);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawBundle(int index)
    {
        var bundle = _bundlesProperty.GetArrayElementAtIndex(index);
        var bundleIdProperty = bundle.FindPropertyRelative("BundleId");
        var titleProperty = bundle.FindPropertyRelative("Title");
        var purchaseDataProperty = bundle.FindPropertyRelative("PurchaseData");
        var rewardDataProperty = bundle.FindPropertyRelative("RewardData");

        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField($"Bundle {index + 1}", EditorStyles.boldLabel);
        if (GUILayout.Button("Remove", GUILayout.Width(60)))
        {
            _bundlesProperty.DeleteArrayElementAtIndex(index);
            return;
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(bundleIdProperty, new GUIContent("Bundle ID"));
        EditorGUILayout.PropertyField(titleProperty, new GUIContent("Title"));

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Purchase Data", EditorStyles.boldLabel);
        DrawSkuDataArray(purchaseDataProperty, "Purchase", true);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Reward Data", EditorStyles.boldLabel);
        DrawSkuDataArray(rewardDataProperty, "Reward", false);

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
    }

    private void DrawSkuDataArray(SerializedProperty arrayProperty, string label, bool negateValue)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField($"{label} Items ({arrayProperty.arraySize})", EditorStyles.boldLabel);
        if (GUILayout.Button($"Add {label}", GUILayout.Width(80)))
        {
            arrayProperty.arraySize++;
        }

        EditorGUILayout.EndHorizontal();

        for (var i = 0; i < arrayProperty.arraySize; i++)
        {
            DrawSkuData(arrayProperty.GetArrayElementAtIndex(i),
                        i,
                        label,
                        negateValue,
                        arrayProperty.DeleteArrayElementAtIndex);
        }
    }

    private void DrawSkuData(SerializedProperty skuDataProperty,
                             int index,
                             string label,
                             bool negateValue,
                             Action<int> deleteAction)
    {
        var skuIdProperty = skuDataProperty.FindPropertyRelative("SkuId");
        var amountProperty = skuDataProperty.FindPropertyRelative("Amount");

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField($"{label} {index + 1}:", GUILayout.Width(80));

        var selectedIndex = GetSkuIndex(skuIdProperty.stringValue);
        var newIndex = EditorGUILayout.Popup(selectedIndex, _skuDisplayNames, GUILayout.Width(200));
        skuIdProperty.stringValue = _skuIds[newIndex];

        var amount = amountProperty.stringValue;
        if (negateValue && int.TryParse(amount, out var amountInt))
        {
            amount = (-amountInt).ToString();
        }

        var newValue = EditorGUILayout.TextField(amount);
        if (!string.IsNullOrEmpty(newValue) && int.TryParse(newValue, out var newValueInt) && negateValue)
        {
            newValue = (-newValueInt).ToString();
        }

        amountProperty.stringValue = newValue;

        if (GUILayout.Button("Remove", GUILayout.Width(60)))
        {
            deleteAction?.Invoke(index);
        }

        EditorGUILayout.EndHorizontal();
    }

    private int GetSkuIndex(string skuId)
    {
        for (var i = 0; i < _skuIds.Length; i++)
        {
            if (_skuIds[i] == skuId)
            {
                return i;
            }
        }

        return 0;
    }
}