
//

#include <Windows.h>

//

#include "exception.hpp"

//

namespace pw
{
    Win32Exception::Win32Exception()
        : std::exception([](){

            DWORD dwLastError = GetLastError();

            LPVOID lpMessageBuffer;
            FormatMessageA(FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS, NULL, dwLastError, MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), (LPSTR)&lpMessageBuffer, 0, NULL);

            std::string message = (LPSTR)lpMessageBuffer;

            LocalFree(lpMessageBuffer);

            return message;

        }().c_str())
    {
    }

    Win32Exception::Win32Exception(const std::string &_what) : std::exception(_what.c_str())
    {
    }

    //

    InvalidOperationException::InvalidOperationException() : logic_error("An invalid operation occured.")
    {
    }

    InvalidOperationException::InvalidOperationException(const std::string &_what) : logic_error("InvalidOperationException: " + _what)
    {
    }

    //

    ArgumentNullException::ArgumentNullException() : invalid_argument("Argument may not be null.")
    {
    }

    ArgumentNullException::ArgumentNullException(const std::string &_what) : invalid_argument("ArgumentNullException: " + _what)
    {
    }
}
