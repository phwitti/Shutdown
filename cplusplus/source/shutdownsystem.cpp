
//

#include <Windows.h>
#include <Shldisp.h>
#include <PowrProf.h>

//

#include "exception.hpp"
#include "shutdownsystem.hpp"

//

namespace pw
{
    void ShutdownSystem::openShutdownWindow()
    {
        IShellDispatch *ptrShellDispatch;

        CoInitializeEx(nullptr, COINIT_MULTITHREADED);
        if (SUCCEEDED(CoCreateInstance(CLSID_Shell, nullptr, CLSCTX_INPROC_SERVER, IID_PPV_ARGS(&ptrShellDispatch))))
        {
            ptrShellDispatch->ShutdownWindows();
        }
        CoUninitialize();
    }

    void ShutdownSystem::shutdown(bool _useHybrid, ShutdownForceType _force)
    {
        if (_useHybrid)
        {
            if (!shutdown(EWX_HYBRID_SHUTDOWN | EWX_SHUTDOWN | (int)_force))
            {
                throw Win32Exception();
            }
        }
        else
        {
            if (!shutdown(EWX_SHUTDOWN | (int)_force))
            {
                throw Win32Exception();
            }
        }
    }

    void ShutdownSystem::reboot(ShutdownSystem::ShutdownForceType _force)
    {
        if (!shutdown(EWX_REBOOT | (int)_force))
        {
            throw Win32Exception();
        }
    }

    void ShutdownSystem::logOff(ShutdownSystem::ShutdownForceType _force)
    {
        if (!shutdown(EWX_LOGOFF | (int)_force))
        {
            throw Win32Exception();
        }
    }

    void ShutdownSystem::standby(ShutdownSystem::ShutdownForceType _force)
    {
        if (!SetSuspendState(TRUE, _force == ShutdownSystem::ShutdownForceType::Force, FALSE))
        {
            throw Win32Exception();
        }
    }

    bool ShutdownSystem::shutdown(int _iFlag)
    {
        TOKEN_PRIVILEGES tp;
        HANDLE process = GetCurrentProcess();
        HANDLE token = nullptr;

        //

        if (!OpenProcessToken(process, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, &token))
            return false;

        //

        tp.PrivilegeCount = 1;
        tp.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;

        if (!LookupPrivilegeValueW(nullptr, SE_SHUTDOWN_NAME, &tp.Privileges[0].Luid))
            return false;

        // Get the shutdown privilege for this process.

        if (!AdjustTokenPrivileges(token, false, &tp, 0, (PTOKEN_PRIVILEGES)nullptr, 0))
            return false;

        //

        return (ExitWindowsEx(_iFlag, 0) == TRUE);
    }
}
