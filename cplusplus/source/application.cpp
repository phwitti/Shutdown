
//

#include <Windows.h>
#include <Shobjidl.h>

//

#include "application.hpp"
#include "exception.hpp"

//

namespace pw
{
    std::unique_ptr<Application> Application::current;

    //

    const std::wstring &Application::getFileName() const
    {
        return fileName;
    }

    const std::wstring &Application::getId() const
    {
        return id;
    }

    //

    void Application::initialize(const std::wstring &_id)
    {
        if (!current)
        {
            if (_id != std::wstring())
            {
                if (FAILED(SetCurrentProcessExplicitAppUserModelID(_id.c_str())))
                    throw pw::Win32Exception();
            }

            //

            current.reset(new Application());

            //

            WCHAR applicationPath[MAX_PATH];
            GetModuleFileNameW(nullptr, applicationPath, _countof(applicationPath));

            //

            current->fileName = applicationPath;
            current->id = _id;

        }
    }

    Application &Application::getCurrent()
    {
        initialize(std::wstring());

        return *current;
    }

    //

    Application::Application()
    {
    }
}
