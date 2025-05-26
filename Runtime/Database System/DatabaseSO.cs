using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CustomTools.DatabaseSystem
{
    public class DatabaseSO<T> : ScriptableObject where T : ScriptableObject, IDatabaseRecord
    {
        public List<T> records = new List<T>();

        public T GetRecordByID(string id)
        {
            if (string.IsNullOrEmpty(id))
                return default;

            foreach (T databaseRecord in records)
            {
                if(databaseRecord == null)
                    continue;

                if (databaseRecord.ID == id)
                    return databaseRecord;
            }

            Debug.LogError($"Database Record with ID '{id}' was not found.  There are {records.Count} records.  Ensure the record has been added to the database.");
            
            return default;
        }

        public virtual void UpdateDatabase()
        {
            RemoveEmptyOrNullRecords();
            MarkAsDirty();
        }

        private void MarkAsDirty()
        {
            #if UNITY_EDITOR

            foreach (T databaseRecord in records)
            {
                EditorUtility.SetDirty(databaseRecord as Object);
            }
            
            EditorUtility.SetDirty(this);
            #endif
        }

        private void RemoveEmptyOrNullRecords()
        {
            for (int i = records.Count - 1; i >= 0; i--)
            {
                if (records[i] == null)
                {
                    records.RemoveAt(i);
                }
            }
        }

        private bool CheckForDuplicateIDs(T record)
        {
            int idCount = 0;

            foreach (T databaseRecord in records)
            {
                if (databaseRecord.ID == record.ID)
                    idCount++;

                if (idCount > 1)
                    return true;
            }

            return false;
        }
        
        public virtual void AddAllToDatabase()
        {
            T[] baseArray = GetAllInstances<T>();

            foreach (T record in baseArray)
            {
                if(records.Contains(record))
                    continue;

                if (CheckForDuplicateIDs(record))
                    continue;
                
                records.Add(record);
            }
            
            UpdateDatabase();
        }
        
        protected static void AddAllToDatabase<V, U>() where V : DatabaseSO<U> where U : ScriptableObject, IDatabaseRecord
        {
            V mainDatabase = GatMainInstance<V>();

            if (mainDatabase == null)
                return;

            U[] baseArray = GetAllInstances<U>();

            foreach (U record in baseArray)
            {
                if(mainDatabase.records.Contains(record))
                    continue;

                if (mainDatabase.CheckForDuplicateIDs(record))
                    continue;
                
                mainDatabase.records.Add(record);
            }
            
            mainDatabase.UpdateDatabase();
        }

        private static V GatMainInstance<V>() where V : ScriptableObject
        {
            V[] instanceArray = GetAllInstances<V>();

            if (instanceArray.Length == 0)
            {
                Debug.LogError("No Database Was Found");
                return null;
            }

            //Returning the first one found
            return instanceArray[0];
        }

        private static V[] GetAllInstances<V>() where V : ScriptableObject
        {
#if UNITY_EDITOR
                string[] guids = AssetDatabase.FindAssets($"t:{typeof(V).Name}");
                List<V> allInstances = new List<V>();

                foreach (string guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    V temp = AssetDatabase.LoadAssetAtPath<V>(path);
                    allInstances.Add(temp);
                }

                return allInstances.ToArray();
#else
                return null;
#endif
        }
    }

    public interface IDatabaseRecord
    {
        public string ID { get; }
    }
}