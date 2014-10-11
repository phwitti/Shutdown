using System;
using System.Collections.Generic;

namespace pw
{
    class ActionDictionary<T>
    {
        private Dictionary<T, Action> m_dictionary = new Dictionary<T,Action>();

        //

        /// <summary>
        /// Adds the specified key and action to the dictionary.
        /// </summary>
        /// <param name="_key">The key of the element to add.</param>
        /// <param name="_action">The action of the element to add.</param>
        /// <exception cref="System.ArgumentNullException">Thrown, if the _key or _action parameter is null.</exception>
        /// <exception cref="System.ArgumentException">An action with the same key already exists in the dictionary.</exception>
        public void Add(T _key, Action _action)
        {
            if (_action == null) throw new ArgumentNullException("The action cannot be null.");

            //

            m_dictionary.Add(_key, _action);
        }

        /// <summary>
        /// Removes the action with the specified key from the dictionary.
        /// </summary>
        /// <param name="_key">The key of the element to remove.</param>
        /// <returns>True if the element is successfully found and removed, otherwise false.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown, if the _key parameter is null.</exception>
        public bool Remove(T _key)
        {
            return m_dictionary.Remove(_key);
        }

        /// <summary>
        /// Gets or sets the action associated with the specified key.
        /// </summary>
        /// <param name="_key">The key of the action to get or set.</param>
        /// <returns>The value associated with the specified key. If the specified key is not found, a get operation throws a KeyNotFoundException, and a set operation creates a new element with the specified key.</returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Thrown by a get operation, if the specified key is not found.</exception>
        public Action this[T _key]
        {
            get
            {
                return m_dictionary[_key];
            }
            set
            {
                m_dictionary[_key] = value;
            }
        }

        //

        /// <summary>
        /// Executes the action, thats associated with the key.
        /// </summary>
        /// <param name="_key">The key of the action to execute.</param>
        /// <returns>True, if the associated action was found and executed, otherwise false.</returns>
        public bool Execute(T _key)
        {
            try
            {
                m_dictionary[_key]();

                // Only reached, if key is found
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Executes the action of the first key which is found in the dictionary.
        /// </summary>
        /// <param name="_arKeys">The keys of the actions to execute.</param>
        /// <returns>True, if an associated action was found and executed, otherwise false.</returns>
        public bool ExecuteFirst(T[] _arKeys)
        {
            foreach (T key in _arKeys)
            {
                try
                {
                    m_dictionary[key]();
                    
                    // Only reached, if key is found
                    return true;
                }
                catch
                {
                    // Ignore KeyNotFound exception
                }
            }

            // None of the keys is found
            return false;
        }

        /// <summary>
        /// Executes the actions of all of the keys.
        /// </summary>
        /// <param name="_arKeys">The keys of the actions to execute.</param>
        /// <returns>True, if at least one associated action was found and executed, otherwise false.</returns>
        public bool ExecuteAll(T[] _arKeys)
        {
            bool bKeyFound = false;

            foreach (T key in _arKeys)
            {
                try
                {
                    m_dictionary[key]();

                    bKeyFound = true;
                }
                catch
                {
                    // Ignore KeyNotFound exception
                }
            }

            return bKeyFound;
        }
    }
}
