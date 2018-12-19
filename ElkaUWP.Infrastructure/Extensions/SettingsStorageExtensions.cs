using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using ElkaUWP.Infrastructure.Helpers;

namespace ElkaUWP.Infrastructure.Extensions
{
    public static class SettingsStorageExtensions
    {
        private const string FileExtension = ".json";

        public static bool IsRoamingStorageAvailable(this ApplicationData appData)
        {
            return appData.RoamingStorageQuota == 0;
        }

        public static async Task SaveAsync<T>(this StorageFolder folder, string name, T content)
        {
            var file = await folder.CreateFileAsync(desiredName: GetFileName(name: name), options: CreationCollisionOption.ReplaceExisting);
            var fileContent = await JsonHelper.Json.ToJsonAsync(value: content);

            await FileIO.WriteTextAsync(file: file, contents: fileContent);
        }

        public static async Task<T> ReadAsync<T>(this StorageFolder folder, string name)
        {
            if (!File.Exists(path: Path.Combine(path1: folder.Path, path2: GetFileName(name: name))))
            {
                return default(T);
            }

            var file = await folder.GetFileAsync(name: $"{name}.json");
            var fileContent = await FileIO.ReadTextAsync(file: file);

            return await JsonHelper.Json.FromJsonAsync<T>(value: fileContent);
        }

        public static async Task SaveAsync<T>(this ApplicationDataContainer settings, string key, T value)
        {
            settings.SaveString(key, await JsonHelper.Json.ToJsonAsync(value: value));
        }

        public static void SaveString(this ApplicationDataContainer settings, string key, string value)
        {
            settings.Values[key: key] = value;
        }

        public static async Task<T> ReadAsync<T>(this ApplicationDataContainer settings, string key)
        {
            object obj = null;

            if (settings.Values.TryGetValue(key: key, value: out obj))
            {
                return await JsonHelper.Json.FromJsonAsync<T>(value: (string)obj);
            }

            return default(T);
        }

        public static async Task<StorageFile> SaveFileAsync(this StorageFolder folder, byte[] content, string fileName, CreationCollisionOption options = CreationCollisionOption.ReplaceExisting)
        {
            if (content == null)
            {
                throw new ArgumentNullException(paramName: nameof(content));
            }

            if (string.IsNullOrEmpty(value: fileName))
            {
                throw new ArgumentException(message: "ExceptionSettingsStorageExtensionsFileNameIsNullOrEmpty", paramName: nameof(fileName));
            }

            var storageFile = await folder.CreateFileAsync(desiredName: fileName, options: options);
            await FileIO.WriteBytesAsync(file: storageFile, buffer: content);
            return storageFile;
        }

        public static async Task<byte[]> ReadFileAsync(this StorageFolder folder, string fileName)
        {
            var item = await folder.TryGetItemAsync(name: fileName).AsTask().ConfigureAwait(continueOnCapturedContext: false);

            if ((item == null) || !item.IsOfType(type: StorageItemTypes.File))
                return null;

            var storageFile = await folder.GetFileAsync(name: fileName);
            byte[] content = await storageFile.ReadBytesAsync();
            return content;

        }

        public static async Task<byte[]> ReadBytesAsync(this StorageFile file)
        {
            if (file == null)
                return null;

            using (IRandomAccessStream stream = await file.OpenReadAsync())
            {
                using (var reader = new DataReader(inputStream: stream.GetInputStreamAt(position: 0)))
                {
                    await reader.LoadAsync(count: (uint)stream.Size);
                    var bytes = new byte[stream.Size];
                    reader.ReadBytes(value: bytes);
                    return bytes;
                }
            }

        }

        private static string GetFileName(string name)
        {
            return string.Concat(str0: name, str1: FileExtension);
        }
    }
}

