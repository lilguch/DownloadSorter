using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DownloadsSorter
{
    public class DownloadOrganizer
    {
        private FileSystemWatcher _fileWatcher;
        private NotifyIcon _notifyIcon;
        public bool IsWatching { get; private set; }
        private ConcurrentDictionary<string, CancellationTokenSource> _debounceTokens = new ConcurrentDictionary<string, CancellationTokenSource>();

        public DownloadOrganizer(NotifyIcon notifyIcon)
        {
            _notifyIcon = notifyIcon ?? throw new ArgumentNullException(nameof(notifyIcon));
        }

        public void StartWatching()
        {
            string downloadsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Downloads");

            _fileWatcher = new FileSystemWatcher
            {
                Path = downloadsPath,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Size,
                Filter = "*.*",
                IncludeSubdirectories = false,
                EnableRaisingEvents = true
            };

            _fileWatcher.Created += OnFileEvent;
            _fileWatcher.Changed += OnFileEvent;
            _fileWatcher.Renamed += OnFileRenamed;
            IsWatching = true;
        }

        private void OnFileEvent(object sender, FileSystemEventArgs e)
        {
            if (IsTemporaryFile(e.FullPath) || IsSystemFile(Path.GetFileName(e.FullPath)))
                return;

            ScheduleProcess(e.FullPath);
        }

        private void OnFileRenamed(object sender, RenamedEventArgs e)
        {
            if (IsTemporaryFile(e.FullPath) || IsSystemFile(Path.GetFileName(e.FullPath)))
                return;

            ScheduleProcess(e.FullPath);
        }
        
        private void ScheduleProcess(string filePath)
        {
            if (_debounceTokens.TryRemove(filePath, out var oldCts))
            {
                try { oldCts.Cancel(); oldCts.Dispose(); } catch { }
            }

            var cts = new CancellationTokenSource();
            _debounceTokens[filePath] = cts;
            var token = cts.Token;

            _ = Task.Run(async () =>
            {
                try
                {
                    int initialDelayMs = 1500;
                    try
                    {
                        if (File.Exists(filePath))
                        {
                            var fi = new FileInfo(filePath);
                            if (fi.Length > 50L * 1024 * 1024)
                                initialDelayMs = 5000;
                            else if (fi.Length > 10L * 1024 * 1024)
                                initialDelayMs = 3000;
                        }
                    }
                    catch {  }

                    await Task.Delay(initialDelayMs, token);

                    bool stable = await WaitForStabilityWithoutOpening(filePath, token);
                    if (!stable) return;

                    if (!File.Exists(filePath)) return;
                    var finalExt = Path.GetExtension(filePath).ToLower();
                    if (string.IsNullOrEmpty(finalExt) || IsTemporaryFile(filePath) || IsSystemFile(Path.GetFileName(filePath)))
                        return;

                    OrganizeFile(filePath, finalExt);
                }
                catch (OperationCanceledException) { }
                catch { }
                finally
                {
                    _debounceTokens.TryRemove(filePath, out var _);
                }
            }, token);
        }

        private async Task<bool> WaitForStabilityWithoutOpening(string filePath, CancellationToken token)
        {
            const int maxChecks = 6;
            int checks = 0;
            long lastSize = -1;
            DateTime lastWrite = DateTime.MinValue;

            while (checks < maxChecks)
            {
                if (token.IsCancellationRequested) throw new OperationCanceledException(token);

                if (!File.Exists(filePath))
                    return false;

                try
                {
                    var fi = new FileInfo(filePath);
                    long size = fi.Length;
                    DateTime lw = fi.LastWriteTimeUtc;

                    if (size == lastSize && lw == lastWrite && size > 0)
                    {
                        return true;
                    }

                    lastSize = size;
                    lastWrite = lw;

                    int delay = 700; 
                    if (size > 50L * 1024 * 1024) delay = 3000;
                    else if (size > 10L * 1024 * 1024) delay = 1500;

                    await Task.Delay(delay, token);
                    checks++;
                }
                catch (OperationCanceledException) { throw; }
                catch
                {
                    await Task.Delay(500, token);
                }
            }

            return false;
        }

        public void StopWatching()
        {
            _fileWatcher?.Dispose();
            IsWatching = false;
        }

        public void GenerateStandartFile()
        {
            var defaultConfig = new SorterConfig
            {
                Rules = new List<SorterRule>
        {
            new SorterRule { Extension = ".pdf", Folder = "Документы" },
        new SorterRule { Extension = ".doc", Folder = "Документы" },
        new SorterRule { Extension = ".docx", Folder = "Документы" },
        new SorterRule { Extension = ".odt", Folder = "Документы" },
        new SorterRule { Extension = ".xls", Folder = "Документы" },
        new SorterRule { Extension = ".xlsx", Folder = "Документы" },
        new SorterRule { Extension = ".ods", Folder = "Документы" },
        new SorterRule { Extension = ".ppt", Folder = "Документы" },
        new SorterRule { Extension = ".pptx", Folder = "Документы" },
        new SorterRule { Extension = ".odp", Folder = "Документы" },
        new SorterRule { Extension = ".txt", Folder = "Документы" },
        new SorterRule { Extension = ".rtf", Folder = "Документы" },
        new SorterRule { Extension = ".csv", Folder = "Документы" },
        new SorterRule { Extension = ".xml", Folder = "Документы" },
        new SorterRule { Extension = ".json", Folder = "Документы" },
        new SorterRule { Extension = ".md", Folder = "Документы" },
        new SorterRule { Extension = ".tex", Folder = "Документы" },
        new SorterRule { Extension = ".log", Folder = "Документы" },
        new SorterRule { Extension = ".ini", Folder = "Документы" },

        new SorterRule { Extension = ".jpg", Folder = "Изображения" },
        new SorterRule { Extension = ".jpeg", Folder = "Изображения" },
        new SorterRule { Extension = ".png", Folder = "Изображения" },
        new SorterRule { Extension = ".gif", Folder = "Изображения" },
        new SorterRule { Extension = ".bmp", Folder = "Изображения" },
        new SorterRule { Extension = ".svg", Folder = "Изображения" },
        new SorterRule { Extension = ".webp", Folder = "Изображения" },
        new SorterRule { Extension = ".tiff", Folder = "Изображения" },
        new SorterRule { Extension = ".tif", Folder = "Изображения" },
        new SorterRule { Extension = ".ico", Folder = "Изображения" },
        new SorterRule { Extension = ".heic", Folder = "Изображения" },
        new SorterRule { Extension = ".raw", Folder = "Изображения" },
        new SorterRule { Extension = ".cr2", Folder = "Изображения" },
        new SorterRule { Extension = ".nef", Folder = "Изображения" },

        new SorterRule { Extension = ".mp4", Folder = "Видео" },
        new SorterRule { Extension = ".avi", Folder = "Видео" },
        new SorterRule { Extension = ".mkv", Folder = "Видео" },
        new SorterRule { Extension = ".mov", Folder = "Видео" },
        new SorterRule { Extension = ".wmv", Folder = "Видео" },
        new SorterRule { Extension = ".flv", Folder = "Видео" },
        new SorterRule { Extension = ".webm", Folder = "Видео" },
        new SorterRule { Extension = ".mpeg", Folder = "Видео" },
        new SorterRule { Extension = ".mpg", Folder = "Видео" },
        new SorterRule { Extension = ".3gp", Folder = "Видео" },
        new SorterRule { Extension = ".ts", Folder = "Видео" },
        new SorterRule { Extension = ".vob", Folder = "Видео" },
        new SorterRule { Extension = ".m4v", Folder = "Видео" },

        new SorterRule { Extension = ".mp3", Folder = "Аудио" },
        new SorterRule { Extension = ".wav", Folder = "Аудио" },
        new SorterRule { Extension = ".flac", Folder = "Аудио" },
        new SorterRule { Extension = ".aac", Folder = "Аудио" },
        new SorterRule { Extension = ".ogg", Folder = "Аудио" },
        new SorterRule { Extension = ".m4a", Folder = "Аудио" },
        new SorterRule { Extension = ".mid", Folder = "Аудио" },
        new SorterRule { Extension = ".wma", Folder = "Аудио" },
        new SorterRule { Extension = ".aiff", Folder = "Аудио" },
        new SorterRule { Extension = ".opus", Folder = "Аудио" },

        new SorterRule { Extension = ".zip", Folder = "Архивы" },
        new SorterRule { Extension = ".rar", Folder = "Архивы" },
        new SorterRule { Extension = ".7z", Folder = "Архивы" },
        new SorterRule { Extension = ".tar", Folder = "Архивы" },
        new SorterRule { Extension = ".gz", Folder = "Архивы" },
        new SorterRule { Extension = ".bz2", Folder = "Архивы" },
        new SorterRule { Extension = ".xz", Folder = "Архивы" },
        new SorterRule { Extension = ".iso", Folder = "Архивы" },
        new SorterRule { Extension = ".cab", Folder = "Архивы" },

        new SorterRule { Extension = ".exe", Folder = "Программы" },
        new SorterRule { Extension = ".msi", Folder = "Программы" },
        new SorterRule { Extension = ".dmg", Folder = "Программы" },
        new SorterRule { Extension = ".apk", Folder = "Программы" },
        new SorterRule { Extension = ".bat", Folder = "Программы" },
        new SorterRule { Extension = ".cmd", Folder = "Программы" },
        new SorterRule { Extension = ".sh", Folder = "Программы" },
        new SorterRule { Extension = ".jar", Folder = "Программы" },
        new SorterRule { Extension = ".ps1", Folder = "Программы" },

        new SorterRule { Extension = ".psd", Folder = "Графика" },
        new SorterRule { Extension = ".ai", Folder = "Графика" },
        new SorterRule { Extension = ".eps", Folder = "Графика" },
        new SorterRule { Extension = ".indd", Folder = "Графика" },
        new SorterRule { Extension = ".dwg", Folder = "3D и CAD" },
        new SorterRule { Extension = ".dxf", Folder = "3D и CAD" },
        new SorterRule { Extension = ".stl", Folder = "3D и CAD" },
        new SorterRule { Extension = ".obj", Folder = "3D и CAD" },
        new SorterRule { Extension = ".blend", Folder = "3D и CAD" },

        new SorterRule { Extension = ".cs", Folder = "Код" },
        new SorterRule { Extension = ".cpp", Folder = "Код" },
        new SorterRule { Extension = ".h", Folder = "Код" },
        new SorterRule { Extension = ".java", Folder = "Код" },
        new SorterRule { Extension = ".py", Folder = "Код" },
        new SorterRule { Extension = ".js", Folder = "Код" },
        new SorterRule { Extension = ".ts", Folder = "Код" },
        new SorterRule { Extension = ".html", Folder = "Код" },
        new SorterRule { Extension = ".css", Folder = "Код" },
        new SorterRule { Extension = ".php", Folder = "Код" },
        new SorterRule { Extension = ".sql", Folder = "Код" },
        new SorterRule { Extension = ".json5", Folder = "Код" },
        new SorterRule { Extension = ".vue", Folder = "Код" },
        new SorterRule { Extension = ".xml", Folder = "Код" },
        new SorterRule { Extension = ".ini", Folder = "Код" },

        new SorterRule { Extension = ".torrent", Folder = "Торренты" },

        new SorterRule { Extension = ".ttf", Folder = "Шрифты" },
        new SorterRule { Extension = ".otf", Folder = "Шрифты" },
        new SorterRule { Extension = ".woff", Folder = "Шрифты" },
        new SorterRule { Extension = ".woff2", Folder = "Шрифты" },

        new SorterRule { Extension = ".bak", Folder = "Резервные копии" },
        new SorterRule { Extension = ".tmp", Folder = "Временные файлы" },
        new SorterRule { Extension = ".crdownload", Folder = "Временные файлы" },
        new SorterRule { Extension = ".part", Folder = "Временные файлы" },
        new SorterRule { Extension = ".download", Folder = "Временные файлы" }
    }
            };

            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(defaultConfig, options);
            File.WriteAllText("rules.json", json);
        }

        private void OrganizeFile(string filePath, string fileExtension)
        {
            try
            {
                string fileName = Path.GetFileName(filePath);

                if (ShouldSkipFile(filePath, fileName))
                    return;

                string category = GetFileCategory(fileExtension);
                string destinationFolder = Path.Combine(Path.GetDirectoryName(filePath), category);

                Directory.CreateDirectory(destinationFolder);

                string newPath = Path.Combine(destinationFolder, fileName);

                File.Move(filePath, newPath);

                ShowNotification("Файл перемещен", $"{fileName} → {category}");
            }
            catch (Exception ex)
            {
                if (!IsTemporaryFile(filePath))
                {
                    ShowNotification("Ошибка перемещения", ex.Message, true);
                }
            }
        }

        private bool ShouldSkipFile(string filePath, string fileName)
        {
            return IsTemporaryFile(filePath) || IsSystemFile(fileName);
        }

        private bool IsFileReadyForMove(string filePath)
        {
            return !IsTemporaryFile(filePath);
        }

        private bool IsTemporaryFile(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            string fileName = Path.GetFileName(filePath).ToLower();

            string[] tempExtensions = {
        ".tmp", ".temp", ".crdownload", ".part", ".download",
        ".partial", ".!ut", ".ut!", ".bc!", ".opdownload"
    };

            string[] tempPatterns = {
        "~", "temp_", "tmp_", ".tmp.",
        "crs-", "pending", "incomplete"
    };

            if (tempExtensions.Contains(extension))
                return true;

            if (tempPatterns.Any(pattern => fileName.Contains(pattern)))
                return true;

            try
            {
                var attributes = File.GetAttributes(filePath);
                if ((attributes & FileAttributes.Temporary) == FileAttributes.Temporary ||
                    (attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                {
                    return true;
                }
            }
            catch
            {
            }

            return false;
        }

        private bool IsSystemFile(string fileName)
        {
            string[] systemFiles = { "desktop.ini", "thumbs.db", ".ds_store" };
            return systemFiles.Contains(fileName.ToLower());
        }


        private void ShowNotification(string title, string message, bool isError = false)
        {
            if (_notifyIcon != null)
            {
                _notifyIcon.BalloonTipTitle = title;
                _notifyIcon.BalloonTipText = message;
                _notifyIcon.BalloonTipIcon = isError ? ToolTipIcon.Error : ToolTipIcon.Info;
                _notifyIcon.ShowBalloonTip(3000);
            }
        }

        private string GetFileCategory(string extension)
        {
            try
            {
                var config = JsonSerializer.Deserialize<SorterConfig>(File.ReadAllText("rules.json"));
                return config.Rules.FirstOrDefault(r => r.Extension.Equals(extension, StringComparison.OrdinalIgnoreCase))?.Folder ?? "Прочее";
            }
            catch
            {
                return "Прочее";
            }
        }

        public void SortExistingFiles()
        {
            string downloadsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Downloads");

            try
            {
                var files = Directory.GetFiles(downloadsPath, "*.*", SearchOption.TopDirectoryOnly);

                int processedCount = 0;
                int skippedCount = 0;
                int totalCount = files.Length;

                foreach (var filePath in files)
                {
                    try
                    {
                        string fileName = Path.GetFileName(filePath);
                        string fileExtension = Path.GetExtension(filePath).ToLower();

                        if (string.IsNullOrEmpty(fileExtension) || ShouldSkipFile(filePath, fileName))
                        {
                            skippedCount++;
                            continue;
                        }

                        OrganizeFile(filePath, fileExtension);
                        processedCount++;
                    }
                    catch (Exception ex)
                    {
                        if (!IsTemporaryFile(filePath))
                        {
                            ShowNotification("Ошибка сортировки",
                                $"Файл {Path.GetFileName(filePath)}: {ex.Message}", true);
                        }
                    }
                }

                ShowNotification("Сортировка завершена",
                    $"Обработано: {processedCount}, Пропущено: {skippedCount}, Всего: {totalCount}");
            }
            catch (Exception ex)
            {
                ShowNotification("Ошибка",
                    $"Не удалось отсортировать старые файлы: {ex.Message}", true);
            }
        }

        public event Action<int, int> UpdateProgress;
    }
}
