#ifndef EXCEPTION_HPP
#define EXCEPTION_HPP

//

#include <exception>
#include <string>

//

namespace pw
{

    class Contract
    {
        
    public:

        template<typename T>
        static void ensure(bool _contract, std::string _message)
        {
            if (!_contract) throw T(_message);
        }

    };

    class Win32Exception : public std::exception
    {

    public:

        Win32Exception();
        Win32Exception(const std::string &_what);

    };

    class InvalidOperationException : public std::logic_error
    {

    public:

        InvalidOperationException();
        InvalidOperationException(const std::string &_what);

    };

    class ArgumentNullException : public std::invalid_argument
    {

    public:

        ArgumentNullException();
        ArgumentNullException(const std::string &_what);

    };
}

//

#endif
