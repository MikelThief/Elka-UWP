using System;
using System.IO;
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
                return default;
            }

            var file = await folder.GetFileAsync(name: $"{name}.json");
            var fileContent = await FileIO.ReadTextAsync(file: file);

            return await JsonHelper.Json.FromJsonAsync<T>(value: fileContent);
        }

        public static async Task SaveAsync<T>(this ApplicationDataContainer settings, string key, T value)
        {
            settings.SaveString(key: key, value: await JsonHelper.Json.ToJsonAsync(value: value));
        }

        public static void SaveString(this ApplicationDataContainer settings, string key, string value)
        {
            settings.Values[key: key] = value;
        }
        public static string ReadString(this ApplicationDataContainer settings, string key) => (string)settings.Values[key: key];

        public static async Task<T> ReadAsync<T>(this ApplicationDataContainer settings, string key)
        {

            return settings.Values.TryGetValue(key: key, value: out var obj)
                ? await JsonHelper.Json.FromJsonAsync<T>(value: (string)obj)
                : (default);
        }

        public static async Task<StorageFile> SaveFileAsync(this StorageFolder folder, byte[] content, string fileName, CreationCollisionOption options = CreationCollisionOption.ReplaceExisting)
        {
            if (content is null)
            {
                throw new ArgumentNullException(paramName: nameof(content));
            }

            if (string.IsNullOrEmpty(value: fileName))
            {
                throw new ArgumentException("ExceptionSettingsStorageExtensionsFileNameIsNullOrEmpty", paramName: nameof(fileName));
            }

            var storageFile = await folder.CreateFileAsync(desiredName: fileName, options: options);
            await FileIO.WriteBytesAsync(file: storageFile, buffer: content);
            return storageFile;
        }

        public static async Task<byte[]> ReadFileAsync(this StorageFolder folder, string fileName)
        {
            var item = await folder.TryGetItemAsync(name: fileName).AsTask().ConfigureAwait(false);

            if ((item is null) || !item.IsOfType(type: StorageItemTypes.File))
                return null;

            var storageFile = await folder.GetFileAsync(name: fileName);
            var content = await storageFile.ReadBytesAsync();
            return content;

        }

        public static async Task<byte[]> ReadBytesAsync(this StorageFile file)
        {
            if (file == null)
                return null;

            using (IRandomAccessStream stream = await file.OpenReadAsync())
            {
                using (var reader = new DataReader(inputStream: stream.GetInputStreamAt(0)))
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

