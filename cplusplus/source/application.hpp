#ifndef APPLICATION_HPP
#define APPLICATION_HPP

//

#include <string>
#include <memory>

//

namespace pw
{
    class Application
    {

    public:

        const std::wstring &getFileName() const;
        const std::wstring &getId() const;

        //

        static void initialize(const std::wstring &_id = L"");
        static Application &getCurrent();
        
    private:

        Application();

        //

        std::wstring fileName;
        std::wstring id;

        //

        static std::unique_ptr<Application> current;

    };
}

//

#endif
