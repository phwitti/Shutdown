using System;
using System.Text;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace pw
{
    class DynamicLibrary
    {
        /// <summary>
        /// Loads the specified module into the address space of the calling process. The specified module may cause other modules to be loaded.
        /// </summary>
        /// <param name="_sFileName">The name of the module. This can be either a library module (a .dll file) or an executable module (an .exe file). The name specified is the file name of the module and is not related to the name stored in the library module itself, as specified by the LIBRARY keyword in the module-definition (.def) file. If the string specifies a full path, the function searches only that path for the module. If the string specifies a relative path or a module name without a path, the function uses a standard search strategy to find the module; for more information, see the Remarks. If the function cannot find the module, the function fails. When specifying a path, be sure to use backslashes (\), not forward slashes (/). For more information about paths, see Naming a File or Directory. If the string specifies a module name without a path and the file name extension is omitted, the function appends the default library extension .dll to the module name. To prevent the function from appending .dll to the module name, include a trailing point character (.) in the module name string.</param>
        /// <returns>If the function succeeds, the return value is a handle to the module. If the function fails, the return value is null.</returns>
        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string _sFileName);

        /// <summary>
        /// Frees the loaded dynamic-link library (DLL) module and, if necessary, decrements its reference count. When the reference count reaches zero, the module is unloaded from the address space of the calling process and the handle is no longer valid.
        /// </summary>
        /// <param name="_ptrModule">A handle to the loaded library module. The LoadLibrary, LoadLibraryEx, GetModuleHandle, or GetModuleHandleEx function returns this handle.</param>
        /// <returns>If the function succeeds, the return value is true, otherwise it is false.</returns>
        [DllImport("kernel32.dll")]
        private static extern bool FreeLibrary(IntPtr _ptrModule);

        /// <summary>
        /// Loads a string resource from the executable file associated with a specified module, copies the string into a buffer, and appends a terminating null character.
        /// </summary>
        /// <param name="_ptrInstance">A handle to an instance of the module whose executable file contains the string resource.</param>
        /// <param name="_uID">The identifier of the string to be loaded.</param>
        /// <param name="_sbBuffer">The buffer is to receive the string. Must be of sufficient length to hold a pointer (8 bytes).</param>
        /// <param name="_iBufferMax">The size of the buffer, in characters. The string is truncated and null-terminated if it is longer than the number of characters specified. If this parameter is 0, then _sbBuffer receives a read-only pointer to the resource itself.</param>
        /// <returns>If the function succeeds, the return value is the number of characters copied into the buffer, not including the terminating null character, or zero if the string resource does not exist.</returns>
        [DllImport("user32.dll")]
        private static extern int LoadString(IntPtr _ptrInstance, uint _uID, StringBuilder _sbBuffer, int _iBufferMax);

        //

        private IntPtr m_ptrModule = IntPtr.Zero;
        
        /// <summary>
        /// Loads the specified module into the address space of the calling process. The specified module may cause other modules to be loaded.
        /// </summary>
        /// <param name="_sPath">The name of the module. This can be either a library module (a .dll file) or an executable module (an .exe file). The name specified is the file name of the module and is not related to the name stored in the library module itself, as specified by the LIBRARY keyword in the module-definition (.def) file. If the string specifies a full path, the function searches only that path for the module. If the string specifies a relative path or a module name without a path, the function uses a standard search strategy to find the module; for more information, see the Remarks. If the function cannot find the module, the function fails. When specifying a path, be sure to use backslashes (\), not forward slashes (/). For more information about paths, see Naming a File or Directory. If the string specifies a module name without a path and the file name extension is omitted, the function appends the default library extension .dll to the module name. To prevent the function from appending .dll to the module name, include a trailing point character (.) in the module name string.</param>
        /// <exception cref="System.ComponentModel.Win32Exception">Thrown when some error occured when loading the module.</exception>
        public DynamicLibrary(string _sPath)
        {
            Load(_sPath);
        }

        ~DynamicLibrary()
        {
            try
            {
                Unload();
            }
            catch
            {
                // Nothing to do
            }
        }

        /// <summary>
        /// Loads the specified module into the address space of the calling process. The specified module may cause other modules to be loaded.
        /// </summary>
        /// <param name="_sPath">The name of the module. This can be either a library module (a .dll file) or an executable module (an .exe file). The name specified is the file name of the module and is not related to the name stored in the library module itself, as specified by the LIBRARY keyword in the module-definition (.def) file. If the string specifies a full path, the function searches only that path for the module. If the string specifies a relative path or a module name without a path, the function uses a standard search strategy to find the module; for more information, see the Remarks. If the function cannot find the module, the function fails. When specifying a path, be sure to use backslashes (\), not forward slashes (/). For more information about paths, see Naming a File or Directory. If the string specifies a module name without a path and the file name extension is omitted, the function appends the default library extension .dll to the module name. To prevent the function from appending .dll to the module name, include a trailing point character (.) in the module name string.</param>
        /// <exception cref="System.ComponentModel.Win32Exception">Thrown when some error occured when loading the module.</exception>
        private void Load(string _sPath)
        {
            Unload();
            m_ptrModule = LoadLibrary(_sPath);

            if (m_ptrModule == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
        }

        /// <summary>
        /// Frees the loaded dynamic-link library (DLL) module and, if necessary, decrements its reference count.
        /// </summary>
        /// <exception cref="System.ComponentModel.Win32Exception">Thrown when some error occured when unloading the module.</exception>
        private void Unload()
        {
            if (m_ptrModule != IntPtr.Zero)
            {
                if (!FreeLibrary(m_ptrModule))
                {
                    throw new Win32Exception();
                }
                m_ptrModule = IntPtr.Zero;
            }
        }

        //

        /// <summary>
        /// Loads a string resource from the module
        /// </summary>
        /// <param name="_iID">The identifier of the string to be loaded.</param>
        /// <param name="_iMaxStringLength">The maximum string length. The string is truncated and null-terminated if it is longer than the number of characters specified.</param>
        /// <returns>If the function succeeds, it returns the read string. It returns an empty string otherwise.</returns>
        public string GetString(uint _iID, int _iMaxStringLength = 256)
        {
            if (m_ptrModule != null)
            {
                StringBuilder stringBuilder = new StringBuilder(_iMaxStringLength);
                LoadString(m_ptrModule, _iID, stringBuilder, stringBuilder.Capacity);

                return stringBuilder.ToString();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
