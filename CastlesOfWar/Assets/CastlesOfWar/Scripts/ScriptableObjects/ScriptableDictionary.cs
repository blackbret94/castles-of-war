using System.Collections.Generic;

namespace Vashta.CastlesOfWar.ScriptableObject
{
    public class ScriptableDictionary<T> : UnityEngine.ScriptableObject where T : ScriptableObjectWithID
    {
        public T[] Directory;
        private Dictionary<string, T> _dictionary;
        private Dictionary<ushort, T> _dictionarySession;
        
        void OnEnable()
        {
            CreateDictionaryIfDoesNotExist();
        }

        private void CreateDictionaryIfDoesNotExist()
        {
            if (_dictionary != null) 
                return;
            
            _dictionary = new Dictionary<string, T>();
            _dictionarySession = new Dictionary<ushort, T>();

            // 0 needs to be null so that network data can be unsigned
            ushort i = 1;
            foreach (T t in Directory)
            {
                t.SessionId = i;
                _dictionary.Add(t.Id,t);
                _dictionarySession.Add(i, t);
                i++;
            }
        }
        
        public T this[string key]
        {
            get
            {
                CreateDictionaryIfDoesNotExist();
                return _dictionary.TryGetValue(key, out var value) ? value : null;
            }
        }

        public T GetBySessionId(ushort key)
        {
            CreateDictionaryIfDoesNotExist();
            return _dictionarySession.TryGetValue(key, out var value) ? value : null;
        }

        public ushort GetSessionId(T data)
        {
            CreateDictionaryIfDoesNotExist();
            
            return _dictionary.TryGetValue(data.Id, out var value) ? value.SessionId : (ushort)0;
        }
    }
}