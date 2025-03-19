using System.Collections.Generic;

namespace Vashta.CastlesOfWar.ScriptableObject
{
    public class ScriptableDictionary<T> : UnityEngine.ScriptableObject where T : ScriptableObjectWithID
    {
        public T[] Directory;
        protected Dictionary<string, T> Dictionary;
        protected Dictionary<ushort, T> DictionarySession;
        
        void OnEnable()
        {
            CreateDictionaryIfDoesNotExist();
        }

        private void CreateDictionaryIfDoesNotExist()
        {
            if (Dictionary != null) 
                return;
            
            Dictionary = new Dictionary<string, T>();
            DictionarySession = new Dictionary<ushort, T>();

            // 0 needs to be null so that network data can be unsigned
            ushort i = 1;
            foreach (T t in Directory)
            {
                t.SessionId = i;
                Dictionary.Add(t.Id,t);
                DictionarySession.Add(i, t);
                i++;
            }
        }
        
        public T this[string key]
        {
            get
            {
                CreateDictionaryIfDoesNotExist();
                return Dictionary.TryGetValue(key, out var value) ? value : null;
            }
        }

        public T GetBySessionId(ushort key)
        {
            CreateDictionaryIfDoesNotExist();
            return DictionarySession.TryGetValue(key, out var value) ? value : null;
        }

        public ushort GetSessionId(T data)
        {
            CreateDictionaryIfDoesNotExist();
            
            return Dictionary.TryGetValue(data.Id, out var value) ? value.SessionId : (ushort)0;
        }
    }
}