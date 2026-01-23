#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

public class ContextCreator : MonoBehaviour
{
    [Tooltip("Выберите папку в Assets (например, 'Assets/Scripts'). Убедитесь, что это именно папка.")]
    public DefaultAsset targetFolder;

    private string outputFileName = "context";

    [Button("Create")]
    private async Task Combine()
    {
        if (targetFolder == null)
        {
            Debug.LogError("Target folder is not assigned!", this);
            return;
        }

        string folderPath = AssetDatabase.GetAssetPath(targetFolder);
        if (!Directory.Exists(folderPath))
        {
            Debug.LogError($"Invalid folder path: {folderPath}", this);
            return;
        }

        string[] csFiles = Directory.GetFiles(folderPath, "*.cs", SearchOption.AllDirectories);

        var combinedContent = new StringBuilder();

        var c = 0;

        foreach (string filePath in csFiles.OrderBy(f => f))
        {
            try
            {
                string content = File.ReadAllText(filePath, Encoding.UTF8);
                string insideNamespaces = ExtractContentInsideNamespaces(content);

                if (!string.IsNullOrWhiteSpace(insideNamespaces))
                    combinedContent.AppendLine(insideNamespaces);
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"Failed to process file: {filePath}\n{ex.Message}", this);
            }

            if (++c <= 100)
                continue;
            await Task.Yield();
            c = 0;
        }

        string resourcesPath = Path.Combine(Application.dataPath, "Resources");
        if (!Directory.Exists(resourcesPath))
            Directory.CreateDirectory(resourcesPath);

        string outputPath = Path.Combine(resourcesPath, $"{outputFileName}.txt");
        File.WriteAllText(outputPath, combinedContent.ToString(), Encoding.UTF8);

        AssetDatabase.Refresh();
        Debug.Log($"✅ Combined {csFiles.Length} C# files into:\n{outputPath}", this);
    }

    private static string ExtractContentInsideNamespaces(string source)
    {
        var lines = source.Replace("\r\n", "\n").Split('\n');
        var resultLines = new List<string>();
        bool insideNamespace = false;

        foreach (string line in lines)
        {
            string trimmed = line.Trim();

            // Пропускаем пустые строки и using'и на верхнем уровне
            if (!insideNamespace && (trimmed.StartsWith("using ") || string.IsNullOrEmpty(trimmed)))
                continue;

            // Обнаруживаем начало namespace
            if (!insideNamespace && trimmed.StartsWith("namespace "))
            {
                insideNamespace = true;
                continue;
            }

            resultLines.Add(line);
        }

        return string.Join("\n", resultLines);
    }
}
#endif