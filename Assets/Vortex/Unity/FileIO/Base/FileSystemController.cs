using System.IO;
#if UNITY_ANDROID && !UNITY_EDITOR
using Vortex.Core.FileSystem.Android;
#endif
using UnityEngine;

namespace Vortex.Core.FileSystem
{
    public static class FileSystemController
    {
        /// <summary>
        /// Путь к основному контейнеру для хранения файлов на устройстве
        /// </summary>
        private static string path;

        [RuntimeInitializeOnLoadMethod]
        public static void Init()
        {
            MakeAppPath();
        }

        public static void MakeAppPath()
        {
            var root = Application.dataPath.Split('/', '\\');
            root = root[.. ^1];
            root[0] += Path.DirectorySeparatorChar;
            root[^1] = "_OutputFiles";
            path = Path.Combine(root);
#if UNITY_ANDROID && !UNITY_EDITOR
            path = AndroidPathResolver.GetAndroidPath();
#endif
        }

        /// <summary>
        /// Возвращает основной контейнер для хранения файлов на устройстве
        /// </summary>
        /// <returns></returns>
        public static string GetAppPath()
        {
            if (path == null)
                MakeAppPath();
            return path ??= "";
        }

        /// <summary>
        /// Обработка имени файла и построение пути до файла
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="extraPath">Путь к файлу относительно верхнего уровня хранилища</param>
        /// <returns></returns>
        public static string PrepareFilePath(string fileName, string extraPath = "")
        {
            fileName = fileName.Split('/', '\\')[^1];
            var fileDirectory = Path.Combine(GetAppPath(), extraPath);
            CreateFolders(fileDirectory);
            var filePath = Path.Combine(fileDirectory, fileName);
            return filePath;
        }

        /// <summary>
        /// Создать папки по указанному в переменной пути
        /// </summary>
        /// <param name="directory"></param>
        public static void CreateFolders(string directory)
        {
            if (Directory.Exists(directory))
                return;
            Directory.CreateDirectory(directory);
            Debug.Log($"[FileSystemController] Create folders for root {directory}");
        }

        /// <summary>
        /// Проверяет наличие папки по указанному пути
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool IsFileExist(string filePath)
        {
            return Directory.Exists(filePath);
        }

        public static void DeleteFiles(string folderPath, string extension)
        {
            if (!Directory.Exists(folderPath)) return;
            var files = Directory.GetFiles(folderPath, $"*{extension}");
            foreach (var file in files)
            {
                File.Delete(file);
            }
        }

        public static void DeleteFoldersInDirectory(string folderName)
        {
            var folderPath = Path.Combine(GetAppPath(), folderName);
            if (!Directory.Exists(folderPath))
                return;

            var directories = Directory.GetDirectories(folderPath);
            foreach (var directory in directories)
            {
                var files = Directory.GetFiles(directory);
                foreach (var file in files)
                    File.Delete(file);
                Directory.Delete(directory);
            }
        }
    }
}