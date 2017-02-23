using FastMember;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web.SessionState;

namespace Microsoft.Web.Redis
{
    /// <summary>
    /// The current SessionStateItemCollection has a performance issue on its Remove method
    /// The following class wraps an SessionStateItemCollection because it is a sealed class
    /// and patches the current remove implemention
    /// </summary>
    internal class SessionStateItemCollectionWrapper : ISessionStateItemCollection, ICollection
    {
        OrderedDictionary old = new OrderedDictionary();
        bool dirty;
        SessionStateItemCollection old2 = new SessionStateItemCollection();
        //object serializedItemsLock;
        //private object _serializedItems;
        internal SessionStateItemCollectionWrapper()
        {
            /*var test = TypeAccessor.Create(typeof(SessionStateItemCollection), true);
            serializedItemsLock = test[old2, "_serializedItemsLock"];
            _serializedItems = test[old2, "_serializedItems"];
            var _readonly = test[old2, "_readOnly"];
            test[]*/

        }

        public object this[int index]
        {
            get
            {
                dirty = true;
                return old[index];
            }

            set
            {
                dirty = true;
                old[index] = value;
                //throw new NotImplementedException();
            }
        }

    /*    public class MyHashTableKeyWrapper : NameObjectCollectionBase.KeysCollection
        {

        }*/


        public NameObjectCollectionBase.KeysCollection Keys {
            get
            {
               var res=  new NameValueCollection();
                foreach(var key in old.Keys)
                {
                    res.Add(key.ToString(), String.Empty);
                }
                return res.Keys;
            }
        }


        public object this[string name]
        {
            get
            {
                dirty = true;
                return old[name];
            }

            set
            {
                dirty = true;
                old[name] = value;
            }
        }

        public int Count
        {
            get
            {
                //throw new NotImplementedException();
                return old.Count;
            }
        }

        public bool Dirty
        {
            get
            {
                return dirty;
            }

            set
            {
                dirty = value;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public object SyncRoot
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Clear()
        {
            dirty = true;
            old.Clear();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void Remove(string name)
        {
            old.Remove(name);
        }

        


        public void RemoveAt(int index)
        {
            old.RemoveAt(index);
        }
    }
}
