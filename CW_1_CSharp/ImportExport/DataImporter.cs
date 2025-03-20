using System;
using CW_1_CSharp.Proxies;

namespace CW_1_CSharp.ImportExport
{
    public abstract class DataImporter
    {
        public void ImportData(string filePath, DataProxy dataProxy)
        {
            // 1) Считать сырой контент
            var rawData = ReadFile(filePath);
            // 2) Спарсить
            var parsed = ParseData(rawData);
            // 3) (опционально) валидировать / очистить
            var validated = ValidateData(parsed);
            // 4) Сохранить в систему (через dataProxy)
            StoreData(validated, dataProxy);
        }

        protected virtual string ReadFile(string filePath)
        {
            return System.IO.File.ReadAllText(filePath);
        }

        protected abstract object ParseData(string rawData);

        protected virtual object ValidateData(object parsedData)
        {
            // По необходимости добавить проверку.
            return parsedData;
        }

        protected abstract void StoreData(object data, DataProxy dataProxy);
    }
}