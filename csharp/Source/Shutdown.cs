using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace pw
{
    public static class ShutdownSystem
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct SingleTokenPrivilege
        {
            public int PrivilegeCount;
            public long Luid;
            public int Attributes;
        }

        /// <summary>
        /// Retrieves a pseudo handle for the current process
        /// </summary>
        /// <returns>The return value is a pseudo handle to the current process.</returns>
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetCurrentProcess();

        /// <summary>
        /// Opens the access token associated with a process.
        /// </summary>
        /// <param name="_ptrProcessHandle">A handle to the process whose access token is opened. The process must have the PROCESS_QUERY_INFORMATION access permission.</param>
        /// <param name="_iDesiredAccess">Specifies an access mask that specifies the requested types of access to the access token. These requested access types are compared with the discretionary access control list (DACL) of the token to determine which accesses are granted or denied.</param>
        /// <param name="_ptrTokenHandle">A pointer to a handle that identifies the newly opened access token when the function returns.</param>
        /// <returns>If the function succeeds, the return value is true, otherwise it is false.</returns>
        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern bool OpenProcessToken(IntPtr _ptrProcessHandle, int _iDesiredAccess, ref IntPtr _ptrTokenHandle);

        /// <summary>
        /// Retrieves the locally unique identifier (LUID) used on a specified system to locally represent the specified privilege name.
        /// </summary>
        /// <param name="_sSystemName">The name of the system on which the privilege name is retrieved. If an empty string is specified, the function attempts to find the privilege name on the local system.</param>
        /// <param name="_sName">The name of the privilege, as defined in the Winnt.h header file. For example, this parameter could specify the constant, SE_SECURITY_NAME, or its corresponding string, "SeSecurityPrivilege".</param>
        /// <param name="_lLuid">The variable that receives the LUID by which the privilege is known on the system specified by the _sSystemName parameter.</param>
        /// <returns>If the function succeeds, the return value is true, otherwise it is false.</returns>
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool LookupPrivilegeValue(string _sSystemName, string _sName, ref long _lLuid);

        /// <summary>
        /// Enables or disables privileges in the specified access token. Enabling or disabling privileges in an access token requires TOKEN_ADJUST_PRIVILEGES access.
        /// </summary>
        /// <param name="_ptrHandle">A handle to the access token that contains the privileges to be modified. The handle must have TOKEN_ADJUST_PRIVILEGES access to the token. If the PreviousState parameter is not NULL, the handle must also have TOKEN_QUERY access.</param>
        /// <param name="_bDisableAllPrivileges">Specifies whether the function disables all of the token's privileges. If this value is TRUE, the function disables all privileges and ignores the NewState parameter. If it is FALSE, the function modifies privileges based on the information pointed to by the NewState parameter.</param>
        /// <param name="_stpNewState">A pointer to a TOKEN_PRIVILEGES structure that specifies an array of privileges and their attributes. If the DisableAllPrivileges parameter is FALSE, the AdjustTokenPrivileges function enables, disables, or removes these privileges for the token.</param>
        /// <param name="_iBufferLength">Specifies the size, in bytes, of the buffer pointed to by the PreviousState parameter. This parameter can be zero if the PreviousState parameter is NULL.</param>
        /// <param name="_ptrPreviousState">A pointer to a buffer that the function fills with a TOKEN_PRIVILEGES structure that contains the previous state of any privileges that the function modifies. That is, if a privilege has been modified by this function, the privilege and its previous state are contained in the TOKEN_PRIVILEGES structure referenced by PreviousState. If the PrivilegeCount member of TOKEN_PRIVILEGES is zero, then no privileges have been changed by this function. This parameter can be NULL. If you specify a buffer that is too small to receive the complete list of modified privileges, the function fails and does not adjust any privileges. In this case, the function sets the variable pointed to by the ReturnLength parameter to the number of bytes required to hold the complete list of modified privileges.</param>
        /// <param name="_ptrReturnLength">A pointer to a variable that receives the required size, in bytes, of the buffer pointed to by the PreviousState parameter. This parameter can be NULL if PreviousState is NULL.</param>
        /// <returns>If the function succeeds, the return value is true, otherwise it is false.</returns>
        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern bool AdjustTokenPrivileges(IntPtr _ptrHandle, bool _bDisableAllPrivileges, ref SingleTokenPrivilege _stpNewState, int _iBufferLength, IntPtr _ptrPreviousState, IntPtr _ptrReturnLength);

        /// <summary>
        /// Logs off the interactive user, shuts down the system, or shuts down and restarts the system. It sends the WM_QUERYENDSESSION message to all applications to determine if they can be terminated.
        /// </summary>
        /// <param name="_iFlags">The shutdown type. This parameter must include one of the following values: EWX_HYBRID_SHUTDOWN, EWX_LOGOFF, EWX_POWEROFF, EWX_REBOOT, EWX_RESTARTAPPS and EWX_SHUTDOWN. This parameter can optionally include one of the following values: EWX_FORCE and EWX_FORCEIFHUNG.</param>
        /// <param name="_iReason">The reason for initiating the shutdown. This parameter must be one of the system shutdown reason codes.</param>
        /// <returns>If the function succeeds, the return value is true. Because the function executes asynchronously, true as return value indicates that the shutdown has been initiated. It does not indicate whether the shutdown will succeed. It is possible that the system, the user, or another application will abort the shutdown.</returns>
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern bool ExitWindowsEx(int _iFlags, int _iReason);

        private const int SE_PRIVILEGE_ENABLED = 0x00000002;
        private const int TOKEN_QUERY = 0x00000008;
        private const int TOKEN_ADJUST_PRIVILEGES = 0x00000020;
        private const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";

        private const int EWX_HYBRID_SHUTDOWN = 0x00400000;
        private const int EWX_LOGOFF = 0x00000000;
        private const int EWX_SHUTDOWN = 0x00000001;
        private const int EWX_REBOOT = 0x00000002;
        private const int EWX_FORCE = 0x00000004;
        private const int EWX_FORCEIFHUNG = 0x00000010;

        /// <summary>
        /// Attempts to initiate an ExitWindowsEx-Call and beforehand get all necessary priviliges.
        /// </summary>
        /// <param name="_iFlag">The shutdown type. This parameter must include one of the following values: EWX_HYBRID_SHUTDOWN, EWX_LOGOFF, EWX_POWEROFF, EWX_REBOOT, EWX_RESTARTAPPS and EWX_SHUTDOWN. This parameter can optionally include one of the following values: EWX_FORCE and EWX_FORCEIFHUNG.</param>
        /// <returns>If the function succeeds, the return value is true. Because the ExitWindowsEx function executes asynchronously, true as return value indicates that the shutdown has been initiated. It does not indicate whether the shutdown will succeed. It is possible that the system, the user, or another application will abort the shutdown.</returns>
        private static bool Shutdown(int _iFlag)
        {
            SingleTokenPrivilege tp;
            IntPtr hproc = GetCurrentProcess();
            IntPtr htok = IntPtr.Zero;
            
            //

            if (!OpenProcessToken(hproc, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, ref htok))
                return false;

            //

            tp.PrivilegeCount = 1;
            tp.Luid = 0;
            tp.Attributes = SE_PRIVILEGE_ENABLED;
            
            if (!LookupPrivilegeValue(null, SE_SHUTDOWN_NAME, ref tp.Luid))
                return false;

            //

            if (!AdjustTokenPrivileges(htok, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero))
                return false;

            //

            return ExitWindowsEx(_iFlag, 0);
        }

        public enum ShutdownForceType
        {
            DoNotForce = 0x00000000,
            Force = EWX_FORCE,
            ForceIfHung = EWX_FORCEIFHUNG
        }

        /// <summary>
        /// Opens up the shutdown window.
        /// </summary>
        public static void OpenShutdownWindow()
        {
            (new Shell32.Shell()).ShutdownWindows();
        }

        /// <summary>
        /// Attempts to shutdown the system.
        /// </summary>
        /// <param name="_bUseHybrid">Specifies if the hybrid-shutdown, which was introduced in Windows 8 and prepares the system for a faster startup should be used.</param>
        /// <param name="_force">Specifies if the shutdown should be forced. This can cause applications to lose data. Therefore, you should only use this flag in an emergency.</param>
        /// <exception cref="System.ComponentModel.Win32Exception">Thrown when some error occured during the shutdown attempt. This may be caused by missing privileges.</exception>
        public static void Shutdown(bool _bUseHybrid = false, ShutdownForceType _force = ShutdownForceType.DoNotForce)
        {
            if (_bUseHybrid)
            {
                if (!Shutdown(EWX_HYBRID_SHUTDOWN | EWX_SHUTDOWN | (int)_force))
                {
                    throw new Win32Exception();
                }
            }
            else
            {
                if (!Shutdown(EWX_SHUTDOWN | (int)_force))
                {
                    throw new Win32Exception();
                }
            }   
        }

        /// <summary>
        /// Attempts to restart the system.
        /// </summary>
        /// <param name="_force">Specifies if the shutdown should be forced. This can cause applications to lose data. Therefore, you should only use this flag in an emergency.</param>
        /// <exception cref="System.ComponentModel.Win32Exception">Thrown when some error occured during the restart attempt. This may be caused by missing privileges.</exception>
        public static void Reboot(ShutdownForceType _force = ShutdownForceType.DoNotForce)
        {
            if (!Shutdown(EWX_REBOOT | (int)_force))
            {
                throw new Win32Exception();
            }
        }

        /// <summary>
        /// Attempts to logoff the current user from the system.
        /// </summary>
        /// <param name="_force">Specifies if the logoff should be forced. This can cause applications to lose data. Therefore, you should only use this flag in an emergency.</param>
        /// <exception cref="System.ComponentModel.Win32Exception">Thrown when some error occured during the restart attempt. This may be caused by missing privileges.</exception>
        public static void LogOff(ShutdownForceType _force = ShutdownForceType.DoNotForce)
        {
            if (!Shutdown(EWX_LOGOFF | (int)_force))
            {
                throw new Win32Exception();
            }
        }
    }
}
