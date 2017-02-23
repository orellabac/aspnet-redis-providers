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
        Hashtable old = new Hashtable();

        public object this[int index]
        {
            get
            {
                throw new NotImplementedException();
                //return old[index];
            }

            set
            {
                //old[index] = value;
                throw new NotImplementedException();
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
               return old[name];
            }

            set
            {
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
                return true;
            }

            set
            {
                //old.Dirty = value;
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
            throw new NotImplementedException();
        }
    }
}
