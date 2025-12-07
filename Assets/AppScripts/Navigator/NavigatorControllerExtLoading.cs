using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using Vortex.Core.AppSystem.Bus;
using Vortex.Core.LoaderSystem.Bus;
using Vortex.Core.System.Enums;
using Vortex.Core.System.ProcessInfo;

namespace AppScripts.Navigator
{
    public partial class NavigatorController : IProcess
    {
        private static event Action _onInit;

        public static event Action OnInit
        {
            add
            {
                if (isInitialized)
                {
                    value.Invoke();
                    return;
                }

                _onInit += value;
            }
            remove => _onInit -= value;
        }

        /// <summary>
        /// Название основного файла данных
        /// </summary>
        private const string Filename = "pages.txt";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Run() => Loader.Register<NavigatorController>();

        private static bool isInitialized = false;

        private ProcessData _processData = new()
        {
            Name = "Navigator",
            Progress = 0,
            Size = 100
        };

        public ProcessData GetProcessInfo() => _processData;

        private static string ConvertCP1251BytesToUTF8String(byte[] cp1251Bytes)
        {
            var cp1251 = Encoding.GetEncoding(1251);
            var unicodeString = cp1251.GetString(cp1251Bytes);
            return unicodeString;
        }

        /// <summary>
        /// Загрузка данных из StreamingAssets
        /// </summary>
        public async Task RunAsync(CancellationToken cancellationToken)
        {
            isInitialized = false;
            _pages.Clear();
            homePage = "";
            try
            {
                var path = Application.streamingAssetsPath;

                var pagesRaw = new List<string[]>();
                var fileBytes = await File.ReadAllBytesAsync(Path.Combine(path, Filename), cancellationToken);
                var data = ConvertCP1251BytesToUTF8String(fileBytes).Split("\t<end>");

                var c = data.Length;
                for (var i = 1; i < c; i++)
                    pagesRaw.Add(data[i].Split('\t'));

                if (pagesRaw.Count == 0)
                {
                    Debug.LogError($"Error reading datafile {Filename}");
                    await Task.CompletedTask;
                    return;
                }

                _processData.Size = pagesRaw.Count;

                var photos = new List<byte[]>();
                foreach (var pageRaw in pagesRaw)
                {
                    if (cancellationToken.IsCancellationRequested)
                        return;
                    if (pageRaw.Length != 5)
                    {
                        //Частный случай - пустая строка. Не хорошо, но не стоит из-за этого возмущаться
                        if (pageRaw.Length == 0 || pageRaw[0] == "\r\n")
                            continue;
                        Debug.LogError(
                            $"Error reading datafile {Filename}. Wrong size on data cell ({pageRaw.Length})");
                        break;
                    }

                    var id = pageRaw[0].TrimStart('\r', '\n');
                    if (homePage.IsNullOrWhitespace())
                        homePage = id;
                    var name = pageRaw[1];
                    var backBtn = pageRaw[2];
                    var content = pageRaw[3].Trim('\"').Split("<br>");
                    var photoWidthRaw = pageRaw[4];
                    var photoWidth = 920;
                    if (!photoWidthRaw.IsNullOrWhitespace())
                        photoWidth = int.Parse(photoWidthRaw);

                    if (_pages.ContainsKey(id))
                    {
                        Debug.LogError($"Error reading data. Duplicate page id: #{id}");
                        continue;
                    }

                    if (!backBtn.IsNullOrWhitespace())
                        if (_pages.ContainsKey(backBtn))
                            _pages[backBtn].SetLink(id);
                        else
                            Debug.LogError(
                                $"There is no page with name {backBtn} for make link with {name} in this point of loading process");

                    if (content.Length == 1 && content[0].IsNullOrWhitespace())
                        content = Array.Empty<string>();

                    photos.Clear();
                    var number = 1;
                    while (true)
                    {
                        var fileName = Path.Combine(path, $"{id}_{number++}.png");
                        if (!File.Exists(fileName))
                            break;

                        var fileData = await File.ReadAllBytesAsync(fileName, cancellationToken);
                        photos.Add(fileData);
                    }

                    NavigatorPage page;
                    var schemeFileName = Path.Combine(path, $"{id}_sch.png");
                    if (File.Exists(schemeFileName))
                    {
                        var scheme = await File.ReadAllBytesAsync(schemeFileName, cancellationToken);
                        page = new NavigatorPage(id, name, backBtn, photos.ToArray(), scheme, photoWidth);
                    }
                    else page = new NavigatorPage(id, name, backBtn, photos.ToArray(), content, photoWidth);

                    _pages.Add(id, page);
                    _processData.Progress++;
                    await Task.Yield();
                }

                isInitialized = true;
                _onInit?.Invoke();
                _onInit = null;
                Debug.Log($"Loading assets complete. Loaded {_pages.Count} pages.");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            await Task.CompletedTask;
        }

        public Type[] WaitingFor() => null;

#if UNITY_EDITOR

        /// <summary>
        /// Тест-Загрузка данных из StreamingAssets
        /// </summary>
        [MenuItem("Encore/Loading Assets")]
        private static void EditorLoadAssets()
        {
            EditorLoad();
        }

        /// <summary>
        /// Загрузка данных из StreamingAssets в режиме редактора (не асинхронная)
        /// </summary>
        private static void EditorLoad()
        {
            _pages.Clear();
            try
            {
                var path = Application.streamingAssetsPath;

                var pagesRaw = new List<string[]>();
                var fileBytes = File.ReadAllBytes(Path.Combine(path, Filename));
                var data = ConvertCP1251BytesToUTF8String(fileBytes).Split("\t<end>");

                var c = data.Length;
                for (var i = 1; i < c; i++)
                    pagesRaw.Add(data[i].Split('\t'));

                if (pagesRaw.Count == 0)
                {
                    Debug.LogError($"Error reading datafile {Filename}");
                    return;
                }

                var photos = new List<byte[]>();
                foreach (var pageRaw in pagesRaw)
                {
                    if (pageRaw.Length != 5)
                    {
                        //Частный случай - пустая строка. Не хорошо, но не стоит из-за этого возмущаться
                        if (pageRaw.Length == 0 || pageRaw[0] == "\r\n")
                            continue;
                        Debug.LogError(
                            $"Error reading datafile {Filename}. Wrong size on data cell ({pageRaw.Length})");
                        break;
                    }

                    var id = pageRaw[0].TrimStart('\r', '\n');
                    var name = pageRaw[1];
                    var backBtn = pageRaw[2];
                    var content = pageRaw[3].Split("<br>");
                    var photoWidthRaw = pageRaw[4];
                    var photoWidth = 920;
                    if (!photoWidthRaw.IsNullOrWhitespace())
                        photoWidth = int.Parse(photoWidthRaw);

                    if (_pages.ContainsKey(id))
                    {
                        Debug.LogError($"Error reading data. Duplicate page id: #{id}");
                        continue;
                    }

                    if (!backBtn.IsNullOrWhitespace())
                        if (_pages.ContainsKey(backBtn))
                            _pages[backBtn].SetLink(id);
                        else
                            Debug.LogError(
                                $"There is no page with name {backBtn} for make link with {name} in this point of loading process");

                    if (content.Length == 1 && content[0].IsNullOrWhitespace())
                        content = Array.Empty<string>();

                    photos.Clear();
                    var number = 1;
                    while (true)
                    {
                        var fileName = Path.Combine(path, $"{id}_{number++}.png");
                        if (!File.Exists(fileName))
                            break;

                        var fileData = File.ReadAllBytes(fileName);
                        photos.Add(fileData);
                    }

                    NavigatorPage page;
                    var schemeFileName = Path.Combine(path, $"{id}_sch.png");
                    if (File.Exists(schemeFileName))
                    {
                        var scheme = File.ReadAllBytes(schemeFileName);
                        page = new NavigatorPage(id, name, backBtn, photos.ToArray(), scheme, photoWidth);
                    }
                    else page = new NavigatorPage(id, name, backBtn, photos.ToArray(), content, photoWidth);

                    _pages.Add(id, page);
                }

                Debug.Log($"Loading assets complete. Loaded {_pages.Count} pages.");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
#endif
    }
}