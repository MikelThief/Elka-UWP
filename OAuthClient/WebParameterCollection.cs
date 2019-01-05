using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace OAuthClient
{
    public class WebParameterCollection : IList<WebParameter>
    {
        private IList<WebParameter> _parameters;

        public virtual WebParameter this[string name]
        {
            get
            {
                var parameters = this.Where(p => p.Name.Equals(value: name));

                if (parameters.Count() == 0)
                {
                    return null;
                }

                if (parameters.Count() == 1)
                {
                    return parameters.Single();
                }

                var value = string.Join(",", value: parameters.Select(p => p.Value).ToArray());
                return new WebParameter(name: name, value: value);
            }
        }

        public virtual IEnumerable<string> Names
        {
            get { return _parameters.Select(p => p.Name); }
        }

        public virtual IEnumerable<string> Values
        {
            get { return _parameters.Select(p => p.Value); }
        }

        public WebParameterCollection(IEnumerable<WebParameter> parameters)
        {
            _parameters = new List<WebParameter>(collection: parameters);
        }

#if !WINRT
        public WebParameterCollection(NameValueCollection collection) : this()
        {
            AddCollection(collection: collection);
        }

        public virtual void AddRange(NameValueCollection collection)
        {
            AddCollection(collection: collection);
        }

        private void AddCollection(NameValueCollection collection)
        {
            var parameters = collection.AllKeys.Select(key => new WebParameter(name: key, value: collection[name: key]));
            foreach (var parameter in parameters)
            {
                _parameters.Add(item: parameter);
            }
        }
#endif

        public WebParameterCollection(IDictionary<string, string> collection) : this()
        {
            AddCollection(collection: collection);
        }

        public void AddCollection(IDictionary<string, string> collection)
        {
            foreach (var parameter in collection.Keys.Select(key => new WebParameter(name: key, value: collection[key: key])))
            {
                _parameters.Add(item: parameter);
            }
        }

        public WebParameterCollection()
        {
            _parameters = new List<WebParameter>(0);
        }

        public WebParameterCollection(int capacity)
        {
            _parameters = new List<WebParameter>(capacity: capacity);
        }

        private void AddCollection(IEnumerable<WebParameter> collection)
        {
            foreach (var pair in collection.Select(parameter => new WebParameter(name: parameter.Name, value: parameter.Value)))
            {
                _parameters.Add(item: pair);
            }
        }

        public virtual void AddRange(WebParameterCollection collection)
        {
            AddCollection(collection: collection);
        }

        public virtual void AddRange(IEnumerable<WebParameter> collection)
        {
            AddCollection(collection: collection);
        }

        public virtual void Sort(Comparison<WebParameter> comparison)
        {
            var sorted = new List<WebParameter>(collection: _parameters);
            sorted.Sort(comparison: comparison);
            _parameters = sorted;
        }

        public virtual bool RemoveAll(IEnumerable<WebParameter> parameters)
        {
            var array = parameters.ToArray();
            var success = array.Aggregate(true, (current, parameter) => current & _parameters.Remove(item: parameter));
            return success && array.Length > 0;
        }

        public virtual void Add(string name, string value)
        {
            var pair = new WebParameter(name: name, value: value);
            _parameters.Add(item: pair);
        }

        #region IList<WebParameter> Members

        public virtual IEnumerator<WebParameter> GetEnumerator()
        {
            return _parameters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public virtual void Add(WebParameter parameter)
        {

            _parameters.Add(item: parameter);
        }

        public virtual void Clear()
        {
            _parameters.Clear();
        }

        public virtual bool Contains(WebParameter parameter)
        {
            return _parameters.Contains(item: parameter);
        }

        public virtual void CopyTo(WebParameter[] parameters, int arrayIndex)
        {
            _parameters.CopyTo(array: parameters, arrayIndex: arrayIndex);
        }

        public virtual bool Remove(WebParameter parameter)
        {
            return _parameters.Remove(item: parameter);
        }

        public virtual int Count
        {
            get { return _parameters.Count; }
        }

        public virtual bool IsReadOnly
        {
            get { return _parameters.IsReadOnly; }
        }

        public virtual int IndexOf(WebParameter parameter)
        {
            return _parameters.IndexOf(item: parameter);
        }

        public virtual void Insert(int index, WebParameter parameter)
        {
            _parameters.Insert(index: index, item: parameter);
        }

        public virtual void RemoveAt(int index)
        {
            _parameters.RemoveAt(index: index);
        }

        public virtual WebParameter this[int index]
        {
            get { return _parameters[index: index]; }
            set { _parameters[index: index] = value; }
        }

        #endregion
    }
}
